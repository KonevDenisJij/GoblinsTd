using System.Linq;
using UnityEngine;

public class SpawnUnitComponentBase : TowerComponentBase
{
    private GameTile2 _tile;
    private UnitType _unitType;
    private int _amountOfUnits;
    private float _spawnSpeed;

    private float _timer = 0;

    public SpawnUnitComponentBase(Tower tower, UnitType unitType, int amountOfUnits, float spawnSpeed)
    {
        _tower = tower;
        _unitType = unitType;
        _amountOfUnits = amountOfUnits;
        _spawnSpeed = spawnSpeed;
    }

    public override void GameAwake()
    {
        _tile = _tower.OriginalTile;
    }

    public override void GameUpdate()
    {
        _timer += Time.deltaTime;

        if (_timer > _spawnSpeed)
        {
            _timer = 0;
            SpawnUnit();
        }
    }

    public override void UpdateCharacteristics()
    {
    }

    public override void UpgradeTower()
    {
    }

    private void SpawnUnit()
    {
        (_tower.OriginFactory as TowerFactory).UnitFactory.Get(_unitType, _tile);
    }
}
