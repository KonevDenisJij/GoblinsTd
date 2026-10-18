using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesLayout : LayoutBase
{
    [SerializeField]
    private Sprite _emptySprite;

    public void UseAbility(int abilityNumber)
    {
        DefenseController.Instance.AbilityManager.UseAbility(abilityNumber);
    }

    public void ChangeIcons(List<AbilityBase> abilities)
    {
        ClearIcons();

        var counter = abilities.Count < 4 ? abilities.Count : transform.childCount - 1;
        for (int i = 0; i < counter; i++)
        {
            transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = abilities[i].AbilityIcon;
            transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// гюлемхрэ мю ялемс яонянамняреи мю цепнияйхе!!!!
    /// </summary>
    public void ClearIcons()
    {
        var counter = transform.childCount - 1;
        for (int i = 0; i < counter; i++)
        {
            transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = _emptySprite;
            transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
