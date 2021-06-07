using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField]    
    private EnemyBullet _bullet;

    [SerializeField]
    private int _fireSFXIdx;

    [SerializeField]
    private float _shootingRange = 10;

    [SerializeField]    
    private float _autoFireRate = 0.5f;

    [SerializeField]    
    private float _damage = 10f;

    private float _nextShot = 0f;

    [SerializeField]    
    private Transform _shootingPoint;

    [SerializeField]
    private SpriteRenderer _body;

    private PlayerController _player;

    void Start()
    {
        _player = PlayerController.current;
        if (_player == null)
        {
            Debug.LogError("PlayerController.current is missing!");
        }
    }

    void Update()
    {
        if (!_body.isVisible || !_player.gameObject.activeInHierarchy)
        {
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) < _shootingRange)
        {
            Fire();
        }
    }

    void Fire()
    {
        if (Time.time > _nextShot)
        {
            _nextShot = Time.time + _autoFireRate;

            Vector3 dir = PlayerController.current.transform.position - transform.position;
            dir.Normalize();

            EnemyBullet bullet = Instantiate(_bullet, _shootingPoint.position, Quaternion.identity);
            bullet.MoveDirection = dir;
            bullet.Damage = _damage;

            AudioManager.current.PlaySoundEffect(_fireSFXIdx);
        }
    }
}
