[System.Serializable]
public class WorldConnection
{
    public WorldPoint Point0 { get => _point0; set => _point0 = value; }
    private WorldPoint _point0;

    public WorldPoint Point1 { get => _point1; set => _point1 = value; }
    private WorldPoint _point1;

    public WorldConnection(WorldPoint point0, WorldPoint point1)
    {
        _point0 = point0;
        _point1 = point1;
    }
}