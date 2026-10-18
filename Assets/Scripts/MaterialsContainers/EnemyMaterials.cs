using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Materials/EnemyMaterials")]
public class EnemyMaterials : ScriptableObject
{
    [SerializeField]
    private Material _coldMaterial;
    public Material ColdMaterial => _coldMaterial;
}
