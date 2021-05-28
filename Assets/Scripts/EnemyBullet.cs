using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10;

    [SerializeField]
    private ParticleSystem _hitEffect;

    [SerializeField]
    private float _damage = 10;

    public Vector3 MoveDirection { get; set; }

    void Update()
    {
        transform.position += MoveDirection * _speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(_hitEffect, transform.position, Quaternion.identity);

        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.s_instance.DamagePlayer(_damage);
        }

        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
