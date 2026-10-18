using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class Charecteristic : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI _killsCounter;

    [SerializeField]
    private Button _descriptionButton;
    [SerializeField]
    private Button _characteristicsButton;
    [SerializeField]
    private Transform _descriptionField;
    [SerializeField]
    private Transform _characteristicsField;
    [SerializeField]
    private Transform _nextLvlCharacteristicsField;

    private TextMeshProUGUI _description => _descriptionField.GetComponentInChildren<TextMeshProUGUI>();
    private TextMeshProUGUI _characteristics => _characteristicsField.GetComponentInChildren<TextMeshProUGUI>();
    private TextMeshProUGUI _nextLvlCharacteristics => _nextLvlCharacteristicsField.GetComponentInChildren<TextMeshProUGUI>();

    public void OpenTab()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Заполнение текста для башен
    /// </summary>
    /// <param name="selectedObject">Башня</param>
    /// <param name="name">Имя</param>
    /// <param name="killsCounter">Кол-во убийств</param>
    /// <param name="nextLvlChar">Характеристики на следующем уровне</param>
    public void FillData(ISelectableObject selectedObject, string name, string killsCounter, string nextLvlChar)
    {
        _description.text = selectedObject.Description;
        _characteristics.text = selectedObject.Characteristics.ToString();

        _name.text = name;
        _killsCounter.text = killsCounter;
        _nextLvlCharacteristics.text = nextLvlChar;
    }

    /// <summary>
    /// Заполнение текста для врагов, интерактивных предметов и тд
    /// </summary>
    /// <param name="selectedObject">Объект</param>
    /// <param name="name">Наименование</param>
    public void FillData(ISelectableObject selectedObject, string name)
    {
        _description.text = selectedObject.Description;
        _characteristics.text = selectedObject.Characteristics.ToString();
        _name.text = name;
    }

    public void CloseTab()
    {
        ObjectSelection.ClearOutline();
        gameObject.SetActive(false);
    }

    public void DestroyTower()
    {
        DefenseController.Instance.TowerManager.DestroyTower(ObjectSelection.SelectedObject as Tower);
    }

    public void UpgradeTower()
    {
        var selectedTower = ObjectSelection.SelectedObject as Tower;

        if (selectedTower.TowerUpgradeComponent != null)
        {
            selectedTower.UpgradeTower();
        }
    }

    public void UpgradeTowerDescription(Tower selectedTower)
    {
        _name.text = ObjectSelection.SelectedObject.Name + " Lvl: " + selectedTower.TowerUpgradeComponent.TowerLvl;
        _characteristics.text = selectedTower.Characteristics.ToString();
        _nextLvlCharacteristics.text = selectedTower.NextLvlCharacteristics.ToString();
    }

    public void OpenDescriptionTab()
    {
        _descriptionButton.gameObject.GetComponent<Image>().color = Color.white;
        _characteristicsButton.gameObject.GetComponent<Image>().color = Color.gray;

        _descriptionField.gameObject.SetActive(true);
        _characteristicsField.gameObject.SetActive(false);
        _nextLvlCharacteristicsField.gameObject.SetActive(false);
    }    

    public void OpenCharacteristicsTab()
    {
        _descriptionButton.gameObject.GetComponent<Image>().color = Color.gray;
        _characteristicsButton.gameObject.GetComponent<Image>().color = Color.white;

        _descriptionField.gameObject.SetActive(false);
        _characteristicsField.gameObject.SetActive(true);
        _nextLvlCharacteristicsField.gameObject.SetActive(true);
    }

    public void UpdateFields(int numberOfKills)
    {
        _killsCounter.text = "Number of kills: " + numberOfKills;
    }
}
