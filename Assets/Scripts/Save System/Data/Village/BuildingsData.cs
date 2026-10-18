using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingsData
{
    public int BuildingNumber { get => _buildingNumber; set => _buildingNumber = value; }
    private int _buildingNumber;

    public int BuildingPosition { get => _buildingPosition; set => _buildingPosition = value; }
    private int _buildingPosition;

    public BuildingsData(BuildingBase building)
    {
        _buildingNumber = (int)building.BuildingType;
        Int32.TryParse(building.BuildingTile.name, out _buildingPosition);
    }
}
