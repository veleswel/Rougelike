using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController current;

    [SerializeField]    
    private Slider _healthSlider;
    [SerializeField]    
    private Text _healthText;
    [SerializeField]
    private GameObject _deathScreen, _pauseScreen;

    [SerializeField]
    private Image _fadeScreen;

    [SerializeField]
    private float _fadeInTime, _fadeOutTime = 2f;
    private float _elapsedTime = 0f;

    private bool _isFadingIn = false, _isFadingOut = false;


    [SerializeField]
    private string _newGameScene, _mainMenuScene;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        _deathScreen.SetActive(false);
        _pauseScreen.SetActive(false);

        GameEvents.current.onPlayerHealthChanged += OnPlayerHealthChangedAction;
        GameEvents.current.onPlayerDied += OnPlayerDiedAction;
        
        _isFadingIn = true;
        _isFadingOut = false;

        _elapsedTime = 0f;
    }

    void Update()
    {
        if (_isFadingIn)
        {
            if (_elapsedTime < _fadeInTime)
            {
                _fadeScreen.color = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, 1f - Mathf.Clamp01(_elapsedTime /_fadeInTime));

                _elapsedTime += Time.deltaTime;

                if (_elapsedTime >= _fadeInTime)
                {
                    _isFadingIn = false;
                }
            }
        }

        if (_isFadingOut)
        {
            if (_elapsedTime < _fadeOutTime)
            {
                _fadeScreen.color = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, Mathf.Clamp01(_elapsedTime /_fadeOutTime));

                _elapsedTime += Time.deltaTime;

                if (_elapsedTime >= _fadeOutTime)
                {
                    _isFadingOut = false;
                }
            }
        }
    }

    void OnDestroy()
    {
        GameEvents.current.onPlayerHealthChanged -= OnPlayerHealthChangedAction;
        GameEvents.current.onPlayerDied -= OnPlayerDiedAction;
    }

    public void InitPlayerHealthUI(float current, float max)
    {
        _healthSlider.value = current;
        _healthSlider.maxValue = max;
        _healthText.text = Mathf.RoundToInt(current).ToString();
    }

    void OnPlayerDiedAction()
    {
        _deathScreen.SetActive(true);

        _pauseScreen.SetActive(false);
        _fadeScreen.gameObject.SetActive(false);
    }

    void OnPlayerHealthChangedAction(float health)
    {
        _healthSlider.value = health;
        _healthText.text = Mathf.RoundToInt(health).ToString();
    }

    public void FadeOut()
    {
        _isFadingIn = false;
        _isFadingOut = true;

        _elapsedTime = 0f;
    }

    public void EnablePauseScreen(bool isEnabled)
    {
        _pauseScreen.SetActive(isEnabled);
    }

    public void OnNewGameButtonClicked()
    {
        SceneManager.LoadScene(_newGameScene);
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene(_mainMenuScene);
    }

    public void OnResumeButtonClicked()
    {
        LevelManager.current.PauseUnpause();
    }
}
