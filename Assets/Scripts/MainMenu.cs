using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string _levelToLoad;

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(_levelToLoad);
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
