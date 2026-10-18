using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenu : MonoBehaviour
{
    [SerializeField]
    private BuildingListBase _buildingList;
    [SerializeField]
    private BuildingButton _choicePrefab;
    [SerializeField]
    private Transform _buttonParent;
    [SerializeField]
    private Transform _previewParent;

    private BuildingButton _lastButton;
    private BuildingPreview _activePreview;
    private int _currentNumber = -1;

    private void OnEnable()
    {
        _choicePrefab.gameObject.SetActive(false);
        int i = 0;
        int numbOfOptions = BuildingManager.Instance.AvailableBuildings().Count;
        var prefabHeight = _choicePrefab.gameObject.GetComponent<RectTransform>().sizeDelta.y;

        foreach (var option in BuildingManager.Instance.AvailableBuildings())
        {
            i++;
            _buildingList.CreateBuildingOption(prefabHeight * (numbOfOptions - i), option.Name, option.Icon, _buttonParent, _choicePrefab, i);
        }

        _buttonParent.gameObject.GetComponent<RectTransform>().sizeDelta = 
            new Vector2(_buttonParent.gameObject.GetComponent<RectTransform>().sizeDelta.x, prefabHeight * i);
    }

    public void ChooseBuilding(BuildingButton button)
    {
        if (_lastButton != null)
        {
            _lastButton.Background.color = Color.white; 
        }

        button.Background.color = Color.green;
        _lastButton = button;

        if (_activePreview != null)
        {
            Destroy(_activePreview.gameObject);
            _activePreview = null;
        }

        _currentNumber = button.Number;

        _activePreview = Instantiate(GetBuildingPreview(_currentNumber));
        _activePreview.transform.SetParent(_previewParent);
        _activePreview.transform.localPosition = new Vector3(0f, 0f, -200f);
        _activePreview.transform.localScale = new Vector3(400f, 400f, 400f);
    }

    public void Build()
    {
        if (_currentNumber != -1)
        {
            BuildingManager.Instance.Build(GetBuildingType(_currentNumber));
        }
        CloseWindow();
    }

    public void CloseWindow()
    {
        if (_lastButton != null)
        {
            _lastButton.Background.color = Color.white;
        }

        if (_activePreview != null)
        {
            Destroy(_activePreview.gameObject);
            _activePreview = null;
        }

        gameObject.SetActive(false);
    }

    private BuildingPreview GetBuildingPreview(int number)
    {
        return BuildingManager.Instance.AvailableBuildings()[number - 1].Preview;
    }
    
    private BuildingType GetBuildingType(int number)
    {
        return BuildingManager.Instance.AvailableBuildings()[number - 1].BuildingType;
    }
}
