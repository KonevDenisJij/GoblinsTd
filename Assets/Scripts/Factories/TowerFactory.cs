using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Factories/TowerFactory")]
public class TowerFactory : GameObjectFactory
{
    [Header("Вспомогательные фабрики")]
    [SerializeField]
    private TowerAbilityFactory _abilitieFactory;
    [SerializeField]
    private UnitFactory _unitFactory;
    public UnitFactory UnitFactory => _unitFactory;

    [Space(10)]
    [Header("Лазерная башня")]
    [SerializeField]
    private Tower _laserTowerPrefab;
    [SerializeField, DefaultValue(20f)]
    private float _damagePerSecond;

    [Space(10)]
    [Header("Мортира")]
    [SerializeField]
    private Tower _mortarTowerPrefab;
    [SerializeField, DefaultValue(1f)]
    private float _shellBlastRadius;
    [SerializeField, DefaultValue(25f)]
    private float _shellDamage;
    [SerializeField, DefaultValue(1f)]
    private float _shotsPerSecond;

    [Space(10)]
    [Header("Шипы")]
    [SerializeField]
    private Tower _thornFloorPrefab;
    [SerializeField]
    private float _thornsDamage = 25f;
    [SerializeField]
    private float _rechargeTime = 5f;

    [Space(10)]
    [Header("Воздушный шар")]
    [SerializeField]
    private Tower _balloonTowerPrefab;
    [SerializeField]
    private float _balloonBlastRadius = 1f;
    [SerializeField]
    private float _balloonDamage = 50f;
    [SerializeField]
    private float _balloonShotsPerSecond = 1f;

    [Space(10)]
    [Header("Бараки рыцарей")]
    [SerializeField]
    private Tower _knightsTowerPrefab;
    [SerializeField]
    private int _amountOfKnights = 1;
    [SerializeField]
    private float _knightsSpawnSpeed = 3f;

    public Tower Get(TowerType type)
    {
        switch (type)
        {
            case TowerType.Laser: return GetLaserTower();
            case TowerType.Mortar: return GetMortarTower();
            case TowerType.ThornFloor: return GetThornFloor();
            case TowerType.Balloon: return GetBalloonTower();
            case TowerType.KnightsBarrack: return GetKnightsTower();
        }
        Debug.Log("Нет такой башни!!!: " + type);
        return null;
    }

    public float GetTowerPrice(TowerType type)
    {
        var tower = Get(type);

        float price = tower.Price;

        Reclaim(tower);

        return price;
    }

    private Tower GetLaserTower()
    {
        Tower instance = CreateGameObjectInstance(_laserTowerPrefab);
        instance.OriginFactory = this;

        instance.Components = new List<TowerComponentBase>();
        instance.Components.Add(new AcquireTargetComponent(instance));
        instance.Components.Add(new TrackTargetComponent(instance));
        instance.Components.Add(new LaserAttackComponent(instance, _damagePerSecond));
        instance.Components.Add(new TowerUpgradeComponent(instance));

        instance.Abilities = new List<TowerAbility>();
        instance.Abilities.Add(_abilitieFactory.Get(instance));

        return FinishTowerCreation(instance);
    }

    private Tower GetMortarTower()
    {
        Tower instance = CreateGameObjectInstance(_mortarTowerPrefab);
        instance.OriginFactory = this;

        instance.Components = new List<TowerComponentBase>();
        instance.Components.Add(new MortarAttackComponent(instance, _shotsPerSecond, _shellDamage, _shellBlastRadius));
        instance.Components.Add(new TowerUpgradeComponent(instance));
        instance.Components.Add(new MultipleAttackWithHumanComponent(instance));

        instance.Abilities = new List<TowerAbility>();

        return FinishTowerCreation(instance);
    }

    private Tower GetThornFloor()
    {
        Tower instance = CreateGameObjectInstance(_thornFloorPrefab);
        instance.OriginFactory = this;

        instance.Components = new List<TowerComponentBase>();
        instance.Components.Add(new FloorTowerTypeComponent(instance));
        instance.Components.Add(new FloorDamageComponent(instance, _thornsDamage, _rechargeTime));

        instance.Abilities = new List<TowerAbility>();

        return FinishTowerCreation(instance);
    }

    private Tower GetBalloonTower()
    {
        Tower instance = CreateGameObjectInstance(_balloonTowerPrefab);
        instance.OriginFactory = this;

        instance.Components = new List<TowerComponentBase>();
        instance.Components.Add(new FlyingTowerTypeComponent(instance));
        instance.Components.Add(new AcquireTargetComponent(instance));
        instance.Components.Add(new TrackTargetComponent(instance));
        instance.Components.Add(new BombDropAttackComponent(instance, _balloonShotsPerSecond, _balloonDamage, _balloonBlastRadius));

        instance.Abilities = new List<TowerAbility>();

        return FinishTowerCreation(instance);
    }

    private Tower GetKnightsTower()
    {
        Tower instance = CreateGameObjectInstance(_knightsTowerPrefab);
        instance.OriginFactory = this;

        instance.Components = new List<TowerComponentBase>();
        instance.Components.Add(new SpawnUnitComponentBase(instance, UnitType.Knight, _amountOfKnights, _knightsSpawnSpeed));

        instance.Abilities = new List<TowerAbility>();

        return FinishTowerCreation(instance);
    }

    private Tower FinishTowerCreation(Tower instance)
    {
        instance.Abilities.Add(_abilitieFactory.GetFastSell(instance));
        var upgComponent = instance.Components.FirstOrDefault(a => a.GetType() == typeof(TowerUpgradeComponent));
        if (upgComponent != null)
        {
            upgComponent.UpdateCharacteristics();
            instance.Abilities.Add(_abilitieFactory.GetFastUpgrade(instance));
        }

        Debug.Assert(instance.Abilities.Count <= 3, "ШТО-ТО ВЫ ПЕРЕБОРЩИЛИ С АБИЛКАМИ У БАШНИ " + instance.name);

        return instance;
    }
}