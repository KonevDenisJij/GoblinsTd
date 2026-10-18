using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Materials/RoadMaterials")]
public class RoadMaterials : ScriptableObject
{
    [SerializeField]
    private Material _yellowMaterial;
    public Material YellowMaterial => _yellowMaterial;

    [SerializeField]
    private Material _redMaterial;
    public Material RedMaterial => _redMaterial;
}
