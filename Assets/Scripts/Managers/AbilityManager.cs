using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : DefaultManager
{
    public static AbilityManager Instance => _instance;
    private static AbilityManager _instance;

    void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public override void GameAwake()
    {
    }

    public override void GameUpdate()
    {

    }

    public void UseAbility(int abilityNumber)
    {
        if (ObjectSelection.SelectedObject is Tower)
        {
            var selectedTower = ObjectSelection.SelectedObject as Tower;
            selectedTower.UseAbility(abilityNumber);
        }
    }

    public void ChangeAbilities()
    {
        if (ObjectSelection.SelectedObject is Tower)
        {
            GUI.Instance.AbilitiesLayout.ChangeIcons((ObjectSelection.SelectedObject as Tower).Abilities.Select(a => a as AbilityBase).ToList());
        }
        else
        {
            GUI.Instance.AbilitiesLayout.ClearIcons();
        }
    }
}
