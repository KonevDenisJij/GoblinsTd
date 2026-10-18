using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    [SerializeField]
    private Tower _parrent;

    private Camera cam;

    private List<WithHumanComponentBase> _components;


    private void Start()
    {
        if (_components != null)
        {
            return;
        }

        cam = FindObjectOfType<BattleFieldCamera>().GetComponentInChildren<Camera>();

        _components = _parrent.Components.Where(a => a is WithHumanComponentBase).Select(a => a as WithHumanComponentBase).ToList();
        Debug.Assert(_components.Count > 0, "Заебись, нет таких компонентов"); //TODO: если нет компонентов прятать кнопочки
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.transform.forward);
    }

    public void AddHuman()
    {
        if (DefenseController.Instance.PlayerManager.HumanResource <= 0)
        {
            return;
        }

        foreach(var comp in _components)
        {
            if (comp.AmountOfHumans == comp.MaxAmountOfHumans)
            {
                continue;
            }

            comp.AddHuman();
        }

        DefenseController.Instance.PlayerManager.ChangeHumanResource(-1);
    }

    public void RemoveHuman()
    {
        if (DefenseController.Instance.PlayerManager.HumanResource >= DefenseController.Instance.PlayerManager.MaxHumanResource)
        {
            return;
        }

        foreach (var comp in _components)
        {
            if (comp.AmountOfHumans == 0)
            {
                continue;
            }

            comp.RemoveHuman();
        }

        DefenseController.Instance.PlayerManager.ChangeHumanResource(+1);
    }
}