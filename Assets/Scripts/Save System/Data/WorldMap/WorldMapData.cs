using System.Collections.Generic;

[System.Serializable]
public class WorldMapData
{
    public List<WorldConnection> AllConnections { get => _allConnections; set => _allConnections = value; }
    private List<WorldConnection> _allConnections;

    public List<WorldPoint> AllPoints { get => _allPoints; set => _allPoints = value; }
    private List<WorldPoint> _allPoints;

    public WorldPoint CurrentPoint { get => _currentPoint; set => _currentPoint = value; }
    private WorldPoint _currentPoint;

    public WorldMapData()
    {
        _allConnections = new List<WorldConnection>();
        _allPoints = new List<WorldPoint>();
    }
}
