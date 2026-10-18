using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUI : MonoBehaviour
{
    public AbilitiesLayout AbilitiesLayout => _abilitiesLayout;
    [SerializeField]
    private AbilitiesLayout _abilitiesLayout;
    public ItemsLayout ItemsLayout => _itemsLayout;
    [SerializeField]
    private ItemsLayout _itemsLayout;
    public Charecteristic Charecteristic => _charecteristic;
    [SerializeField]
    private Charecteristic _charecteristic;

    [SerializeField]
    private Options _options;
    [SerializeField]
    private TextMeshProUGUI _health;
    [SerializeField]
    private TextMeshProUGUI _gold;
    [SerializeField]
    private TextMeshProUGUI _humanResource;

    private static GUI _instance;
    public static GUI Instance => _instance;

    public bool IsPaused => _isPaused;
    private bool _isPaused = false;

    private static Camera _camera => GameObject.FindGameObjectsWithTag("Portrait Camera")[0].GetComponent<Camera>();

    void OnEnable()
    {
        _instance = this;
    }

    private void Update()
    {
        UpdatePortraitCamera(ObjectSelection.SelectedObject);
    }

    public void OnPauseClicked()
    {
        if (Options.OptionsIsOn)
        {
            return;
        }
        _isPaused = !_isPaused;
        PauseManager.GetInstance().SetPaused(_isPaused);
    }

    public void OnDoubleSpeedClicked()
    {

    }

    public void OnOptionsClicked()
    {
        if (Options.OptionsIsOn)
        {
            return;
        }
        PauseManager.GetInstance().SetPaused(true);
        _options.OpenTab();
    }

    public void UpdateHealth()
    {
        _health.text = DefenseController.Instance.PlayerManager.PlayerHealth.ToString();
    }

    public void UpdateGold()
    {
        _gold.text = DefenseController.Instance.PlayerManager.PlayerGold.ToString();
    }

    public void UpdateHumanResource()
    {
        _humanResource.text = DefenseController.Instance.PlayerManager.HumanResource.ToString() + "/" + DefenseController.Instance.PlayerManager.MaxHumanResource.ToString();
    }

    private void UpdatePortraitCamera(ISelectableObject selectedObject)
    {
        if (selectedObject == null)
        {
            return;
        }

        _camera.transform.position = (selectedObject as MonoBehaviour).transform.position + new Vector3(-2f, 4f, -0.6f);
    }
}
