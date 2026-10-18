using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    public Image Icon => _icon;

    [SerializeField]
    private Image _background;
    public Image Background => _background;

    [SerializeField]
    private TextMeshProUGUI _text;
    public TextMeshProUGUI Text => _text;

    public int Number { get => _number; set => _number = value; }
    private int _number;
}
