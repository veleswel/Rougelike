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
    private float _autoFireRate = 0.5f;
    private float _nextShot = 0f;

    [SerializeField]    
    private Transform _shootingPoint;

    public void Fire()
    {
        if (Time.time > _nextShot)
        {
            _nextShot = Time.time + _autoFireRate;

            Vector3 dir = PlayerController.current.transform.position - transform.position;
            dir.Normalize();

            EnemyBullet bullet = Instantiate(_bullet, _shootingPoint.position, Quaternion.identity);
            bullet.MoveDirection = dir;

            AudioManager.current.PlaySoundEffect(_fireSFXIdx);
        }
    }
}
