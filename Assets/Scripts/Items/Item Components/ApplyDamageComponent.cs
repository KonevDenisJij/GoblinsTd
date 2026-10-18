using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ApplyItemDamageComponent : ItemComponentBase
{
    private Item _item;
    private float _damage;

    public ApplyItemDamageComponent(Item instance, float damage)
    {
        _item = instance;
        _damage = damage;
    }
    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
    }

    public override void ItemUse()
    {
        TargetPoint.FillBuffer(_item.transform.position, _item.Radius);
        for (int i = 0; i < TargetPoint.BufferedCount; i++)
        {
            TargetPoint.GetBuffered(i).Enemy.ApplyDamage(_damage);
        }
    }
}
