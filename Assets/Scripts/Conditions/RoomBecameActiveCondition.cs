using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBecameActiveCondition : BaseCondition
{
    [SerializeField]
    private Room _room;

    void Start()
    {
        GameEvents.Instance.onRoomBecameActive += OnRoomBecameActiveAction;
    }

    void OnDestroy()
    {
        GameEvents.Instance.onRoomBecameActive -= OnRoomBecameActiveAction;
    }

    void OnRoomBecameActiveAction(Room room)
    {
        if (_room == room)
        {
            IsConditionCompleted = true;
            GameEvents.Instance.TriggerOnConditionCompleted(this);
        }
    }

    public void SetRoom(Room room)
    {
        _room = room;
    }
}
