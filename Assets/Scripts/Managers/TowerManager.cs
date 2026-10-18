using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : DefaultManager
{
    [SerializeField]
    private TowerFactory _towerFactory;

    public List<Tower> Towers => _towers;
    private List<Tower> _towers = new List<Tower>();

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
        foreach(var tower in _towers)
        {
            tower.GameUpdate();
        }
    }

    public float GetTowerPrice(TowerType type)
    {
        return _towerFactory.GetTowerPrice(type);
    }

    public Tower GetTower(TowerType type)
    {
        return _towerFactory.Get(type);
    }

    public void AddTower(Tower tower)
    {
        _towers.Add(tower);
    }

    public void DestroyTower(Tower tower)
    {
        GUI.Instance.Charecteristic.CloseTab();
        tower.OriginalTile.DestroyContent();
        _towers = _towers.Where(a => a != tower).ToList();
        tower.OriginFactory.Reclaim(tower);
    }
}
