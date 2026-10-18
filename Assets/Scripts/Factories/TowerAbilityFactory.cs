using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Factories/TowerAbilityFactory")]
public class TowerAbilityFactory : AbilityFactoryBase
{
    [Header("Лазерная башня")]
    [SerializeField]
    private float _damageBoost = 20;
    [SerializeField]
    private float _attackBoostTime = 5;
    [SerializeField]
    private float _attackDelay = 5;
    [SerializeField]
    private float _rechargeTime = 30;
    [SerializeField]
    private Material _boostBeamMaterial;
    [SerializeField]
    private Material _normalBeamMaterial;
    [SerializeField]
    private Sprite _laserTowerImage;

    [Space(15)]
    [Header("Продажа башни")]
    [SerializeField]
    private Sprite _towerSellImage;

    [Space(15)]
    [Header("Улучшение башни")]
    [SerializeField]
    private Sprite _towerUpgradeImage;

    public TowerAbility Get(Tower tower)
    {
        switch (tower.TowerType)
        {
            case TowerType.Laser: return GetLaserTowerAbilitie(tower);
        }

        Debug.Log("ХУЙНЯ КАКАЯ-ТО!!!!");
        return null;
    }

    public TowerAbility GetFastSell(Tower tower)
    {
        TowerAbility ability = new TowerAbility();
        ability.Components = new List<AbilityComponentBase>();
        ability.TowerParent = tower;
        ability.AbilityIcon = _towerSellImage;

        ability.Components.Add(new FastTowerSellAbility(tower));
        return ability;
    }

    public TowerAbility GetFastUpgrade(Tower tower)
    {
        TowerAbility ability = new TowerAbility();
        ability.Components = new List<AbilityComponentBase>();
        ability.TowerParent = tower;
        ability.AbilityIcon = _towerUpgradeImage;

        ability.Components.Add(new FastTowerUpgradeAbility(tower));
        return ability;
    }

    private TowerAbility GetLaserTowerAbilitie(Tower tower)
    {
        TowerAbility ability = new TowerAbility();
        ability.Components = new List<AbilityComponentBase>();
        ability.TowerParent = tower;
        ability.AbilityIcon = _laserTowerImage;

        ability.Components.Add(new DamageBoosterAbilityComponent(
            tower.Components
            .First(a => a.GetType() == typeof(LaserAttackComponent)), 
            _damageBoost,
            _attackBoostTime,
            _attackDelay,
            _rechargeTime,
            _boostBeamMaterial,
            _normalBeamMaterial));

        return ability;
    }
}
