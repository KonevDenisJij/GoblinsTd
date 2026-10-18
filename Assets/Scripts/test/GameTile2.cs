using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTile2 : MonoBehaviour
{
    public float X => transform.position.x;

    public float Z => transform.position.z;

    public float Cost => _cost;
    private float _cost = 1;

    public List<Enemy> Enemies => _enemies;
    private List<Enemy> _enemies = new List<Enemy>();

    public GameTileContent Content => _content;
    private GameTileContent _content;

    public void OnCreate(int x, int z)
    {
        transform.position = new Vector3(x, 0, z);
    }

    public void ChangeCost(float cost)
    {
        _cost = cost;
    }

    public void CreateContent(GameTileContent content)
    {
        _content = content;
        content.OriginalTile = this;
        ChangeCost(content.PathWeight);
    }

    public void DestroyContent()
    {
        _content = null;
        ChangeCost(1);
    }

    public void SetMaterial(Material material)
    {
        GetComponentInChildren<Renderer>().material = material;
    }

    public void AddEnemies(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemies(Enemy enemy)
    {
        _enemies = _enemies.Where(a => a != enemy).ToList();
    }
}
