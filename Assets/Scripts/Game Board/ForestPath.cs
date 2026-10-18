using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestPath
{
    private Dictionary<int, WalkableTile> _path;

    private  WalkableTile _lastTile;
    private float _lastX;
    private float _lastZ;

    private float[] _pathSize = new float[4]; // 0 - Север, 1 - Восток, 2 - Юг, 3 - Запад 

    private GameTileFactory _factory;

    private static Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);

    public float[] GeneratePath(WalkableTile gate, int randNumber, int pathLength, GameTileFactory factory, out Dictionary<int, WalkableTile> path)
    {
        Direction randomDirection;
        var randomVector = new Vector3(0f, 0f, 0f);
        _path = new Dictionary<int, WalkableTile>();
        _factory = factory;

        gate.gameObject.SetActive(true);
        gate.Directions = new Dictionary<int, Direction>();

        if (randNumber == 0)
        {
            randomVector.z = 3f;
            randomDirection = Direction.South;
        }
        else if (randNumber == 1)
        {
            randomVector.x = 3f;
            randomDirection = Direction.West;
        }
        else if (randNumber == 2)
        {
            randomVector.z = -3f;
            randomDirection = Direction.North;
        }
        else
        {
            randomVector.x = -3f;
            randomDirection = Direction.East;
        }

        _lastTile = gate;
        _lastTile.Directions.Add(pathLength, randomDirection);
        _path.Add(pathLength, _lastTile);

        for (int i = pathLength - 1; i > 0; i--)
        {
            if (i == pathLength - 1)
            {
                _lastTile = _factory.Get(TileType.Walkable) as WalkableTile;
                _lastTile.transform.position = randomVector;
                _lastTile.transform.rotation = rotation;

                _lastTile.Directions = new Dictionary<int, Direction>();

                _pathSize = PathHelper.CheckMapSize(_pathSize, _lastTile);

                _lastTile.Directions.Add(i, randomDirection);
                _path.Add(i, _lastTile);

                _lastX = _lastTile.transform.position.x;
                _lastZ = _lastTile.transform.position.z;

                continue;
            }

            CreateNewTile(_lastTile.Directions[i + 1], i);
        }

        foreach (var tile in _path)
        {
            tile.Value.SetArrowRotation();
        }

        foreach (var tile in _path)
        {
            var allTileMentions = _path.Where(a => a.Value == tile.Value);

            List<Direction> incomingDirections = new List<Direction>();
            foreach(var direction in allTileMentions)
            {
                if (direction.Key > 1)
                {
                    incomingDirections.Add(_path[direction.Key - 1].Directions[direction.Key - 1]);
                }
            }

            var tempDir = incomingDirections;
            tile.Value.SetRoad(tempDir.Distinct().ToList());
        }

        path = _path;
        return _pathSize;
    }

    private void CreateNewTile(Direction dir, int tileNumber)
    {
        var allDirections = new List<Direction>
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West
        };

        allDirections = allDirections.Where(a => a != dir).ToList();

        int randomDirection = UnityEngine.Random.Range(0, 3);
        Direction direction = allDirections[randomDirection];

        if (direction == Direction.North)
        {
            CreateDirectionTile(_lastX, _lastZ + 1f, Direction.South, tileNumber, dir);
        }
        else if (direction == Direction.East)
        {
            CreateDirectionTile(_lastX + 1f, _lastZ, Direction.West, tileNumber, dir);
        }
        else if (direction == Direction.South)
        {
            CreateDirectionTile(_lastX, _lastZ - 1f, Direction.North, tileNumber, dir);
        }
        else if (direction == Direction.West)
        {
            CreateDirectionTile(_lastX - 1f, _lastZ, Direction.East, tileNumber, dir);
        }
    }

    private void CreateDirectionTile(float x, float z, Direction dir, int tileNumber, Direction forbiddenDir)
    {
        if (Mathf.Abs(x) <= 2f && Mathf.Abs(z) <= 2f)
        {
            CreateNewTile(forbiddenDir, tileNumber);
        }
        else
        {
            if (CheckExistingTiles(tileNumber, x, z, dir))
            {
                GameTile tempTile;
                if (tileNumber == 1)
                {
                    tempTile = _factory.Get(TileType.Spawn);
                }
                else
                {
                    tempTile = _factory.Get(TileType.Walkable);
                }

                var newTile = tempTile as WalkableTile;
                newTile.transform.position = new Vector3(x, 0f, z);
                newTile.transform.rotation = rotation;

                newTile.Directions = new Dictionary<int, Direction>();
                newTile.Directions.Add(tileNumber, dir);

                PathHelper.CheckMapSize(_pathSize, newTile);

                _lastTile = newTile;
                _path.Add(tileNumber, _lastTile);

                _lastX = x;
                _lastZ = z;
            }
        }
    }

    private bool CheckExistingTiles(int tileNumber, float x, float z, Direction dir)
    {
        if (PathHelper.CheckCollision(x, z, 1, out var buffer) > 0)
        {
            if (tileNumber == 1)
            {
                var newTile = buffer[0].GetComponentInParent<WalkableTile>();
                newTile.Directions.Add(tileNumber, dir);


                var spawnerTile = _factory.Get(TileType.Spawn) as WalkableTile;
                spawnerTile.transform.position = new Vector3(x, 0f, z);
                spawnerTile.transform.rotation = rotation;

                spawnerTile.Directions = newTile.Directions;

                _path.Where(a => a.Value == newTile).Select(a => a.Key).ToList().ForEach(a => _path[a] = spawnerTile);
                newTile.Recycle();

                _lastTile = spawnerTile;
                _path.Add(tileNumber, _lastTile);

                _lastX = x;
                _lastZ = z;

                return false;
            }
            else
            {
                var newTile = buffer[0].GetComponentInParent<WalkableTile>();
                newTile.Directions.Add(tileNumber, dir);

                _lastTile = newTile;
                _path.Add(tileNumber, _lastTile);

                _lastX = x;
                _lastZ = z;

                return false;
            }
        }
        return true;
    }
}