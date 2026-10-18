using UnityEngine;
using UnityEngine.UI;

public class LayoutBase : MonoBehaviour
{
    public void AbilitiesSlider()
    {
        var counter = transform.childCount - 1;
        for (int i = 0; i < counter; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!transform.GetChild(i).gameObject.activeSelf);
        }
        Debug.Log(transform.GetChild(counter).name);
        transform.GetChild(counter).transform.rotation = Quaternion.Euler(
            0f,
            0f,
            transform.GetChild(counter).transform.rotation.z > 0f ? 0f : 180f
            );
    }

    public void HighlightIcon(int towerNumber)
    {
        if (Options.OptionsIsOn)
        {
            return;
        }

        RemoveHighlight();

        transform.GetChild(towerNumber - 1).GetComponent<Image>().color = Color.green;
    }

    public void RemoveHighlight()
    {
        if (Options.OptionsIsOn)
        {
            return;
        }

        var counter = transform.childCount - 1;
        for (int i = 0; i < counter; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
    }
}
