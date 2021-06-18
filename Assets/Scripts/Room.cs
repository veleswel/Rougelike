using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool IsActive { get; private set; } = false;

    private Room[] _adjacentRooms = new Room[] { null, null, null, null };

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsActive = true;

            CameraController.Instance.MoveToTarget(transform);

            GameEvents.Instance.TriggerOnRoomBecameActive(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsActive = false;
        }
    }

    public Room GetAdjacentRoom(Direction dir)
    {
        return _adjacentRooms[(int)dir];
    }
    
    public void SetAdjacentRoom(Direction dir, Room room)
    {
        _adjacentRooms[(int)dir] = room;
    }
}
