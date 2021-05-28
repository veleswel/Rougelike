using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10;

    [SerializeField]
    private ParticleSystem _hitEffect;

    [SerializeField]
    private float _damage = 10;

    private Rigidbody2D _rigidBody;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        if (_rigidBody == null)
        {
            Debug.LogError("PlayerBullet's Rigidbody2D is missing!");
        }
    }
    void Update()
    {
        _rigidBody.velocity = transform.right * _speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(_hitEffect, transform.position, Quaternion.identity);

        if (other.gameObject.tag == "Enemy")
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            if (enemy == null)
            {
                Debug.LogError("Can't get EnemyController from " + other.gameObject.name);
            }
            
            enemy.Damage(_damage);
        }
        
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
