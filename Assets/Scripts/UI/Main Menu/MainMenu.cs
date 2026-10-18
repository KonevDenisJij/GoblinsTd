using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button _loadButton;

    private void OnEnable()
    {
        CheckSave();
    }

    public void PlayGame()
    {
        SaveSystem.StartNewGame();
        SceneManager.LoadScene(2);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void CheckSave()
    {
        if (SaveSystem.CheckSaveFile())
        {
            _loadButton.gameObject.SetActive(true);
        }
        else
        {
            _loadButton.gameObject.SetActive(false);
        }
    }
}
