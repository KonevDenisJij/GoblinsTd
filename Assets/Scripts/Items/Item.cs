using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : GameTileContent
{
    [SerializeField]
    private ItemType _itemType;
    public ItemType ItemType => _itemType;

    public List<ItemComponentBase> Components { get => _components; set => _components = value; }
    private List<ItemComponentBase> _components = new List<ItemComponentBase>();
    public float Radius { get => _radius; set => _radius = value; }
    private float _radius;

    public override void GameUpdate()
    {
        /*foreach (ItemComponentBase component in _components)
        {
            component.GameUpdate(); //в компонентах нет ни одного геймапдейта, поэтому я хз надо ли оно. 
        }*/ //Если вдруг появится какой-то предмет типо тотем который хилит башни в радиусе, то это скорее WarEntity будет, а не предмет
    }

    public void UseItem()
    {
        foreach(ItemComponentBase component in _components)
        {
            component.ItemUse();
        }
    }
}