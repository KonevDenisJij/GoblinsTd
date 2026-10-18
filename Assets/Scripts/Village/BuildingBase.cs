using UnityEngine;

[SelectionBase]
public class BuildingBase : MonoBehaviour
{
    private string _name;
    public string Name { get => _name; set => _name = value; }

    public int BuildingLevel { get => _buildingLevel; set => _buildingLevel = value; }
    private int _buildingLevel = 1;

    public BuildingType BuildingType { get => _buildingType; set => _buildingType = value; }
    private BuildingType _buildingType;

    public BuildingTile BuildingTile => _buildingTile;
    private BuildingTile _buildingTile;

    public BuildingFactory OriginFactory
    {
        get => _originFactory;
        set
        {
            Debug.Assert(_originFactory == null, "Redefined origin factory!");
            _originFactory = value;
        }
    }
    private BuildingFactory _originFactory;

    public void SetBuildingTile(BuildingTile tile)
    {
        _buildingTile = tile;
        transform.SetParent(tile.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
