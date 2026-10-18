using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameHolder;

    [SerializeField]
    private TextMeshProUGUI _description;

    [SerializeField]
    private TextMeshProUGUI _effects;

    private BuildingBase _buildingBase;

    public void EnableWindow(BuildingBase buildingBase)
    {
        _buildingBase = buildingBase;
        _nameHolder.text = _buildingBase.Name + " lvl: " + _buildingBase.BuildingLevel;
        _description.text = "HUI HUI HUI";
        _effects.text = "PIDOR PIDOR";
        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        _nameHolder.text = "";
        _description.text = "";
        _effects.text = "";

        gameObject.SetActive(false);
    }

    public void UpgradeBuilding()
    {
        _buildingBase.BuildingLevel++;
        _nameHolder.text = _buildingBase.Name + " lvl: " + _buildingBase.BuildingLevel;
    }

    public void SellBuilding()
    {
        Destroy(_buildingBase.gameObject);
        CloseWindow();
    }
}
