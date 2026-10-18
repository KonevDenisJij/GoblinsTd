using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factories/UnitFactory")]
public class UnitFactory : GameObjectFactory
{
    [Header("Рыцарь")]
    [SerializeField]
    private Unit _knightPrefab;

    [SerializeField, FloatRangeSlider(-0.4f, 0.4f)]
    private FloatRange _pathOffset = new FloatRange(0f);

    public Unit Get(UnitType type, GameTile2 tile)
    {
        switch (type)
        {
            case UnitType.Knight : return GetKnight(tile);
        }

        Debug.Log("Ошибка!!! Нет такого юнита");

        return null;
    }

    private Unit GetKnight(GameTile2 tile)
    {
        Unit instance = CreateGameObjectInstance(_knightPrefab);
        instance.OriginFactory = this;
        instance.Initialize(_pathOffset.RandomValueInRange, tile);

        return instance;
    }

    public void Reclaim(Unit enemy)
    {
        Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(enemy.gameObject);
    }
}
