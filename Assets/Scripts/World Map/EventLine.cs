using UnityEngine;

public class EventLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _line;
    public LineRenderer Line => _line;

    public EventPoint Point0 => _point0;
    private EventPoint _point0;

    public EventPoint Point1 => _point1;
    private EventPoint _point1;

    [Space(20f)]
    [SerializeField]
    private Material _selectedLine; //состояние 0
    [SerializeField]
    private Material _currentLine;  //состояние 1

    public void SelectLine()
    {
        if (_line.material != _currentLine)
        {
            _line.material = _selectedLine;
        }
    }

    public void MakeLineCurrent()
    {
        _line.material = _currentLine;
    }
}
