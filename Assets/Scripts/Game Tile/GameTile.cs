using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : GameTileContent
{
    [SerializeField]
    private TileType _tileType;
    public TileType TileType { get => _tileType; set => _tileType = value; }
    public Tower TowerContent { get => _towerContent; set => _towerContent = value; }
    private Tower _towerContent;

    public bool Occupied { get => _occupied; set => _occupied = value; }
    private bool _occupied;
}
