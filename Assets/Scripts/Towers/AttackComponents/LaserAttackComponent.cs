using System;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LaserAttackComponent : AttackComponentBase
{
    private float _damagePerSecond;
    private float _damageUpgrade = 20f;

    private Vector3 _laserBeamScale;
    private TargetPoint _lastTarget = default;

    private AcquireTargetComponent _targetComponent;
    private TrackTargetComponent _trackerComponent;
    public LaserAttackComponent(Tower tower,
        float damagePerSecond)
    {
        _tower = tower;

        _damagePerSecond = damagePerSecond;
        _laserBeamScale = _tower.transform.GetChild(3).localScale;
    }

    public override void GameAwake()
    {
        _targetComponent = _tower.Components.FirstOrDefault(a => a.GetType() == typeof(AcquireTargetComponent)) as AcquireTargetComponent;
        _trackerComponent = _tower.Components.FirstOrDefault(a => a.GetType() == typeof(TrackTargetComponent)) as TrackTargetComponent;

        if (_targetComponent == null || _trackerComponent == null)
        {
            Debug.Log("Ошибка :(");
        }
    }

    public override void GameUpdate()
    {
        if ((_trackerComponent.TargetTracked || _targetComponent.TargetAcquired) && _isActive)
        {
            Shoot();
        }
        else
        {
            _tower.transform.GetChild(3).localScale = Vector3.zero;
        }
    }

    public override void UpdateCharacteristics()
    {
        _tower.Characteristics.AppendLine($"DPS: {_damagePerSecond}", 2);
        _tower.NextLvlCharacteristics.AppendLine($" + {_damageUpgrade}", 2);
    }

    public override void UpgradeTower()
    {
        _damagePerSecond += _damageUpgrade;
        _damagePerSecond = (float)Math.Round(_damagePerSecond);
    }

    private void Shoot()
    {
        if (_targetComponent.Target == null)
        {
            return;
        }

        Vector3 point = _targetComponent.Target.Position;
        _tower.transform.GetChild(2).LookAt(point);
        _tower.transform.GetChild(3).localRotation = _tower.transform.GetChild(2).localRotation;

        float d = Vector3.Distance(_tower.transform.GetChild(2).position, point);
        _laserBeamScale.z = d;
        _tower.transform.GetChild(3).localScale = _laserBeamScale;
        _tower.transform.GetChild(3).localPosition = _tower.transform.GetChild(2).localPosition + 0.5f * d * _tower.transform.GetChild(3).forward;

        _targetComponent.Target.Enemy.ApplyDamage(_damagePerSecond * Time.deltaTime);
    }

    public override void ChangeDamage(float damage)
    {
        _damagePerSecond += damage;
    }
}
