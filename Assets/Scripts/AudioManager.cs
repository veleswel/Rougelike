using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;

    [SerializeField]
    private AudioSource _levelMusic, _gameOverMusic, _winMusic;

    [SerializeField]
    private AudioSource[] _soundEffects;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        GameEvents.current.onPlayerDied += OnPlayerDiedAction;
    }

    void OnDestroy()
    {
        GameEvents.current.onPlayerDied -= OnPlayerDiedAction;
    }

    private void OnPlayerDiedAction()
    {
        _levelMusic.Stop();
        _gameOverMusic.Play();
    }

    private void OnPlayerWinAction()
    {
        _levelMusic.Stop();
        _winMusic.Play();
    }

    public void PlaySoundEffect(int idx)
    {
        if (idx >= _soundEffects.Length || idx < 0)
        {
            Debug.LogError("Can't play a sound effect with index: " + idx.ToString() + ". No such index.");
        }
        _soundEffects[idx].Play();
    }
}
