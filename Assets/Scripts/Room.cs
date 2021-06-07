using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private bool _isActive = false;

    [SerializeField]
    private List<EnemyController> _enemies = new List<EnemyController>();

    public List<EnemyController> Enemies { get { return _enemies; } }

    void Start()
    {
        GameEvents.current.onEnemyDied += OnEnemyDiedEaction;
    }

    void OnDestroy()
    {
        GameEvents.current.onEnemyDied -= OnEnemyDiedEaction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isActive = true;

            CameraController.current.MoveToTarget(transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isActive = false;
        }
    }

    void OnEnemyDiedEaction(EnemyController enemy)
    {
        if (_enemies.Count != 0 && _enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);

            if (_enemies.Count == 0)
            {
                GameEvents.current.TriggerOnRoomCleared(this);
            }
        }
    }
}
