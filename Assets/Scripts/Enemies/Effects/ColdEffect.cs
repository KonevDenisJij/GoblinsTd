using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdEffect : EffectBase
{
    public override void EffectEnd(Enemy enemy)
    {
        enemy.SetSpeed(1f);
        foreach (var mr in enemy.GetComponentsInChildren<MeshRenderer>())
        {
            var newColors = new Material[1];
            newColors[0] = mr.material;
            mr.materials = newColors;
        }
    }

    public override void EffectStart(Enemy enemy, float duration)
    {
        Duration = duration;
        enemy.ChangeSpeed(-0.5f);
        foreach (var mr in enemy.GetComponentsInChildren<MeshRenderer>())
        {
            var newColors = new Material[2];
            newColors[0] = mr.material;
            newColors[1] = DefenseController.Instance.MaterialsManager.EnemyMaterials.ColdMaterial;
            mr.materials = newColors;
        }
    }

    public override void GameUpdate(Enemy enemy)
    {
        _duration -= Time.deltaTime;
    }
}
