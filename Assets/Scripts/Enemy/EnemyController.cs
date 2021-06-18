using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    private Rigidbody2D _rigidBody;
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _body;
    private PlayerController _player;

    [SerializeField]
    private float _chaseRadius = 10;

    private Vector3 _moveDirection;

    [SerializeField]
    private float _health = 100;

    [SerializeField]
    private GameObject[] _deathSplatters;

    [SerializeField]
    private ParticleSystem _hitEffect;

    [SerializeField]
    private int _damageSFXIdx, _dieSFXIdx;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        if (_rigidBody == null)
        {
            Debug.LogError("EnemyController's Rigidbody2D is missing!");
        }

        _player = PlayerController.current;
        if (_player == null)
        {
            Debug.LogError("PlayerController.current is missing!");
        }

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("EnemyController's Animator is missing!");
        }
    }

    void Update()
    {
        if (!_body.isVisible || !_player.gameObject.activeInHierarchy)
        {
            _rigidBody.velocity = Vector2.zero;
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) < _chaseRadius)
        {
            _moveDirection = _player.transform.position - transform.position;
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _moveDirection = Vector3.zero;
            _animator.SetBool("isMoving", false);
        }

        _moveDirection.Normalize();
        
        if (_moveDirection.x > 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
        }

        _rigidBody.velocity = _moveDirection * _moveSpeed;
    }

    public void Damage(float damage)
    {
        Instantiate(_hitEffect, transform.position, Quaternion.identity);

        _health -= damage;

        GameEvents.Instance.TriggerOnEnemyDamaged(this);

        AudioManager.current.PlaySoundEffect(_damageSFXIdx);

        if (_health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        AudioManager.current.PlaySoundEffect(_dieSFXIdx);

        GameEvents.Instance.TriggerOnEnemyDied(this);

        int splatter = Random.Range(0, _deathSplatters.Length);
        int rotation = Random.Range(0, 4);

        Instantiate(_deathSplatters[splatter], transform.position, Quaternion.Euler(0f, 0f, 90f * rotation));

        Destroy(this.gameObject);
    }
}
