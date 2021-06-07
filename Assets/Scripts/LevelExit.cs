using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField]
    private string _levelToLoad;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.current.DisableMovement = true;
            LevelManager.current.LoadLevel(_levelToLoad);
        }
    }
}
