using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController current;

    [SerializeField]    
    private Slider _healthSlider;
    [SerializeField]    
    private Text _healthText;
    [SerializeField]    
    private GameObject _deathScreen;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        _deathScreen.SetActive(false);

        GameEvents.current.onPlayerHealthChanged += OnPlayerHealthChangedAction;
        GameEvents.current.onPlayerDied += OnPlayerDiedAction;
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
    }

    void OnPlayerHealthChangedAction(float health)
    {
        _healthSlider.value = health;
        _healthText.text = Mathf.RoundToInt(health).ToString();
    }
}
