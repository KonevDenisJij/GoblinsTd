using System;
using System.Linq;
using UnityEngine;

public class TrackTargetComponent : TowerComponentBase
{
    private AcquireTargetComponent _targetComponent;

    public bool TargetTracked => _targetTracked;
    private bool _targetTracked;

    public TrackTargetComponent(Tower tower)
    {
        _tower = tower;
    }

    public void SetAcquireComponent(AcquireTargetComponent component)
    {
        _targetComponent = component;
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
        if (_targetComponent.Target == null)
        {
            _targetTracked = false;
            return;
        }

        Vector3 a = _tower.transform.localPosition;
        Vector3 b = _targetComponent.Target.Position;
        float x = a.x - b.x;
        float z = a.z - b.z;
        float r = _targetComponent.TargetingRange + 0.125f;

        if (x * x + z * z > r * r)
        {
            _targetComponent.Target = null;
            _targetTracked = false;
            return;
        }

        _targetTracked = true;
        return;
    }

    public override void UpgradeTower()
    {
    }

    public override void UpdateCharacteristics()
    {
    }
}
