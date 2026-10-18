using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillageData
{
    public List<BuildingsData> Buildings { get => _buildings; set => _buildings = value; }
    private List<BuildingsData> _buildings;
    
    public VillageData()
    {
        _buildings = new List<BuildingsData>();
    }
}
