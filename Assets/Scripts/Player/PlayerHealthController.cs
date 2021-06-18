using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    private float _currentHealth;

    public float CurrentHealth { get { return _currentHealth; } }

    [SerializeField]
    private float _maxHealth;

    private float _invincibleCountdown = 0f;

    void Start()
    {
        _currentHealth = _maxHealth;
        UIController.current.InitPlayerHealthUI(_currentHealth, _maxHealth);
    }
    
    void Update()
    {
        if (_invincibleCountdown > 0f)
        {
            _invincibleCountdown -= Time.deltaTime;

            if (_invincibleCountdown <= 0f)
            {
                Color currentColor = PlayerController.current.BodySprite.color;
                PlayerController.current.BodySprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
            }
        }
    }

    public bool DamagePlayer(float damage)
    {
        if (_invincibleCountdown <= 0f)
        {
            _currentHealth -= damage;
            
            GameEvents.Instance.TriggerOnPlayerDamaged();
            GameEvents.Instance.TriggerOnPlayerHealthChanged(_currentHealth);

            return true;
        }

        return false;   
    }

    public void HealPlayer(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        GameEvents.Instance.TriggerOnPlayerHealthChanged(_currentHealth);
    }

    public void SetInvincibleCountdown(float countdown)
    {
        _invincibleCountdown = countdown;

        Color currentColor = PlayerController.current.BodySprite.color;
        PlayerController.current.BodySprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, .5f);
    }
}
