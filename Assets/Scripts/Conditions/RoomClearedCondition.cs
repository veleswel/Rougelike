using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomClearedCondition : BaseCondition
{
    [SerializeField]
    private Room _room;

    void Start()
    {
        GameEvents.Instance.onRoomCleared += OnRoomClearedAction;
    }

    void OnDestroy()
    {
        GameEvents.Instance.onRoomCleared -= OnRoomClearedAction;
    }

    void OnRoomClearedAction(Room room)
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
