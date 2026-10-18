using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public List<WaveEnemyData> Enemies => _enemies;
    private List<WaveEnemyData> _enemies;

    private WaveEnemyData _nextEnemy;

    private float _currentTime = 0f;

    private int _enemyNumber = 0;

    private bool _waveFinished = false;

    public Wave()
    {
        _enemies = new List<WaveEnemyData>();

        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Small, Delay = 0.1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Big, Delay = 2f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Big, Delay = 0f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Small, Delay = 10f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
        _enemies.Add(new WaveEnemyData() { Enemy = EnemyType.Normal, Delay = 1f });
    }

    public void StartWave()
    {
        _nextEnemy = _enemies[_enemyNumber];
    }

    public void Update()
    {
        if (_waveFinished)
        {
            return;
        }

        _currentTime += Time.deltaTime;

        if (_currentTime > _nextEnemy.Delay)
        {
            DefenseController.Instance.EnemyManager.CreateEnemy(DefenseController.Instance.Board.SpawnPoints[0], _nextEnemy.Enemy);

            if (_enemyNumber == _enemies.Count - 1)
            {
                _waveFinished = true;
                return;
            }

            _enemyNumber++;
            _nextEnemy = _enemies[_enemyNumber];
            _currentTime = 0;
        }
    }
}