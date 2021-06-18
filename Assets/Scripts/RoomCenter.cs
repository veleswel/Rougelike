using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    [SerializeField]
    private List<EnemyController> _enemies = new List<EnemyController>();

    public List<EnemyController> Enemies { get { return _enemies; } }

    [SerializeField]
    private Room _room;

    void Start()
    {
        GameEvents.Instance.onEnemyDied += OnEnemyDiedEaction;
    }

    public void SetRoom(Room room)
    {
        _room = room;
    }

    void OnDestroy()
    {
        GameEvents.Instance.onEnemyDied -= OnEnemyDiedEaction;
    }

    void OnEnemyDiedEaction(EnemyController enemy)
    {
        if (_enemies.Count != 0 && _enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);

            if (_enemies.Count == 0)
            {
                GameEvents.Instance.TriggerOnRoomCleared(_room);
            }
        }
    }
}
