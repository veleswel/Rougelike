using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    private float _healAmount = 20;

    [SerializeField]
    private int _pickupSFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AudioManager.current.PlaySoundEffect(_pickupSFX);
            PlayerController.current.Heal(_healAmount);
            Destroy(this.gameObject);
        }
    }
}
