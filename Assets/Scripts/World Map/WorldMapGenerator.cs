using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TriangleNet.Geometry;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMapGenerator
{
    private static WorldMapGenerator _instance;
    public static WorldMapGenerator Instance => _instance;

    public WorldMapGenerator(int numberOfPoints, 
        EventPoint pointPrefab, 
        Transform pointsParent, 
        EventLine lineRenderer, 
        Transform linesParent, 
        float duplicatesRadius)
    {
        _numberOfPoints = numberOfPoints;
        _pointPrefab = pointPrefab;
        _pointsParent = pointsParent;
        _lineRenderer = lineRenderer;
        _linesParent = linesParent;
        _duplicatesRadius = duplicatesRadius;

        _instance = this;
    }

    private int _numberOfPoints;
    private EventPoint _pointPrefab;
    private Transform _pointsParent;
    private EventLine _lineRenderer;
    private Transform _linesParent;
    private float _duplicatesRadius;

    private float _xSize = 125f;
    private float _negXSize = -125f;

    private float _zSize = 150f;
    private float _negZSize = -150f;

    private bool _regenerateMap = false;

    private EventPoint _currentPoint;

    private TriangleNet.Mesh _mesh;

    private List<EventPoint> _allPoints = new List<EventPoint>();
    private List<EventPoint> _selectedPoints = new List<EventPoint>();
    private List<EventPoint> _currentPoints = new List<EventPoint>();

    private List<EventLine> _selectedLines = new List<EventLine>();
    private List<EventLine> _currentLines = new List<EventLine>();

    private WorldMapData _worldMapData;
    private Ray TouchRay
    {
        get
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return default;
            };

            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

    public void GenerateMap()
    {
        int globalSafer = 0;

        while (true)
        {
            _regenerateMap = false;
            globalSafer++;

            Polygon polygon = new Polygon();
            _allPoints = new List<EventPoint>();

            InstantiatePoint(0f, 0f, polygon);
            InstantiatePoint(_xSize, 0f, polygon);
            InstantiatePoint(_negXSize, 0f, polygon);
            InstantiatePoint(0f, _zSize, polygon);
            InstantiatePoint(0f, _negZSize, polygon);

            for (int i = 5; i < _numberOfPoints; i++)
            {
                var randPos = new Vertex(Random.Range(_negXSize, _xSize), Random.Range(_negZSize, _zSize));
                int safer = 0;
                while (safer < 1000)
                {
                    if (!polygon.Points.Any(a => Mathf.Sqrt(Mathf.Pow((float)a.x - (float)randPos.x, 2) + Mathf.Pow((float)a.y - (float)randPos.y, 2)) < _duplicatesRadius))
                    {
                        break;
                    }
                    randPos = new Vertex(Random.Range(_negXSize, _xSize), Random.Range(_negZSize, _zSize));

                    safer++;
                }

                Debug.Assert(safer < 100, ":(");

                InstantiatePoint((float)randPos.x, (float)randPos.y, randPos, polygon);
            }

            var options = new TriangleNet.Meshing.ConstraintOptions() { ConformingDelaunay = true, SegmentSplitting = 1 };

            TriangleNet.Mesh mesh = (TriangleNet.Mesh)polygon.Triangulate(options);

            _mesh = mesh;

            foreach (Edge edge in _mesh.Edges)
            {
                Vertex v0 = _mesh.vertices[edge.P0];
                Vertex v1 = _mesh.vertices[edge.P1];
                if (Mathf.Sqrt(Mathf.Pow((float)v0.x - (float)v1.x, 2) + Mathf.Pow((float)v0.y - (float)v1.y, 2)) > 110f)
                {
                    _regenerateMap = true;
                }
            }

            if (!_regenerateMap || globalSafer > 1000)
            {
                if (globalSafer > 1000)
                {
                    Debug.Log("œ–Œ¡À≈ÃÕ¿ﬂ  ¿–“¿!!!");
                }

                _worldMapData = new WorldMapData();
                _worldMapData.CurrentPoint = new WorldPoint(_allPoints[0].transform.position);
                _worldMapData.AllPoints.Add(_worldMapData.CurrentPoint);

                for (int i = 1; i < _allPoints.Count; i++)
                {
                    _worldMapData.AllPoints.Add(new WorldPoint(_allPoints[i].transform.position));
                }

                foreach (Edge edge in _mesh.Edges)
                {
                    Vertex v0 = _mesh.vertices[edge.P0];
                    Vertex v1 = _mesh.vertices[edge.P1];
                    Vector3 p0 = new Vector3((float)v0.x, 0f, (float)v0.y);
                    Vector3 p1 = new Vector3((float)v1.x, 0f, (float)v1.y);

                    _worldMapData.AllConnections.Add(new WorldConnection(
                        _worldMapData.AllPoints.FirstOrDefault(a => a.CheckWorldPosition(p0)),
                        _worldMapData.AllPoints.FirstOrDefault(a => a.CheckWorldPosition(p1))));
                }

                break;
            }

            for (int i = 0; i < _pointsParent.childCount; i++)
            {

                MonoBehaviour.Destroy(_pointsParent.GetChild(i).gameObject);
            }
        }

        _currentPoint = _allPoints[0];

        SelectPoint(ref _currentPoint, ref _currentPoints, ref _currentLines);
        _currentPoints.Add(_currentPoint);
        _currentPoints.ForEach(a => a.MakePointCurrent());
        _currentLines.ForEach(a => a.MakeLineCurrent());
        MapGUI.Instance.FadePlayButton();
    }

    public void LoadMap(WorldMapData data)
    {
        _worldMapData = data;
        for (int i = 0; i < _worldMapData.AllPoints.Count; i++)
        {
            if (_worldMapData.AllPoints[i] == _worldMapData.CurrentPoint)
            {
                InstantiateCurrentPoint(_worldMapData.AllPoints[i].ToVector3());
            }
            InstantiatePoint(_worldMapData.AllPoints[i].ToVector3());
        }
    }

    private void InstantiatePoint(float x, float z, Vertex vertex, Polygon polygon)
    {
        polygon.Add(vertex);
        var newPoint = MonoBehaviour.Instantiate(_pointPrefab, new Vector3(x, 0f, z), _pointPrefab.transform.rotation);
        newPoint.Vertex = vertex;
        newPoint.transform.SetParent(_pointsParent);
        _allPoints.Add(newPoint);
    }

    private void InstantiatePoint(float x, float z, Polygon polygon)
    {
        var vertex = new Vertex(x, z);
        polygon.Add(vertex);
        var newPoint = MonoBehaviour.Instantiate(_pointPrefab, new Vector3(x, 0f, z), _pointPrefab.transform.rotation);
        newPoint.Vertex = vertex;
        newPoint.transform.SetParent(_pointsParent);
        _allPoints.Add(newPoint);
    }

    private void InstantiateCurrentPoint(Vector3 position)
    {
        var newPoint = MonoBehaviour.Instantiate(_pointPrefab, new Vector3(position.x, position.y, position.z), _pointPrefab.transform.rotation);
        newPoint.transform.SetParent(_pointsParent);
        _allPoints.Add(newPoint);
        SelectPoint(ref newPoint, ref _currentPoints, ref _currentLines);
    }
    
    private void InstantiatePoint(Vector3 position)
    {
        var newPoint = MonoBehaviour.Instantiate(_pointPrefab, new Vector3(position.x, position.y, position.z), _pointPrefab.transform.rotation);
        newPoint.transform.SetParent(_pointsParent);
        _allPoints.Add(newPoint);
    }

    public void HandleTouch()
    {
        var point = GetPoint(TouchRay);

        if (point != null)
        {
            SelectPoint(ref point, ref _selectedPoints, ref _selectedLines);

            if (_currentPoints.Contains(point) && _currentPoint != point)
            {
                MapGUI.Instance.ShowPlayButton();
            }
            else
            {
                MapGUI.Instance.FadePlayButton();
            }
        }
    }
    
    private void SelectPoint(ref EventPoint point, ref List<EventPoint> points, ref List<EventLine> lines)
    {
        Vertex v0 = point.Vertex;
        Vector3 p0 = new Vector3((float)v0.x, 0f, (float)v0.y);

        for (int i = 0; i < lines.Count; i++)
        {
            MonoBehaviour.Destroy(lines[i].gameObject);
        }
        lines = new List<EventLine>();

        points.ForEach(a => a.UnselectPoint());
        points = new List<EventPoint>();

        points.Add(_allPoints.First(a => a.Vertex == v0));

        foreach (var connection in _worldMapData.AllConnections)
        {
            if (connection.Point0.CheckWorldPosition(p0))
            {
                points.Add(_allPoints.First(a => a.transform.position == connection.Point1.ToVector3()));
                var newLine = MonoBehaviour.Instantiate(_lineRenderer);
                newLine.GetComponentInChildren<LineRenderer>().SetPosition(0, p0);
                newLine.GetComponentInChildren<LineRenderer>().SetPosition(1, connection.Point1.ToVector3());

                newLine.transform.SetParent(_linesParent);
                lines.Add(newLine);
            }
            else if (connection.Point1.CheckWorldPosition(p0))
            {
                points.Add(_allPoints.First(a => a.transform.position == connection.Point0.ToVector3()));
                var newLine = MonoBehaviour.Instantiate(_lineRenderer);
                newLine.GetComponentInChildren<LineRenderer>().SetPosition(0, p0);
                newLine.GetComponentInChildren<LineRenderer>().SetPosition(1, connection.Point0.ToVector3());

                newLine.transform.SetParent(_linesParent);
                lines.Add(newLine);
            }
        }

        points.ForEach(a => a.SelectPoint());
        lines.ForEach(a => a.SelectLine());

        SaveSystem.SaveWorldMap(_worldMapData);
    }

    private EventPoint GetPoint(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.gameObject.GetComponentInParent<EventPoint>();
        }

        return null;
    }

    public void OnDrawGizmos()
    {
        if (_mesh == null)
        {
            return;
        }

        Gizmos.color = UnityEngine.Color.red;
        foreach (Edge edge in _mesh.Edges)
        {
            Vertex v0 = _mesh.vertices[edge.P0];
            Vertex v1 = _mesh.vertices[edge.P1];
            Vector3 p0 = new Vector3((float)v0.x, 0f, (float)v0.y);
            Vector3 p1 = new Vector3((float)v1.x, 0f, (float)v1.y);
            Gizmos.DrawLine(p0, p1);
        }
    }
}
