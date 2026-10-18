using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tower : GameTileContent
{
    public float Health => _health;
    [SerializeField]
    private float _health;

    public TowerType TowerType => _towerType;
    [SerializeField]
    private TowerType _towerType;

    public List<TowerComponentBase> Components { get => _components; set => _components = value; }
    private List<TowerComponentBase> _components;

    public TowerUpgradeComponent TowerUpgradeComponent => _components.FirstOrDefault(a => a.GetType() == typeof(TowerUpgradeComponent)) as TowerUpgradeComponent;

    public bool InBuildingMode { get => _inBuildingMode; set => _inBuildingMode = value; }
    private bool _inBuildingMode;

    public int KillCounter { get => _killCounterContainer.Value; set => _killCounterContainer.Value = value; }
    public Observable<int> _killCounterContainer = new Observable<int>();

    public CustomStringBuilder NextLvlCharacteristics => TowerUpgradeComponent?.NextLvlCharacteristics ?? new CustomStringBuilder();
    public virtual CustomStringBuilder Characteristics => TowerUpgradeComponent?.Characteristics ?? new CustomStringBuilder();

    public bool FloorType => _components.FirstOrDefault(a => a.GetType() == typeof(FloorTowerTypeComponent)) != null;

    public bool FlyingType => _components.FirstOrDefault(a => a.GetType() == typeof(FlyingTowerTypeComponent)) != null;

    public List<TowerAbility> Abilities { get => _abilities; set => _abilities = value; }
    private List<TowerAbility> _abilities;

    public bool CanFunctionate => _canFunctionate;
    private bool _canFunctionate = true;

    public void ChangeHealth(float damage)
    {
        _health += damage;
        if (_health <= 0)
        {
            DefenseController.Instance.TowerManager.DestroyTower(this);
        }
        else if (_health > TowerUpgradeComponent.MaxTowerHealth)
        {
            _health = TowerUpgradeComponent.MaxTowerHealth;
        }
    }

    public override void GameUpdate()
    {
        if (_inBuildingMode)
        {
            return;
        }

        foreach (var ability in _abilities)
        {
            foreach (var component in ability.Components)
            {
                component.GameUpdate();
            }
        }

        if (!_canFunctionate)
        {
            return;
        }

        foreach (TowerComponentBase component in _components)
        {
            component.GameUpdate();
        }
    }

    public void StartFunctionateMode()
    {
        _canFunctionate = true;
    }

    public void StopFunctionateMode()
    {
        _canFunctionate = false;
        //TODO: сразу останавливать всякие эффекты от бафов и замораживать кд бафов
    }

    public void UpgradeTower()
    {
        TowerUpgradeComponent.UpgradeTower();
    }

    public void UseAbility(int abilityNumber)
    {
        _abilities[abilityNumber].UseAbility();
    }

    /// <summary>
    /// true = можно строить, false = нельзя строить
    /// </summary>
    /// <returns>Можно ли строить</returns>
    public bool CheckPlacement(GameTile2 tile)
    {
        if (_components.Any(a => a is PositionCheckComponentBase))
        {
            var checkers = _components.Where(a => a is PositionCheckComponentBase)
                .Select(a => a as PositionCheckComponentBase);

            foreach (var checker in checkers)
            {
                if (!checker.CheckPosition(tile))
                {
                    return false;
                }
            }
        }

        return true;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        var radius = (_components.FirstOrDefault(a => a.GetType() == typeof(AcquireTargetComponent)) as AcquireTargetComponent)?.TargetingRange ?? 0f;
        Gizmos.DrawWireSphere(position, radius);
    }
}