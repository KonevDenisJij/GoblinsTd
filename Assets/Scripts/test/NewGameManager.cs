using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewGameManager : MonoBehaviour
{
    [SerializeField]
    private NewGameBoard _board;
    [SerializeField]
    private Enemy _enemyPrefab;
    [SerializeField]
    private Tower _towerPrefab;
    [SerializeField]
    private Material _redMaterial;
    public Material RedMaterial => _redMaterial;
    [SerializeField]
    private Material _yellowMaterial;
    public Material YellowMaterial => _yellowMaterial;

    public NewGameBoard Board => _board;

    private List<Enemy> _enemies = new List<Enemy>();

    public static NewGameManager Instance => _instance;
    private static NewGameManager _instance;

    public bool UpdatePathMode => _updatePathMode;
    private bool _updatePathMode;

    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    void OnEnable()
    {
        _instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CreateWall();
        }

        UpdateEnemies();
    }

    private void HandleTouch()
    {
        GameTile2 tile = _board.GetTile(TouchRay);

        if (tile != null)
        {
            var newEnemy = Instantiate(_enemyPrefab);
            _enemies.Add(newEnemy);
        }
    }

    private void CreateWall()
    {
        GameTile2 tile = _board.GetTile(TouchRay);

        if (tile != null)
        {
            var newTower = Instantiate(_towerPrefab);
            tile.CreateContent(newTower);
            newTower.transform.position = tile.transform.position;
        }
    }

    private void UpdateEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.UpdateEnemy();
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        _enemies = _enemies.Where(a => a != enemy).ToList();
        enemy.CurrentTile.RemoveEnemies(enemy);
        Destroy(enemy.gameObject);
    }
}
