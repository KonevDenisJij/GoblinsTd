using System;
using System.Linq;
using UnityEngine;

public class BombDropAttackComponent : TowerComponentBase
{
    private float _shellBlastRadius;
    private float _nextLvlBlastRadius = 0.125f;
    private float _shellDamage;
    private float _nextLvlDamage = 10f;
    private float _shotsPerSecond;
    private float _nextLvlShotsPerSecond = 0.125f;

    private float _launchProgress;

    private AcquireTargetComponent _targetComponent;
    private TrackTargetComponent _trackerComponent;
    public BombDropAttackComponent(Tower tower,
        float shotsPerSecond,
        float shellDamage,
        float shellBlastRadius)
    {
        _tower = tower;

        _shotsPerSecond = shotsPerSecond;
        _shellDamage = shellDamage;
        _shellBlastRadius = shellBlastRadius;
    }

    public override void GameAwake()
    {
        _targetComponent = _tower.Components.FirstOrDefault(a => a.GetType() == typeof(AcquireTargetComponent)) as AcquireTargetComponent;
        _trackerComponent = _tower.Components.FirstOrDefault(a => a.GetType() == typeof(TrackTargetComponent)) as TrackTargetComponent;

        if (_targetComponent == null || _trackerComponent == null)
        {
            Debug.Log("Îøèáêà :(");
        }
    }

    public override void GameUpdate()
    {
        _launchProgress += _shotsPerSecond * Time.deltaTime;
        while (_launchProgress >= 1f)
        {
            if (_trackerComponent.TargetTracked || _targetComponent.TargetAcquired)
            {
                Launch(_targetComponent.Target);
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
        (DefenseController.Instance.AmmunitionManager.SpawnWarEntity(AmmunitionType.MortarShell) as MortarShell)
            .Initialize(
            _tower.transform.position,
            new Vector3(_tower.transform.position.x, 0f, _tower.transform.position.z),
            new Vector3(0f, 1f, 0f),
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
