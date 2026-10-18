using TriangleNet.Geometry;
using UnityEngine;

public class EventPoint : MonoBehaviour
{
    [SerializeField]
    private Material _standartPoint; //состояние 0
    [SerializeField]
    private Material _selectedPoint; //состояние 1
    [SerializeField]
    private Material _currentPoint;  //состояние 2

    /// <summary>
    /// 0 - не выбранная точка, 1 - выбранная точка, 2 - действующая точка
    /// </summary>
    private int _state;

    public Vertex Vertex { get => _vertex; set => _vertex = value; }
    private Vertex _vertex;

    public WorldPoint Point { get => _point; set => _point = value; }
    private WorldPoint _point;

    public Vector3 Position => new Vector3((float)_vertex.x, 0f, (float)_vertex.y);

    public void SelectPoint()
    {
        if (_state != 2)
        {
            _state = 1;
            GetComponent<Renderer>().material = _selectedPoint;
        }
    }

    public void UnselectPoint()
    {
        if (_state != 2)
        {
            _state = 0;
            GetComponent<Renderer>().material = _standartPoint;
        }
    }

    public void MakePointCurrent()
    {
        _state = 2;
        GetComponent<Renderer>().material = _currentPoint;
    }

    public void MakePointNotCurrent()
    {
        _state = 0;
        GetComponent<Renderer>().material = _standartPoint;
    }
}
