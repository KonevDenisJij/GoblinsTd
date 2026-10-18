using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerUpgradeComponent : TowerComponentBase
{
    public float TowerLvl => _towerLvl;
    private float _towerLvl;
    public float MaxTowerLvl => _maxTowerLvl;
    private float _maxTowerLvl;
    public float MaxTowerHealth => Tower.Health + _towerLvl * _nextLvlHealth;
    private float _nextLvlHealth;

    public CustomStringBuilder Characteristics => _characteristics;
    private CustomStringBuilder _characteristics;
    public CustomStringBuilder NextLvlCharacteristics => _nextLvlCharacteristics;
    private CustomStringBuilder _nextLvlCharacteristics;

    public TowerUpgradeComponent(Tower tower, float maxTowerLvl = 10f, float nextLvlHealth = 20f)
    {
        _tower = tower;
        _maxTowerLvl = maxTowerLvl;
        _nextLvlHealth = nextLvlHealth;
        _towerLvl = 1;
    }

    public TowerUpgradeComponent(Tower tower, float maxTowerLvl, float nextLvlHealth, float towerLvl)
    {
        _tower = tower;
        _maxTowerLvl = maxTowerLvl;
        _nextLvlHealth = nextLvlHealth;
        _towerLvl = towerLvl;
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
    }

    public override void UpdateCharacteristics()
    {
        _characteristics = new CustomStringBuilder();
        _nextLvlCharacteristics = new CustomStringBuilder();

        _characteristics.AppendLine($"Health: {Tower.Health}", 0);
        _nextLvlCharacteristics.AppendLine($" + {_nextLvlHealth}", 0);

        foreach (var component in _tower.Components.Where(a => a != this))
        {
            component.UpdateCharacteristics();
        }

        for (int i = 0; i < _characteristics.Text.Count; i++)
        {
            if (_characteristics.Text.ContainsKey(i) && !_nextLvlCharacteristics.Text.ContainsKey(i))
            {
                _nextLvlCharacteristics.AppendLine("", i);
            }
        }
    }

    public override void UpgradeTower()
    {
        if (_towerLvl >= _maxTowerLvl)
        {
            return;
        }

        _towerLvl++;
        Tower.ChangeHealth(_nextLvlHealth);

        foreach (TowerComponentBase component in _tower.Components.Where(a => a != this))
        {
            component.UpgradeTower();
        }

        UpdateCharacteristics();
        if (_tower.TowerUpgradeComponent != null)
        {
            GUI.Instance.Charecteristic.UpgradeTowerDescription(_tower);
        }
    }
}
