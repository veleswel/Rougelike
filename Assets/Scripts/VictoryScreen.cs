using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField]
    private string _sceneToLoad;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(_sceneToLoad);
        }    
    }
}
