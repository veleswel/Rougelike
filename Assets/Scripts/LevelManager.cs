using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current;
    
    [SerializeField]
    private float _transitionTime = 2f;

    private bool _isPaused = false;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        PlayerController.current.DisableMovement = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void LoadLevel(string levelToLoad)
    {
        StartCoroutine(LevelLoadRoutine(levelToLoad));
    }

    IEnumerator LevelLoadRoutine(string levelToLoad)
    {
        UIController.current.FadeOut();

        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(levelToLoad);
    }

    public void PauseUnpause()
    {
        if (!_isPaused)
        {
            _isPaused = true;
            Time.timeScale = 0f;
            PlayerController.current.DisableMovement = true;
        }
        else
        {
            _isPaused = false;
            Time.timeScale = 1f;
            PlayerController.current.DisableMovement = false;
        }

        UIController.current.EnablePauseScreen(_isPaused);
    }
}
