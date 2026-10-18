using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class NewGameBoard : MonoBehaviour
{
    [SerializeField]
    private GameTile2 _finish;
    [SerializeField]
    private GameTile2 _start;
    [SerializeField]
    private Material _defaultMaterial;

    [SerializeField]
    private int _xSize = 28;
    [SerializeField]
    private int _zSize = 28;

    public GameTile2 Finish => _finish;

    public List<GameTile2> SpawnPoints => _spawnPoints;
    private List<GameTile2> _spawnPoints = new List<GameTile2>();

    public GameTile2[,] AllTiles => _allTiles;
    private GameTile2[,] _allTiles;

    [SerializeField]
    private GameTile2 _tilePrefab;

    public GameTile2 GetTile(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.gameObject.GetComponentInParent<GameTile2>() != null)
            {
                return hit.collider.gameObject.GetComponentInParent<GameTile2>();
            }
            else if (hit.collider.gameObject.GetComponentInParent<GameTileContent>() != null)
            {
                return hit.collider.gameObject.GetComponentInParent<GameTileContent>().OriginalTile;
            }
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

    public GameTile2 GetTileByCoordinates(int x, int z)
    {
        if (x < 0 || x >= _xSize || z < 0 || z >= _zSize)
            return null;

        return _allTiles[x, z];
    }

    public void Init()
    {
        /*if (_caveMap != null)
        {
            _allTiles = _caveMap.GameInit();

            var rand = new System.Random();
            var randX = rand.Next(_xSize);
            var randZ = rand.Next(_zSize);
            var newStart = Instantiate(_start, _allTiles[randX, randZ].transform.position, _allTiles[randX, randZ].transform.rotation, transform);
            Destroy(_allTiles[randX, randZ].gameObject);
            _spawnPoints.Add(newStart);

            _allTiles[randX, randZ] = newStart;

            _allTiles[(int)_finish.transform.position.x, (int)_finish.transform.position.z] = _finish;

            return;
        }*/
        //GenerateMap();
        FillMap();
    }

    public void SetDefaultMaterial()
    {
        foreach (var tile in _allTiles)
        {
            tile.GetComponentInChildren<Renderer>().material = _defaultMaterial;
        }
    }

    private void FillMap()
    {
        _allTiles = new GameTile2[28, 28];

        var allTiles = FindObjectsOfType<GameTile2>();

        for (int x = 0; x < 28; x++)
        {
            for (int z = 0; z < 28; z++)
            {
                _allTiles[x, z] = allTiles.SingleOrDefault(a => a.transform.position.x == x && a.transform.position.z == z);
            }
        }

        var start = FindObjectsOfType<GameTile2>().Single(a => a.tag == "Respawn");
        _spawnPoints.Add(start);

        _allTiles[(int)_finish.transform.position.x, (int)_finish.transform.position.z] = _finish;

        FindObjectsOfType<GameTile2>()
            .Where(a => a.tag == "Road")
            .ToList()
            .ForEach(a => a.ChangeCost(-1f));
        
    }

    private void GenerateMap()
    {
        _allTiles = new GameTile2[_xSize, _zSize];

        for (int x = 0; x < _xSize; x++)
        {
            for (int z = 0; z < _zSize; z++)
            {
                if (PathHelper.CheckCollision(x, z, 1, out var buffer) == 0)
                {
                    var newTile = Instantiate(_tilePrefab);
                    newTile.OnCreate(x, z);
                    newTile.transform.SetParent(this.transform);
                    _allTiles[x,z] = newTile;
                }
            }
        }

        var rand = new System.Random();
        var randX = rand.Next(_xSize);
        var randZ = rand.Next(_zSize);
        var newStart = Instantiate(_start, _allTiles[randX, randZ].transform.position, _allTiles[randX, randZ].transform.rotation, transform);
        Destroy(_allTiles[randX, randZ].gameObject);
        _spawnPoints.Add(newStart);

        _allTiles[randX, randZ] = newStart;

        _allTiles[(int)_finish.transform.position.x, (int)_finish.transform.position.z] = _finish;

        var numbOfObjects = rand.Next(4);

        for (int i = 0; i < numbOfObjects; i++)
        {
            GenerateRandomInteractableObject(rand);
        }
    }

    private void GenerateRandomInteractableObject(System.Random rand)
    {
        var randX = rand.Next(_xSize);
        var randZ = rand.Next(_zSize);
        var tile = _allTiles[randX, randZ];

        DefenseController.Instance.InteractableObjectManager.CreateObject(InteractableObjectType.GoldMine, tile);
    }
}
