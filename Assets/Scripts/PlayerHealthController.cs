using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController s_instance;

    private float _currentHealth;
    [SerializeField]
    private float _maxHealth;

    [SerializeField]
    private float _baseInvincibleDuration = 1f;
    private float _invincibleDuration = 0f;

    void Awake()
    {
        s_instance = this;
    }

    void Start()
    {
        _currentHealth = _maxHealth;

        UIController.s_instance.HealthSlider.maxValue = _maxHealth;
        UIController.s_instance.HealthSlider.value = _currentHealth;
        UIController.s_instance.HealthText.text = Mathf.RoundToInt(_currentHealth).ToString();
    }
    
    void Update()
    {
        if (_invincibleDuration > 0f)
        {
            _invincibleDuration -= Time.deltaTime;

            if (_invincibleDuration <= 0f)
            {
                Color currentColor = PlayerController.s_instance.BodySprite.color;
                PlayerController.s_instance.BodySprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
            }
        }
    }

    public void DamagePlayer(float damage)
    {
        if (_invincibleDuration <= 0f)
        {
            _invincibleDuration = _baseInvincibleDuration;

            Color currentColor = PlayerController.s_instance.BodySprite.color;
            PlayerController.s_instance.BodySprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, .5f); 

            _currentHealth -= damage;

            UIController.s_instance.HealthSlider.value = _currentHealth;
            UIController.s_instance.HealthText.text = Mathf.RoundToInt(_currentHealth).ToString();

            if (_currentHealth <= 0)
            {
                PlayerController.s_instance.gameObject.SetActive(false);
                UIController.s_instance.DeathScreen.SetActive(true);
            }
        }   
    }
}
