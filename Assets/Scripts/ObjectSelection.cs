using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectSelection
{
    private static JumpFloodOutlineRenderer _lineRenderer;
    public static ISelectableObject SelectedObject => _selectedObj;
    private static ISelectableObject _selectedObj;

    public static void SelectObject(ISelectableObject obj)
    {
        if (!(obj is Enemy) && !(obj is Tower) && !(obj is InteractableObject))
        {
            return;
        }

        if (_selectedObj != obj)
        {
            ClearOutline();
        }

        _selectedObj = obj;

        if ((obj as MonoBehaviour).GetComponent<JumpFloodOutlineRenderer>() != null)
        {
            return;
        }

        if (obj is Tower)
        {
            var selectedTower = obj as Tower;

            selectedTower.gameObject
                .AddComponent<JumpFloodOutlineRenderer>().renderers
                .AddRange(selectedTower.GetComponentsInChildren<Renderer>());

            selectedTower._killCounterContainer.Changed += UpdateCharacteristicsFields;

            var killsCounter = "Number of kills: " + selectedTower.KillCounter;
            var towerName = obj.Name + " Lvl: " + selectedTower.TowerUpgradeComponent?.TowerLvl ?? "-";
            var nextLvlCharacteristics = selectedTower.NextLvlCharacteristics.ToString();

            _lineRenderer = selectedTower.GetComponent<JumpFloodOutlineRenderer>();

            GUI.Instance.Charecteristic.FillData(selectedTower, towerName, killsCounter, nextLvlCharacteristics);
        }
        else if (obj is Enemy)
        {
            var selectedEnemy = obj as Enemy;

            selectedEnemy.gameObject
                .AddComponent<JumpFloodOutlineRenderer>().renderers
                .AddRange(selectedEnemy.GetComponentsInChildren<Renderer>());

            var enemyName = obj.Name;

            _lineRenderer = (_selectedObj as Enemy).GetComponent<JumpFloodOutlineRenderer>();

            GUI.Instance.Charecteristic.FillData(selectedEnemy, enemyName);
        }
        else if (obj is InteractableObject)
        {
            var selectedObject = obj as InteractableObject;

            selectedObject.gameObject
                .AddComponent<JumpFloodOutlineRenderer>().renderers
                .AddRange(selectedObject.GetComponentsInChildren<Renderer>());

            var objectName = obj.Name;

            _lineRenderer = (_selectedObj as GameTileContent).GetComponent<JumpFloodOutlineRenderer>();

            GUI.Instance.Charecteristic.FillData(selectedObject, objectName);
        }

        DefenseController.Instance.AbilityManager.ChangeAbilities();
    }

    public static void ClearOutline()
    {
        MonoBehaviour.Destroy(_lineRenderer);

        GUI.Instance.AbilitiesLayout.ClearIcons();

        if (_selectedObj is Tower)
        {
            (_selectedObj as Tower)._killCounterContainer.Changed -= UpdateCharacteristicsFields;
        }

        _selectedObj = null;
        _lineRenderer = null;
    }

    private static void UpdateCharacteristicsFields(object target, Observable<int>.ChangedEventArgs args)
    {
        GUI.Instance.Charecteristic.UpdateFields((_selectedObj as Tower).KillCounter);
    }
}
