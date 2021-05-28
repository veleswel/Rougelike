using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamagePlayer : MonoBehaviour
{
    [SerializeField]
    private float _damage = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.s_instance.DamagePlayer(_damage);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.s_instance.DamagePlayer(_damage);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.s_instance.DamagePlayer(_damage);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.s_instance.DamagePlayer(_damage);
        }
    }
}
