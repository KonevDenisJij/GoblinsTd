using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Factories/WarFactory")]
public class WarFactory : GameObjectFactory
{
    [SerializeField]
    private MortarShell _shellPrefab = default;
    [SerializeField]
    private Explosion _explosionPrefab = default;

    public WarEntity Get(AmmunitionType type)
    {
        WarEntity prefab = _shellPrefab;
        switch (type)
        {
            case AmmunitionType.Explosion: 
                prefab = _explosionPrefab;
                break;
            case AmmunitionType.MortarShell: 
                prefab = _shellPrefab;
                break;
        }
        WarEntity instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }

    public void Reclaim(WarEntity entity)
    {
        Debug.Assert(entity.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(entity.gameObject);
    }
}