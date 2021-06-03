using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{   
    [SerializeField]    
    private Transform _firePoint;

    [SerializeField]    
    private float _autoFireRate = 0.25f;

    private float _nextShot = 0f;

    [SerializeField]    
    private PlayerBullet _bullet;

    [SerializeField]
    private int _fireSFXIdx;

    public void Fire()
    {
        if (Time.time > _nextShot)
        {
            _nextShot = Time.time + _autoFireRate;

            Instantiate(_bullet, _firePoint.position, _firePoint.rotation);
            
            AudioManager.current.PlaySoundEffect(_fireSFXIdx);
        }
    }
}
