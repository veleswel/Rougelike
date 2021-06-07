using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomClearedCondition : BaseCondition
{
    [SerializeField]
    private Room _room;

    void Start()
    {
        GameEvents.current.onRoomCleared += OnRoomClearedAction;
    }

    void OnDestroy()
    {
        GameEvents.current.onRoomCleared -= OnRoomClearedAction;
    }

    void OnRoomClearedAction(Room room)
    {
        if (_room == room)
        {
            IsConditionCompleted = true;
            GameEvents.current.TriggerOnConditionCompleted(this);
        }
    }
}
