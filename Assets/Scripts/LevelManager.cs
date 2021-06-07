using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current;
    
    [SerializeField]
    private float _transitionTime = 2f;

    void Awake()
    {
        current = this;
    }

    public void LoadLevel(string levelToLoad)
    {
        StartCoroutine(LevelLoadRoutine(levelToLoad));
    }

    IEnumerator LevelLoadRoutine(string levelToLoad)
    {
        GameEvents.current.TriggerOnPlayerWon();
        UIController.current.FadeOut();

        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(levelToLoad);
    }
}
