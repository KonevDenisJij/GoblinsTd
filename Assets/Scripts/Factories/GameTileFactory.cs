using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Factories/GameTileFactory")]
public class GameTileFactory : GameObjectFactory
{
    [SerializeField]
    private GameTile _destinationPrefab = default;
    [SerializeField]
    private GameTile _walkableTilePrefab = default;
    [SerializeField]
    private GameTile _notWalkableTilePrefab = default;
    [SerializeField]
    private GameTile _spawnPointPrefab = default;

    public GameTile Get(TileType type)
    {
        switch (type)
        {
            case TileType.Walkable: return Get(_walkableTilePrefab);
            case TileType.NotWalkable: return Get(_notWalkableTilePrefab);
            case TileType.Spawn: return Get(_spawnPointPrefab);
            case TileType.Finish: return Get(_destinationPrefab);
        }
        Debug.Assert(false, "Unsupported non-tower type: " + type);
        return null;
    }
    public GameTile GetWithCollisionCheck(TileType type, Vector3 position, Quaternion rotation)
    {
        if (SceneManager.GetSceneByName(name).GetRootGameObjects().ToList().Any(a => a.transform.position == position))
        {
            return null;
        }

        var tile = Get(type);
        tile.transform.position = position;
        tile.transform.rotation = rotation;

        return tile;
    }

    private T Get<T>(T prefab) where T : GameTileContent
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }
}
