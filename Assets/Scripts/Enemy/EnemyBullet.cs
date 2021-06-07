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
    private int _impactSFXIdx;

    public Vector3 MoveDirection { get; set; }
    public float Damage { get; set; }

    void Update()
    {
        transform.position += MoveDirection * _speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(_hitEffect, transform.position, Quaternion.identity);

        AudioManager.current.PlaySoundEffect(_impactSFXIdx);

        if (other.gameObject.tag == "Player")
        {
            PlayerController.current.Damage(Damage);
        }

        Destroy(this.gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
