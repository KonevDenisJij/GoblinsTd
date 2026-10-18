using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Factories/BuildingFactory")]
public class BuildingFactory : GameObjectFactory
{
    [SerializeField]
    private List<BuildingStruct> _availableBuildings;
    public List<BuildingStruct> AvailableBuildings => _availableBuildings;

    public BuildingBase Get(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.Armory:
                return GetArmory(_availableBuildings.First(a => a.BuildingType == BuildingType.Armory));
            case BuildingType.Treasury:
                return GetTreasury(_availableBuildings.First(a => a.BuildingType == BuildingType.Treasury));
        }
        Debug.Assert(false, "Unsupported tower type: " + type);
        return null;
    }

    private BuildingBase GetArmory(BuildingStruct prefab)
    {
        var building = CreateGameObjectInstance(prefab.BuildingBase);
        building.Name = prefab.Name;
        building.BuildingType = prefab.BuildingType;
        return building;
    }

    private BuildingBase GetTreasury(BuildingStruct prefab)
    {
        var building = CreateGameObjectInstance(prefab.BuildingBase);
        building.Name = prefab.Name;
        building.BuildingType = prefab.BuildingType;
        return building;
    }

    public void Reclaim(BuildingBase building)
    {
        Debug.Assert(building.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(building.gameObject);
    }
}