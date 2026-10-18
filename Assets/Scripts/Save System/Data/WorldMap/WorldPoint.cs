using UnityEngine;

[System.Serializable]
public class WorldPoint
{
    public float[] Position { get => _position; set => _position = value; }
    private float[] _position = new float[3];

    public WorldPoint(Vector3 position)
    {
        _position[0] = position.x;
        _position[1] = position.y;
        _position[2] = position.z;
    } 

    public bool CheckWorldPosition(Vector3 position)
    {
        return (position.x == _position[0]) && (position.y == _position[1]) && (position.z == _position[2]);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(_position[0], _position[1], _position[2]);
    }
}
