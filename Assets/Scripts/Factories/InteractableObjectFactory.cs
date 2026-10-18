using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Factories/InteractableObjectFactory")]
public class InteractableObjectFactory : GameObjectFactory
{
    [Header("Линия рендера")]
    [SerializeField]
    private Material _lineMaterial;

    [Header("Золотая шахта")]
    [SerializeField]
    private InteractableObject _goldMinePrefab;

    [Header("Сломанная лазерная башня")]
    [SerializeField]
    private InteractableObject _brokenLaserTowerPrefab;

    public InteractableObject Get(InteractableObjectType type)
    {
        switch (type)
        {
            case InteractableObjectType.GoldMine: return GetGoldMine();
            case InteractableObjectType.BrokenLaserTower: return GetBrokenLaserTower();
        }
        Debug.Assert(false, "Unsupported interactable object type: " + type);
        return null;
    }

    public float GetItemPrice(ItemType type)
    {
        return 0;
    }

    private InteractableObject GetGoldMine()
    {
        InteractableObject instance = CreateGameObjectInstance(_goldMinePrefab);
        instance.OriginFactory = this;
        instance.Components = new List<InteractableObjectComponentBase>();
        instance.Components.Add(new PlacementOnFreeSpaceComponent(instance));

        return instance;
    }

    private InteractableObject GetBrokenLaserTower()
    {
        InteractableObject instance = CreateGameObjectInstance(_brokenLaserTowerPrefab);
        instance.OriginFactory = this;

        return instance;
    }
}
