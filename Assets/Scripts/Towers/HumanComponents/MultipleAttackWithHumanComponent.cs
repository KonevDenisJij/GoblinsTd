using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultipleAttackWithHumanComponent : WithHumanComponentBase
{
    public MultipleAttackWithHumanComponent(Tower instance)
    {
        _tower = instance;
        _maxAmountOfHumans = 3;
        _amountOfHumans = 0;
    }

    public override void AddHuman()
    {
        _amountOfHumans++;
        Debug.Log("Челиков: " + _amountOfHumans);
        Tower.StartFunctionateMode();

        if (_amountOfHumans > 1)
        {
            var comp = Tower.Components.FirstOrDefault(a => a is MortarAttackComponent) as MortarAttackComponent;
            Tower.Components.Add(new MortarAttackComponent(Tower, comp.ShotsPerSecond - 0.1f * (_amountOfHumans - 1), comp.ShellDamage, comp.ShellBlastRadius));
        }
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
    }

    public override void RemoveHuman()
    {
        _amountOfHumans--;
        Debug.Log("- челик. Всего челиков: " + _amountOfHumans + " из " + _maxAmountOfHumans);
    }

    public override void UpdateCharacteristics()
    {
    }

    public override void UpgradeTower()
    {
    }
}
