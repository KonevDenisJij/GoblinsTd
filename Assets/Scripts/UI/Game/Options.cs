using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public static bool OptionsIsOn { get => _optionsIsOn; set => _optionsIsOn = value; }
    private static bool _optionsIsOn;
    public void OpenTab()
    {
        _optionsIsOn = true;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// рнкэйн дкъ яксвюеб оепейкчвемхъ лефдс яжемюлх!!!
    /// </summary>
    public void CloseTab()
    {
        _optionsIsOn = false;
        gameObject.SetActive(false);
    }

    public void ResumeButton()
    {
        _optionsIsOn = false;
        PauseManager.GetInstance().SetPaused(GUI.Instance.IsPaused);
        gameObject.SetActive(false);
    }

    public void OptionsButton()
    {

    }

    public void ControlsButton()
    {

    }

    public void ExitButton()
    {
        SceneManager.LoadScene(0);
    }
}
