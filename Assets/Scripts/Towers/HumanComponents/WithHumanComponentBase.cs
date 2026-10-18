using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WithHumanComponentBase : TowerComponentBase
{
    public int AmountOfHumans => _amountOfHumans;
    protected int _amountOfHumans;
    public int MaxAmountOfHumans => _maxAmountOfHumans;
    protected int _maxAmountOfHumans;

    public void ShowUI()
    {

    }

    public abstract void AddHuman();

    public abstract void RemoveHuman();
}
