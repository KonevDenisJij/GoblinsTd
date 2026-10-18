using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingListBase : MonoBehaviour
{
    public void CreateBuildingOption(float yPos, string buildingName, Sprite buildingIcon, Transform parent, BuildingButton prefab, int number)
    {
        var prefabPos = prefab.GetComponent<RectTransform>().localPosition.y;

        var newButton = Instantiate(prefab);
        newButton.transform.SetParent(parent);
        newButton.GetComponent<RectTransform>().localPosition = new Vector3(0f, prefabPos + yPos, 0f);
        newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, newButton.GetComponent<RectTransform>().sizeDelta.y);
        newButton.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        newButton.gameObject.SetActive(true);
        newButton.Text.text = buildingName;
        newButton.Icon.sprite = buildingIcon;
        newButton.Number = number;
    }
}
