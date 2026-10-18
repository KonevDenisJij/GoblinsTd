using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorDamageComponent : TowerComponentBase
{
    private float _damage;
    private float _rechargeTimer;

    private Transform _spikes;

    private float _currentTime = 0f;

    public FloorDamageComponent(Tower tower, float damage, float rechargeTimer)
    {
        _tower = tower;
        _damage = damage;
        _rechargeTimer = rechargeTimer;
    }

    public override void GameAwake()
    {
        _currentTime = _rechargeTimer;
        _spikes = _tower.transform.GetChild(1);
    }

    public override void GameUpdate()
    {
        if (_currentTime < _rechargeTimer)
        {
            _currentTime += Time.deltaTime;
            return;
        }

        _spikes.gameObject.SetActive(true);

        var enemyColliders = Physics
            .OverlapBox(_tower.transform.position, _tower.transform.localScale / 2, Quaternion.identity)
            .Where(a => a.GetComponentInParent<Enemy>() != null)
            .Select(a => a.GetComponentInParent<Enemy>())
            .ToList();
        
        if (enemyColliders.Any())
        {
            enemyColliders.ForEach(a => a.ApplyDamage(_damage));
            _currentTime = 0f;
            _spikes.gameObject.SetActive(false);
        }
    }

    public override void UpdateCharacteristics()
    {
    }

    public override void UpgradeTower()
    {
    }
}
