using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : DefaultManager
{
    [SerializeField]
    private EnemyFactory _enemyFactory;

    public List<Enemy> Enemies => _enemies;
    private List<Enemy> _enemies = new List<Enemy>();

    public static EnemyManager Instance => _instance;
    private static EnemyManager _instance;
    public bool UpdatePathMode => _updatePathMode;
    private bool _updatePathMode = false;

    void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
        UpdateEnemies();
        Physics.SyncTransforms();
    }

    private void UpdateEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.UpdateEnemy();
        }
    }

    public void CreateEnemy(GameTile2 start, EnemyType type)
    {
        Enemy enemy = _enemyFactory.Get(type);
        enemy.transform.position = start.transform.position;
        enemy.Init(start, DefenseController.Instance.Board.Finish);
        _enemies.Add(enemy);
    }

    public void DestroyEnemy(Enemy enemy)
    {
        _enemies = _enemies.Where(a => a != enemy).ToList();
        enemy.CurrentTile.RemoveEnemies(enemy);
        enemy.OriginFactory.Reclaim(enemy);
    }

    public void UpdatePath()
    {
        _updatePathMode = true;
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            if (_enemies[i].PathUpdated)
            {
                continue;
            }

            _enemies[i].FindPath(DefenseController.Instance.Board.Finish);
        }

        _updatePathMode = false;
        foreach (var enemy in _enemies)
        {
            enemy.PathUpdated = false;
        }
    }
}
