using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmmunitionManager : DefaultManager
{
    [SerializeField]
    private WarFactory _warFactory;

    private List<WarEntity> _ammunition = new List<WarEntity>();

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
        var tempList = new List<WarEntity>(_ammunition);
        foreach(var entity in tempList)
        {
            if (!entity.GameUpdate())
            {
                DestroyWarEntity(entity);
            }
        }
    }

    public WarEntity SpawnWarEntity(AmmunitionType type)
    {
        WarEntity warEntity = _warFactory.Get(type);
        _ammunition.Add(warEntity);
        return warEntity;
    }

    private void DestroyWarEntity(WarEntity entity)
    {
        _ammunition.Remove(entity);
        entity.Recycle();
    }
}
