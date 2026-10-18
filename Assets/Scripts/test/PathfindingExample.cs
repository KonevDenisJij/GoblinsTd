using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class PathfindingExample
{
    public static Stack<GameTile2> FindPath(GameTile2 start, GameTile2 finish)
    {
        var startPathTile = new PathFindingTile(start);
        var finishPathTile = new PathFindingTile(finish);

        startPathTile.SetDistance(finish);

        var activeTiles = new List<PathFindingTile>();
        activeTiles.Add(startPathTile);
        var visitedTiles = new List<PathFindingTile>();
        var retPath = new Stack<GameTile2>();

        while (activeTiles.Any())
        {
            var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

            if (checkTile.X == finish.X && checkTile.Z == finish.Z)
            {
                var tile = checkTile;
                //DefenseController.Instance.Board.SetDefaultMaterial();
                visitedTiles.ForEach(a => a.Tile.SetMaterial(DefenseController.Instance.MaterialsManager.RoadMaterials.YellowMaterial));

                while (tile != null)
                {
                    tile.Tile.SetMaterial(DefenseController.Instance.MaterialsManager.RoadMaterials.RedMaterial);

                    retPath.Push(tile.Tile);
                    if (DefenseController.Instance.EnemyManager.UpdatePathMode && tile.Tile.Enemies.Any())
                    {
                        foreach (var enemy in tile.Tile.Enemies)
                        {
                            if (!enemy.PathUpdated)
                            {
                                enemy.UpdatePath(retPath);
                                enemy.PathUpdated = true;
                            }
                        }
                    }
                    tile = tile.Parent;
                }
                return retPath;
            }

            visitedTiles.Add(checkTile);
            activeTiles.Remove(checkTile);

            var walkableTiles = GetWalkableTiles(checkTile, finishPathTile);

            foreach (var walkableTile in walkableTiles)
            {
                if (visitedTiles.Any(x => x.X == walkableTile.X && x.Z == walkableTile.Z))
                    continue;

                if (activeTiles.Any(x => x.X == walkableTile.X && x.Z == walkableTile.Z))
                {
                    var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Z == walkableTile.Z);
                    if (existingTile.CostDistance > checkTile.CostDistance)
                    {
                        activeTiles.Remove(existingTile);
                        activeTiles.Add(walkableTile);
                    }
                }
                else
                {
                    activeTiles.Add(walkableTile);
                }
            }
        }

        Debug.Log("нет пути...");

        return new Stack<GameTile2>();
    }

    private static List<PathFindingTile> GetWalkableTiles(PathFindingTile currentTile, PathFindingTile targetTile)
    {
        var tiles = new List<PathFindingTile>()
        {
            new PathFindingTile(DefenseController.Instance.Board.GetTileByCoordinates((int)currentTile.X,(int)currentTile.Z - 1), currentTile.Cost, currentTile),
            new PathFindingTile(DefenseController.Instance.Board.GetTileByCoordinates((int)currentTile.X,(int)currentTile.Z + 1), currentTile.Cost, currentTile),
            new PathFindingTile(DefenseController.Instance.Board.GetTileByCoordinates((int)currentTile.X - 1,(int)currentTile.Z), currentTile.Cost, currentTile),
            new PathFindingTile(DefenseController.Instance.Board.GetTileByCoordinates((int)currentTile.X + 1,(int)currentTile.Z), currentTile.Cost, currentTile)
        };

        tiles = tiles.Where(a => a.Tile != null).ToList();
        tiles.ForEach(a => a.SetDistance(targetTile.Tile));

        return tiles;
    }

    
    class PathFindingTile
    {
        public PathFindingTile(GameTile2 tile)
        {
            _tile = tile;
            _cost = tile.Cost;
        }
        public PathFindingTile(GameTile2 tile, float cost, PathFindingTile parent)
        {
            _tile = tile;
            _cost = cost + (tile != null ? tile.Cost : 0);
            _parent = parent;
        }

        public GameTile2 Tile => _tile;
        private GameTile2 _tile;
        public float X => _tile.X;
        public float Z => _tile.Z;

        public float Cost => _cost;
        private float _cost;
        private float _distance;
        public float CostDistance => _cost + _distance;
        public PathFindingTile Parent => _parent;
        private PathFindingTile _parent;

        public void SetDistance(GameTile2 tile)
        {
            _distance = Math.Abs(tile.X - _tile.X) + Math.Abs(tile.Z - _tile.Z);
        }
    }
}