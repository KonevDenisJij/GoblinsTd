using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factories/ItemFactory")]
public class ItemFactory : GameObjectFactory
{
    [Header("Линия рендера")]
    [SerializeField]
    private Material _lineMaterial;

    [Header("Бомба")]
    [SerializeField]
    private Item _bombPrefab;
    [SerializeField]
    private float _bombRange;
    [SerializeField]
    private float _bombDamage;

    [Header("Ледяная бомба")]
    [SerializeField]
    private Item _coldBombPrefab;
    [SerializeField]
    private float _coldBombRange;
    [SerializeField]
    private float _coldBombDamage;
    [SerializeField]
    private float _coldBombDuration;



    public Item Get(ItemType type)
    {
        switch (type)
        {
            case ItemType.Bomb: return GetBomb();
            case ItemType.ColdBomb: return GetColdBomb();
        }
        Debug.Assert(false, "Unsupported tower type: " + type);
        return null;
    }

    public float GetItemPrice(ItemType type)
    {
        var item = Get(type);

        float price = item.Price;

        Reclaim(item);

        return price;
    }

    private Item GetBomb()
    {
        Item instance = CreateGameObjectInstance(_bombPrefab);
        instance.OriginFactory = this;
        instance.Components.Add(new ItemRadiusComponent(instance, _bombRange, _lineMaterial));
        instance.Components.Add(new ApplyItemDamageComponent(instance, _bombDamage));

        return instance;
    }

    private Item GetColdBomb()
    {
        Item instance = CreateGameObjectInstance(_coldBombPrefab);
        instance.OriginFactory = this;
        instance.Components.Add(new ItemRadiusComponent(instance, _coldBombRange, _lineMaterial));
        instance.Components.Add(new ApplyItemDamageComponent(instance, _coldBombDamage));
        instance.Components.Add(new ColdItemComponent(instance, _coldBombDuration));

        return instance;
    }
}
