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
    private Image _deathScreen, _fadeScreen;

    [SerializeField]
    private float _fadeTime = 2f;

    private bool _isFadingIn = false, _isFadingOut = false;

    private bool _isPlayerDead = false;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        _deathScreen.enabled = false;

        GameEvents.current.onPlayerHealthChanged += OnPlayerHealthChangedAction;
        GameEvents.current.onPlayerDied += OnPlayerDiedAction;

        _isPlayerDead = false;
        
        _isFadingIn = true;
        _isFadingOut = false;
    }

    void Update()
    {
        if (_isPlayerDead == true && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        if (_isFadingIn)
        {
            _fadeScreen.color = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, Mathf.MoveTowards(_fadeScreen.color.a, 0f, Time.deltaTime * _fadeTime));
            if (_fadeScreen.color.a == 0f)
            {
                _isFadingIn = false;
            }
        }

        if (_isFadingOut)
        {
            _fadeScreen.color = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, Mathf.MoveTowards(_fadeScreen.color.a, 1f, Time.deltaTime * _fadeTime));
            if (_fadeScreen.color.a == 1f)
            {
                _isFadingOut = false;
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
        _deathScreen.enabled = false;
        _isPlayerDead = true;
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
    }
}
