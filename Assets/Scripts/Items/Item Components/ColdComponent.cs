using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdItemComponent : ItemComponentBase
{
    private Item _item;
    private float _duration;

    public ColdItemComponent(Item instance, float duration)
    {
        _item = instance;
        _duration = duration;
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
            TargetPoint.GetBuffered(i).Enemy.AddEffect(new ColdEffect(), _duration);
        }
    }
}
