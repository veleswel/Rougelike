using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController current;

    [SerializeField]    
    private float _baseMoveSpeed = 5f;
    private float _activeMoveSpeed;

    [SerializeField]
    private float _dashSpeed = 8f;
    [SerializeField]
    private float _dashDuration = .5f;
    [SerializeField]
    private float _dashCooldownDuration = 1f;
    [SerializeField]
    private float _dashInvincibilityDuration = .5f;

    private float _dashCountdown;
    private float _dashCoolCountdown;

    private Vector2 _moveInput;

    private Rigidbody2D _rigidBody;

    [SerializeField]    
    private Transform _gunHand;

    [SerializeField]
    private  PlayerWeapon _weapon;

    [SerializeField]    
    private SpriteRenderer _bodySprite;

    public SpriteRenderer BodySprite { get { return _bodySprite; } }

    private Camera _mainCamera;

    private Animator _animator;

    [SerializeField]
    private int _damageSFXIdx, _dashSFXIdx, _dieSFXIdx;

    [SerializeField]
    private PlayerHealthController _healthController;

    private float _playerDamageSFXCountdown = 0f;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        if (_rigidBody == null)
        {
            Debug.LogError("PlayerController's Rigidbody2D is missing!");
        }

        _mainCamera = Camera.main;

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("PlayerController's Animator is missing!");
        }

        _activeMoveSpeed = _baseMoveSpeed;
    }

    void Update()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();

        if (_moveInput != Vector2.zero)
        {
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }

        _rigidBody.velocity = _moveInput * _activeMoveSpeed;

        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(transform.localPosition);

        if (mousePosition.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            _gunHand.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            _gunHand.localScale = Vector3.one;
        }

        Vector2 diff = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        _gunHand.rotation = Quaternion.Euler(Vector3.forward * angle);

        if (Input.GetMouseButton(0))
        {
            _weapon.Fire();
        }

        if (Input.GetKeyDown(KeyCode.Space) && _dashCountdown <= 0f && _dashCoolCountdown <= 0f)
        {
            _activeMoveSpeed = _dashSpeed;
            _dashCountdown = _dashDuration;

            _animator.SetTrigger("dash");

            AudioManager.current.PlaySoundEffect(_dashSFXIdx);

            _healthController.SetInvincibleCountdown(_dashInvincibilityDuration);
        }

        if (_dashCountdown > 0f)
        {
            _dashCountdown -= Time.deltaTime;

            if (_dashCountdown <= 0f)
            {
                _activeMoveSpeed = _baseMoveSpeed;
                _dashCoolCountdown = _dashCooldownDuration;
            }
        }

        if (_dashCoolCountdown > 0f)
        {
            _dashCoolCountdown -= Time.deltaTime;
        }
    }

    public void Damage(float damage)
    {
        _healthController.DamagePlayer(damage);

        if (_playerDamageSFXCountdown <= 0f)
        {
            AudioManager.current.PlaySoundEffect(_damageSFXIdx);

            _playerDamageSFXCountdown = 1f;
            StartCoroutine(PlayerDamageSFXDownRountine());
        }
    }

    public void Heal(float heal)
    {
        _healthController.HealPlayer(heal);
    }

    public void Die()
    {
        PlayerController.current.gameObject.SetActive(false);
        AudioManager.current.PlaySoundEffect(_dieSFXIdx);
    }

    public bool IsDashing()
    {
        return _dashCountdown > 0f;
    }

    IEnumerator PlayerDamageSFXDownRountine()
    {
        while (_playerDamageSFXCountdown > 0f)
        {
            _playerDamageSFXCountdown -= Time.deltaTime;
            yield return null;
        }
    }
}
