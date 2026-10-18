using UnityEngine;

public abstract class PositionCheckComponentBase : TowerComponentBase
{
    public GameTileContent Tile => _tile;
    protected GameTileContent _tile;

    public abstract bool CheckPosition(GameTile2 tile);
}
