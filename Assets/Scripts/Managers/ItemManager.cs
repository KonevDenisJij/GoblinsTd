using UnityEngine;

public class ItemManager : DefaultManager
{
    [SerializeField]
    private ItemFactory _itemFactory;

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
        
    }

    public float GetItemPrice(ItemType type)
    {
        return _itemFactory.GetItemPrice(type);
    }

    public Item GetItem(ItemType type)
    {
        return _itemFactory.Get(type);
    }    
}
