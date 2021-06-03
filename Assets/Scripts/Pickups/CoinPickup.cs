using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField]
    private int _pickupSFX;
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AudioManager.current.PlaySoundEffect(_pickupSFX);
            Destroy(this.gameObject);
        }
    }
}
