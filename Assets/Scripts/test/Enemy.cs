using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TriangleNet.Topology;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour, ISelectableObject
{
    public string Name => _name;
    [SerializeField]
    private string _name;
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
    [SerializeField]
    private CustomStringBuilder _characteristics;
    public string Description => _description;
    [SerializeField]
    private string _description;

    public EnemyFactory OriginFactory
    {
        get => _originFactory;
        set
        {
            Debug.Assert(_originFactory == null, "Redefined origin factory!");
            _originFactory = value;
        }
    }
    private EnemyFactory _originFactory;

    public GameTile2 CurrentTile => _currentTile;
    private GameTile2 _currentTile;
    private GameTile2 _nextTile;

    private Stack<GameTile2> _currentPath;
    private float _movementProgress = 0f;
    private float _progressFactor = 1f;
    private float _speed = 1f;

    private float _health = 100f;
    public bool Death => _death;
    private bool _death = false;

    delegate void Behavior();
    private Behavior _behavior;

    public bool PathUpdated { get => _pathUpdated; set => _pathUpdated = value; }

    private bool _pathUpdated;

    private float _prevAngle = 0f;
    private float _nextAngle = 0f;

    private float _buildingDamage = 10f;
    private float _attackDelay = 0f;

    private List<EffectBase> _effects = new List<EffectBase>();

    private void Move() => transform.localPosition = Vector3.LerpUnclamped(_currentTile.transform.position, _nextTile.transform.position, _movementProgress);
    private void Turn() => transform.localRotation = Quaternion.Euler(0f, Mathf.LerpUnclamped(_prevAngle, _nextAngle, _movementProgress * 2 >= 1 ? 1 : _movementProgress * 2), 0f);
    private void Intro() => transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, _movementProgress);

    public void ChangeSpeed(float speed) 
    { 
        _speed += speed;

        if (_speed < 0f)
        {
            _speed = 0f;
        }
        else if (_speed > 3f)
        {
            _speed = 3f;
        }
    }

    public void SetSpeed(float speed)
    {
        if (speed < 0.0001f)
        {
            _speed = 0f;
        }
        else if (speed >= 3f)
        {
            _speed = 3f;
        }

        _speed = speed;
    }

    public void ApplyDamage(float damage, Tower tower = null)
    {
        _health -= damage;

        if (_health <= 0f)
        {
            if (tower != null)
            {
                tower.KillCounter += 1;
            }

            _movementProgress = 0f;
            Destroy(GetComponentInChildren<TargetPoint>());
            _death = true;
        }
    }

    public void Init(GameTile2 start, GameTile2 finish)
    {
        SetCurrentTile(start);
        FindPath(start, finish);
        _pathUpdated = false;      
    }

    public void UpdateEnemy()
    {
        UpdateMovement();
    }

    public void FindPath(GameTile2 start, GameTile2 finish)
    {
        var newPath = PathfindingExample.FindPath(start, finish);
        CreatePath(newPath);
    }

    public void FindPath(GameTile2 finish)
    {
        var newPath = PathfindingExample.FindPath(_currentTile, finish);
        UpdatePath(newPath);
    }

    private void CreatePath(Stack<GameTile2> newPath)
    {
        _currentPath = newPath;
        _currentPath.Pop();
        SetNextTile(_currentPath.Peek());
        transform.localEulerAngles = new Vector3(0f, CalculateAngelTo(), 0f);
        _behavior = Intro;
    }

    public void UpdatePath(Stack<GameTile2> newPath)
    {
        _currentPath = new Stack<GameTile2>(newPath.Reverse());
        _currentPath.Pop();
        if (_currentPath.Count == 0)
        {
            return;
        }

        if (_currentPath.Peek() == _nextTile && _behavior == Move)
        {
            _currentPath.Pop();
            return;
        }

        var tempTile = _currentTile;
        SetCurrentTile(_nextTile);
        SetNextTile(tempTile);
        transform.localEulerAngles = new Vector3(0f, CalculateAngelTo(), 0f);
        _movementProgress = 1 - _movementProgress;
    }

    public void AddEffect(EffectBase effect, float duration)
    {
        _effects.Add(effect);
        effect.EffectStart(this, duration);
    }

    private void UpdateMovement()
    {
        _movementProgress += Time.deltaTime * _progressFactor * _speed;

        if (_currentTile == DefenseController.Instance.Board.Finish || _death)
        {
            Outro();
            return;
        }

        if (_movementProgress >= 1f)
        {
            _movementProgress = 0f;
            _behavior = PrepareNextState();
        }

        if (_behavior != null)
        {
            _behavior();
        }
    }

    private Behavior PrepareNextState()
    {
        _currentPath.TryPeek(out var tempTile);
        if (_nextTile != tempTile)
        {
            SetCurrentTile(_nextTile);
            _currentPath.TryPeek(out _nextTile);
        }

        if (_nextTile == null)
        {
            return null;
        }

        return SelectNextStage();
    }

    private Behavior SelectNextStage()
    {
        Behavior result;

        if (_currentTile.transform.position + transform.forward != _nextTile.transform.position)
        {
            _prevAngle = transform.localEulerAngles.y;
            _nextAngle = CalculateAngelTo();
            result = Turn;
        }
        else if (_nextTile.Content != null 
            && (_nextTile.Content is Tower) 
            && !((_nextTile.Content as Tower).FloorType || (_nextTile.Content as Tower).FlyingType))
        {
            _attackDelay = 0f;
            result = AttackTower;
        }
        else
        {
            SetNextTile(_currentPath.Pop());
            result = Move;
        }

        return result;
    }

    private float CalculateAngelTo()
    {
        var vectorForward = _nextTile.transform.position - _currentTile.transform.position;

        if (vectorForward == Vector3.forward)
        {
            if (_prevAngle == 270f)
            {
                return 360f;
            }
            return 0f;
        }
        else if(vectorForward == Vector3.right)
        {
            return 90f;
        }
        else if(vectorForward == Vector3.back)
        {
            return 180f;
        }
        else
        {
            if (_prevAngle == 180f)
            {
                return 270f;
            }
            return -90f;
        }
    }

    private void SetCurrentTile(GameTile2 tile)
    {
        tile.AddEnemies(this);
        if (_currentTile != null && _currentTile.Enemies.Contains(this))
        {
            _currentTile.RemoveEnemies(this);
        }
        _currentTile = tile;
    }

    private void SetNextTile(GameTile2 tile)
    {
        _nextTile = tile;
    }

    private void Outro()
    {
        transform.localScale = Vector3.LerpUnclamped(Vector3.one, Vector3.zero, _movementProgress);

        if (_movementProgress >= 1)
        {
            DefenseController.Instance.EnemyManager.DestroyEnemy(this);
        }
    }

    private void AttackTower()
    {
        _attackDelay += Time.deltaTime;
        var tower = _nextTile.Content as Tower;
        if (_attackDelay > 0.5f && _nextTile.Content != null)
        {
            tower.ChangeHealth(-_buildingDamage);
            _attackDelay = 0f;
        }
    }
}