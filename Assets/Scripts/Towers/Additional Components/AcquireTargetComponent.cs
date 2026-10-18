using System;
using System.Linq;
using UnityEngine;

public class AcquireTargetComponent : TowerComponentBase
{
    private float _nextLvlAttackRadius;
    public TargetPoint Target { get => _target; set => _target = value; }
    private TargetPoint _target;

    public bool TargetAcquired => _targetAcquired;
    private bool _targetAcquired;

    public float TargetingRange { get => _targetingRange; set => _targetingRange = value; }
    private float _targetingRange;

    private TrackTargetComponent _targetComponent;

    public AcquireTargetComponent(Tower tower, float targetingRange = 1.5f, float nextLvlAttackRadius = 0.5f)
    {
        _tower = tower;
        _targetingRange = targetingRange;
        _nextLvlAttackRadius = nextLvlAttackRadius;
    }

    public void SetTrackComponent(TrackTargetComponent component)
    {
        _targetComponent = component;
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
        if (!_targetComponent.TargetTracked)
        {
            if (TargetPoint.FillBuffer(_tower.transform.localPosition, _targetingRange))
            {
                _target = TargetPoint.RandomBuffered;
                _targetAcquired = true;
                return;
            }
            _target = null;
            _targetAcquired = false;
        }
    }

    public override void UpdateCharacteristics()
    {
        _tower.Characteristics.AppendLine($"Range: {_targetingRange}", 1);
        _tower.NextLvlCharacteristics.AppendLine($" + {_nextLvlAttackRadius}", 1);
    }

    public override void UpgradeTower()
    {
        _targetingRange += _nextLvlAttackRadius;
        _targetingRange = (float)Math.Round(_targetingRange, 1);
    }
}
