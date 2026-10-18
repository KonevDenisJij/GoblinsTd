using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapGUI : MonoBehaviour
{
    [SerializeField]
    private Options _options;
    [SerializeField]
    private Button _playButton;

    private static MapGUI _instance;
    public static MapGUI Instance => _instance;

    void OnEnable()
    {
        _instance = this;
        _options.CloseTab();
    }

    public void LoadVillage()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnOptionsClicked()
    {
        if (Options.OptionsIsOn)
        {
            return;
        }
        _options.OpenTab();
    }

    public void FadePlayButton()
    {
        _playButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 0.5f);
        _playButton.interactable = false;
    }

    public void ShowPlayButton()
    {
        _playButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 1f);
        _playButton.interactable = true;
    }
}
