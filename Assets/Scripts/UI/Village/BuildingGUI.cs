using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingGUI : MonoBehaviour
{
    [SerializeField]
    private BuildingMenu _buildingMenu;
    [SerializeField]
    private BuildingDescription _buildingDescription;
    [SerializeField]
    private Options _options;

    private static BuildingGUI _instance;
    public static BuildingGUI Instance => _instance;

    void OnEnable()
    {
        _instance = this;
        _options.CloseTab();
    }

    public void StartBuildingProcess(BuildingTile buildingBase)
    {
        if (_buildingMenu.isActiveAndEnabled)
        {
            _buildingMenu.CloseWindow();
        }
        if (_buildingDescription.isActiveAndEnabled)
        {
            _buildingDescription.CloseWindow();
        }

        _buildingMenu.gameObject.SetActive(true);
    }

    public void OpenBuildingMenu(BuildingBase buildingBase)
    {
        if (_buildingMenu.isActiveAndEnabled)
        {
            _buildingMenu.CloseWindow();
        }
        if (_buildingDescription.isActiveAndEnabled)
        {
            _buildingDescription.CloseWindow();
        }

        _buildingDescription.EnableWindow(buildingBase);
    }

    public void OnOptionsClicked()
    {
        if (Options.OptionsIsOn)
        {
            return;
        }
        _options.OpenTab();
    }

    public void ReturnToMap()
    {
        SaveSystem.SaveVillage(BuildingManager.Instance.PrepareSaveData());
        SceneManager.LoadScene(2);
    }
}
