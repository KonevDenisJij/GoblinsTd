using UnityEngine;

[System.Serializable]
public struct BuildingStruct
{
    [SerializeField]
    private string _name;
    public string Name => _name;

    [SerializeField]
    private BuildingPreview _preview;
    public BuildingPreview Preview => _preview;

    [SerializeField]
    private BuildingBase _buildingBase;
    public BuildingBase BuildingBase => _buildingBase;

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon => _icon;

    [SerializeField]
    private BuildingType _buildingType;
    public BuildingType BuildingType => _buildingType;

    public BuildingStruct(string name, BuildingPreview preview, BuildingBase building, BuildingType buildingType, Sprite icon)
    {
        _name = name;
        _preview = preview;
        _buildingBase = building;
        _buildingType = buildingType;
        _icon = icon;
    }
}
