using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTowerSellAbility : AbilityComponentBase
{
    private Tower _tower;

    public FastTowerSellAbility(Tower tower)
    {
        _tower = tower;
    }

    public override void AbilityUse()
    {
        DefenseController.Instance.TowerManager.DestroyTower(_tower);
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
    }
}
