using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTowerUpgradeAbility : AbilityComponentBase
{
    private Tower _tower;

    public FastTowerUpgradeAbility(Tower tower)
    {
        _tower = tower;
    }

    public override void AbilityUse()
    {
        _tower.UpgradeTower();
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
    }
}
