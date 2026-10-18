/*using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OldEnemy : GameBehavior, ISelectableObject
{
    [SerializeField]
    private Transform _model = default;
    [SerializeField]
    private float _health = 100f;
    [SerializeField]
    private Material _coldMaterial;
    [SerializeField]
    private float _towerDamage;
    [SerializeField, Min(0)]
    private int _healthDamage = 1;
    [SerializeField]
    private float _attackDelay;
    public Material ColdMaterial => _coldMaterial;


    //movement
    private WalkableTile _tileFrom, _tileTo;
    private Vector3 _positionFrom, _positionTo;
    private float _progress, _progressFactor, _pathOffset;

    public EnemyType EnemyType => _enemyType;
    [SerializeField]
    private EnemyType _enemyType;

    [Min(0f)]
    private float _speed = 1f;
    public float Speed { get => _speed; set => _speed = value; }
    public float Health => _health;

    //rotation
    private Direction _direction;
    private DirectionChange _directionChange;
    private float _directionAngleFrom, _directionAngleTo;
    private int _tileNumber;

    //Effects
    private List<EffectBase> _effects = new List<EffectBase>();

    //Attack
    private float _timeTillNextAttack;


    //ISelectableObject
    public string Name => _enemyName;
    [SerializeField]
    private string _enemyName;

    public CustomStringBuilder Characteristics
    {
        get
        {
            if (_characteristics == null)
            {
                _characteristics = new CustomStringBuilder();
            }
            return _characteristics;
        }
        set => _characteristics = value;
    }
    private CustomStringBuilder _characteristics;

    public string Description => _description;
    [SerializeField]
    private string _description;

    public void AddEffect(EffectBase effect, float duration)
    {
        _effects.Add(effect);
        effect.EffectStart(this, duration);
    }

    public void SpawnOn(int tileNumber)
    {
        _tileNumber = tileNumber;
        Debug.Assert(tileNumber > 0, "Nowhere to go!", this);
        _tileFrom = GameBoard.NextTileOnPath(_tileNumber);
        _tileTo = GameBoard.NextTileOnPath(++_tileNumber);
        _progress = 0f;
        PrepareIntro();
    }

    public override bool GameUpdate()
    {
        if (_health <= 0f)
        {
            if (ObjectSelection.SelectedObject as OldEnemy == this)
            {
                GUI.Instance.Charecteristic.CloseTab();
            }
            return false;
        }

        foreach (var effect in _effects)
        {
            effect.GameUpdate(this);

            if (effect.Duration <= 0f)
            {
                effect.EffectEnd(this);
            }
        }
        _effects = _effects.Where(a => a.Duration > 0f).ToList();

        _timeTillNextAttack = _timeTillNextAttack <= 0f ? 0f : _timeTillNextAttack - Time.deltaTime;

        _progress += Time.deltaTime * _progressFactor * _speed;
        while (_progress >= 1f)
        {
            if (_tileTo == null)
            {
                return false;
            }
            _progress = (_progress - 1f) / _progressFactor;
            PrepareNextState();
            _progress *= _progressFactor;
        }
        if (_directionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(
                _directionAngleFrom, _directionAngleTo, _progress
            );
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }

        return true;
    }
    public void ApplyDamage(float damage)
    {
        Debug.Assert(damage >= 0f, "Negative damage applied.");
        _health -= damage;
    }

    public void Initialize(float pathOffset)
    {
        _pathOffset = pathOffset;
    }

    public void SetSpeed(EnemySpeed speed)
    {
        switch (speed)
        {
            case EnemySpeed.VerySlow:
                _speed = 0.25f;
                return;
            case EnemySpeed.Slow:
                _speed = 0.5f;
                return;
            case EnemySpeed.Normal:
                _speed = 1f;
                return;
            case EnemySpeed.Fast:
                _speed = 1.5f;
                return;
            case EnemySpeed.VeryFast:
                _speed = 2f;
                return;
        }
    }

    private void PrepareNextState()
    {
        _tileFrom = _tileTo;
        _tileTo = GameBoard.NextTileOnPath(++_tileNumber);

        _positionFrom = _positionTo;


        if (_tileTo == null)
        {
            //GameManag.Instance.ChangeHealth(-_healthDamage);
            PrepareOutro();
            return;
        }

        _positionTo = _tileTo.transform.localPosition;

        //_directionChange = _direction.GetDirectionChangeTo(_tileTo.Directions[_tileNumber]);//усимъ 
        _direction = _tileTo.Directions[_tileNumber];
        _directionAngleFrom = _directionAngleTo;


        switch (_directionChange)
        {
            case DirectionChange.None: PrepareForward(); break;
            case DirectionChange.TurnRight: PrepareTurnRight(); break;
            case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
            default: PrepareTurnAround(); break;
        }
    }

    private void PrepareIntro()
    {
        _positionFrom = _tileFrom.transform.localPosition;
        _positionTo = _tileTo.transform.localPosition;
        _direction = _tileTo.Directions[_tileNumber];
        _directionChange = DirectionChange.None;
        _model.localPosition = new Vector3(_pathOffset, 0f, 0f);
        _directionAngleFrom = _directionAngleTo = _direction.GetAngle();
        transform.localRotation = _direction.GetRotation();
        _progressFactor = 2f;
    }
    private void PrepareOutro()
    {
        _positionTo = _tileFrom.transform.localPosition;
        _directionChange = DirectionChange.None;
        _directionAngleTo = _direction.GetAngle();
        _model.localPosition = new Vector3(_pathOffset, 0f, 0f);
        transform.localRotation = _direction.GetRotation();
        _progressFactor = 2f;
    }
    private void PrepareForward()
    {
        transform.localRotation = _direction.GetRotation();
        _directionAngleTo = _direction.GetAngle();
        _model.localPosition = new Vector3(_pathOffset, 0f, 0f);
        _progressFactor = 1f;
    }

    private void PrepareTurnRight()
    {
        _directionAngleTo = _directionAngleFrom + 90f;
        _model.localPosition = new Vector3(_pathOffset - 0.5f, 0f, 0f);
        transform.localPosition = _positionFrom + _direction.GetHalfVector();
        _progressFactor = 1f / (Mathf.PI * 0.5f * (0.5f - _pathOffset));
    }

    private void PrepareTurnLeft()
    {
        _directionAngleTo = _directionAngleFrom - 90f;
        _model.localPosition = new Vector3(_pathOffset + 0.5f, 0f, 0f);
        transform.localPosition = _positionFrom + _direction.GetHalfVector();
        _progressFactor = 1f / (Mathf.PI * 0.5f * (0.5f + _pathOffset));
    }

    private void PrepareTurnAround()
    {
        _directionAngleTo = _directionAngleFrom + 180f;
        _model.localPosition = new Vector3(_pathOffset, 0f, 0f);
        transform.localPosition = _positionFrom;
        _progressFactor = 2f;
    }
}*/