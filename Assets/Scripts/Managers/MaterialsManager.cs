using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsManager : DefaultManager
{
    [SerializeField]
    private EnemyMaterials _enemyMaterials;
    [SerializeField]
    private RoadMaterials _roadMaterials;
    public EnemyMaterials EnemyMaterials => _enemyMaterials;
    public RoadMaterials RoadMaterials => _roadMaterials;

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {
    }
}
