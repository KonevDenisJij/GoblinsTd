using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private WalkableTile[] _gates;
    [SerializeField]
    private GameTileFactory _factory;
    [SerializeField]
    private InteractableObjectFactory _factory2;
    [SerializeField, DefaultValue(30)]
    private int _pathLength;

    public float[] MapSize => _mapSize;
    private float[] _mapSize = new float[4];

    private static Dictionary<int, WalkableTile> _path;
    private List<GameTile> _allTiles;

    public Dictionary<int, WalkableTile> Path => _path;
    public WalkableTile SpawnPoint => Path[1];

    [DefaultValue(false)]
    private bool _showPath;

    public void Initialize()
    {
        _path = new Dictionary<int, WalkableTile>();
        _allTiles = new List<GameTile>();
        GenerateMap();
    }

    public void ShowPath()
    {
        _showPath = !_showPath;
        foreach (var path in _path)
        {
            path.Value.ToggleArrows(_showPath);
        }
    }

    public static WalkableTile NextTileOnPath(int pathNumber)
    {
        return _path.GetValueOrDefault(pathNumber);
    }

    public GameTileContent GetTile(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.gameObject.GetComponentInParent<GameTileContent>();
        }

        return null;
    }
    public GameTileContent GetTile(Ray ray, int ignoreLayer)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, ignoreLayer))
        {
            return hit.collider.gameObject.GetComponentInParent<GameTileContent>();
        }

        return null;
    }

    public Enemy GetEnemy(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.gameObject.GetComponentInParent<Enemy>();
        }

        return null;
    }

    private void GenerateMap()
    {
        var forestPath = new ForestPath();

        int randomNumber = Random.Range(0, 4);
        _mapSize = forestPath.GeneratePath(_gates[randomNumber], randomNumber, _pathLength, _factory, out _path);
        _mapSize[0] += 2f;
        _mapSize[1] += 2f;
        _mapSize[2] -= 2f;
        _mapSize[3] -= 2f;

        for (float z = _mapSize[2]; z <= _mapSize[0]; z++)
        {
            for (float x = _mapSize[3]; x <= _mapSize[1]; x++)
            {
                if (PathHelper.CheckCollision(x, z, 1, out var buffer) == 0)
                {
                    _allTiles.Add(_factory.GetWithCollisionCheck(TileType.NotWalkable, new Vector3(x, 0f, z), Quaternion.Euler(0f, 90f, 0f)));
                }
            }
        }

        var rand = new System.Random();

        var numbOfObjects = rand.Next(4);

        for (int i = 0; i < numbOfObjects; i++)
        {
            GenerateRandomInteractableObject(rand);
        }
    }

    private void GenerateRandomInteractableObject(System.Random rand)
    {

        var randTileForGoldMine = _allTiles[rand.Next(_allTiles.Count - 1)];

        var mine = _factory2.Get(InteractableObjectType.GoldMine);

        var a = mine.Components.First(a => a is InteractableObjectPlacementComponentBase) as InteractableObjectPlacementComponentBase;

        var tile = a.PlaceObject(_allTiles, rand);
        tile.Occupied = true;
    }
}
