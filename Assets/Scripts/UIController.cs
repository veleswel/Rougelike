using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController s_instance;

    [SerializeField]    
    private Slider _healthSlider;
    [SerializeField]    
    private Text _healthText;
    [SerializeField]    
    private GameObject _deathScreen;

    public Slider HealthSlider { get { return _healthSlider; } }
    public Text HealthText { get { return _healthText; } }
    public GameObject DeathScreen { get {return _deathScreen; } }

    void Awake()
    {
        s_instance = this;
    }

    void Start()
    {
        _deathScreen.SetActive(false);
    }

    void Update()
    {
        
    }
}
