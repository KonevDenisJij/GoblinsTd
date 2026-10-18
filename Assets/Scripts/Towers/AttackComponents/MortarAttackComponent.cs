using System;
using System.Linq;
using UnityEngine;

public class MortarAttackComponent : AttackComponentBase
{
    public float ShellBlastRadius => _shellBlastRadius;
    private float _shellBlastRadius;
    private float _nextLvlBlastRadius = 0.125f;
    public float ShellDamage => _shellDamage;
    private float _shellDamage;
    private float _nextLvlDamage = 10f;
    public float ShotsPerSecond => _shotsPerSecond;
    private float _shotsPerSecond;
    private float _nextLvlShotsPerSecond = 0.125f;

    private float _launchSpeed;
    private float _launchProgress;

    private AcquireTargetComponent _acquireComponent;
    private TrackTargetComponent _trackerComponent;

    public MortarAttackComponent(Tower tower,
        float shotsPerSecond,
        float shellDamage,
        float shellBlastRadius)
    {
        _tower = tower;
        _trackerComponent = new TrackTargetComponent(_tower);
        _acquireComponent = new AcquireTargetComponent(_tower);
        _trackerComponent.SetAcquireComponent(_acquireComponent);
        _acquireComponent.SetTrackComponent(_trackerComponent);
        _tower.Components.Add(_acquireComponent);
        _tower.Components.Add(_trackerComponent);

        _shotsPerSecond = shotsPerSecond;
        _shellDamage = shellDamage;
        _shellBlastRadius = shellBlastRadius;

        float x = _acquireComponent.TargetingRange + 0.2501f;
        float y = -_tower.transform.GetChild(1).position.y;
        _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
        _launchProgress += _shotsPerSecond * Time.deltaTime;
        while (_launchProgress >= 1f)
        {
            if (_trackerComponent.TargetTracked && _acquireComponent.TargetAcquired)
            {
                Launch(_acquireComponent.Target);
                _launchProgress -= 1f;
            }
            else
            {
                _launchProgress = 0.999f;
            }
        }
    }

    public void Launch(TargetPoint target)
    {
        Vector3 launchPoint = _tower.transform.GetChild(1).position;
        Vector3 targetPoint = target.Position;
        targetPoint.y = 0f;
        Vector2 dir;
        dir.x = targetPoint.x - launchPoint.x;
        dir.y = targetPoint.z - launchPoint.z;
        float x = dir.magnitude;
        float y = -launchPoint.y;
        dir /= x;

        float g = 9.81f;
        float s = _launchSpeed;
        float s2 = s * s;

        float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
        Debug.Assert(r >= 0f, "Launch velocity insufficient for range! s: " + s + " g: " + g + " x: " + x + " y: " + y + " r: " + r);
        float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;

        _tower.transform.GetChild(1).localRotation = Quaternion.LookRotation(new Vector3(dir.x, tanTheta, dir.y));

        (DefenseController.Instance.AmmunitionManager.SpawnWarEntity(AmmunitionType.MortarShell) as MortarShell)
            .Initialize(
            launchPoint,
            targetPoint,
            new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y),
            _shellBlastRadius,
            _shellDamage,
            _tower);
    }

    public override void UpdateCharacteristics()
    {
        _tower.Characteristics.AppendLine($"Damage: {_shellDamage}", 3);
        _tower.Characteristics.AppendLine($"Attack speed: {_shotsPerSecond}", 4);
        _tower.Characteristics.AppendLine($"Blast radius: {_shellBlastRadius}", 5);

        _tower.NextLvlCharacteristics.AppendLine($" + {_nextLvlDamage}", 3);
        _tower.NextLvlCharacteristics.AppendLine($" + {_nextLvlShotsPerSecond}", 4);
        _tower.NextLvlCharacteristics.AppendLine($" + {_nextLvlBlastRadius}", 5);
    }

    public override void UpgradeTower()
    {
        _shellBlastRadius += _nextLvlBlastRadius;
        _shellBlastRadius = (float)Math.Round(_shellBlastRadius, 3);

        _shellDamage += _nextLvlDamage;
        _shellDamage = (float)Math.Round(_shellDamage);

        _shotsPerSecond += _nextLvlShotsPerSecond;
        _shotsPerSecond = (float)Math.Round(_shotsPerSecond, 3);
    }
}