using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    private BuildingFactory _buildingFactory;

    private BuildingTile _currentTile;

    public List<BuildingBase> Buildings => _buildings;
    private List<BuildingBase> _buildings = new List<BuildingBase>();

    public static BuildingManager Instance => _instance;
    private static BuildingManager _instance;

    void OnEnable()
    {
        _instance = this;

        var village = SaveSystem.LoadVillage();

        if (village != null)
        {
            LoadVillage(village);
        }
    }

    private Ray TouchRay
    {
        get
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return default;
            };

            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

    public List<BuildingStruct> AvailableBuildings()
    {
        return _buildingFactory.AvailableBuildings;
    }

    public void Build(BuildingType type)
    {
        var newBuilding = _buildingFactory.Get(type);
        newBuilding.SetBuildingTile(_currentTile);
        newBuilding.transform.localPosition = new Vector3(0f, 0f, 0f);
        newBuilding.transform.localScale = new Vector3(8f, 8f, 8f);

        _buildings.Add(newBuilding);
    }

    public VillageData PrepareSaveData()
    {
        var data = new VillageData();
        foreach(var building in _buildings)
        {
            data.Buildings.Add(new BuildingsData(building));
        }

        return data;
    }

    private void LoadVillage(VillageData data)
    {
        foreach(var building in data.Buildings)
        {
            _currentTile = GameObject.Find(building.BuildingPosition.ToString()).GetComponent<BuildingTile>();
            Build(GetTypeByNumber(building.BuildingNumber));
        }
    }

    private BuildingType GetTypeByNumber(int number)
    {
        switch (number)
        {
            case 0:
                return BuildingType.Armory;
            case 1:
                return BuildingType.Treasury;
            default:
                return BuildingType.Treasury;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
    }

    private void HandleTouch()
    {
        BuildingTile buildingTile = CastRay<BuildingTile>(TouchRay);
        BuildingBase buildingBase = CastRay<BuildingBase>(TouchRay);

        if (buildingTile != null && buildingTile.transform.childCount == 0)
        {
            _currentTile = buildingTile;
            BuildingGUI.Instance.StartBuildingProcess(buildingTile);
        } 
        else if (buildingBase != null)
        {
            BuildingGUI.Instance.OpenBuildingMenu(buildingBase);
        }
    }

    private T CastRay<T>(Ray ray) where T : MonoBehaviour
    {
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.GetComponentInParent<T>() != null)
        {
            return hit.collider.gameObject.GetComponentInParent<T>();
        }

        return null;
    }
}