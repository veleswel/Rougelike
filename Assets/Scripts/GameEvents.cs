using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    void Awake()
    {
        current = this;
    }

    public event Action onPlayerDamaged, onPlayerDied, onPlayerWon;
    public event Action<float> onPlayerHealthChanged;
    public event Action<EnemyController> onEnemyDamaged, onEnemyDied;
    public event Action<Breakable> onBreakableBroken;

    public event Action<BaseCondition> onConditionCompleted;

    public event Action<Room> onRoomCleared;

    public void TriggerOnPlayerHealthChanged(float health)
    {
        if (onPlayerHealthChanged != null)
        {
            onPlayerHealthChanged(health);
        }
    }

    public void TriggerOnPlayerDied()
    {
        if (onPlayerDied != null)
        {
            onPlayerDied();
        }
    }

    public void TriggerOnPlayerWon()
    {
        if (onPlayerWon != null)
        {
            onPlayerWon();
        }
    }

    public void TriggerOnPlayerDamaged()
    {
        if (onPlayerDamaged != null)
        {
            onPlayerDamaged();
        }
    }

    public void TriggerOnEnemyDamaged(EnemyController enemy)
    {
        if (onEnemyDamaged != null)
        {
            onEnemyDamaged(enemy);
        }
    }

    public void TriggerOnEnemyDied(EnemyController enemy)
    {
        if (onEnemyDied != null)
        {
            onEnemyDied(enemy);
        }
    }

    public void TriggerOnBreakableBroken(Breakable breakable)
    {
        if (onBreakableBroken != null)
        {
            onBreakableBroken(breakable);
        }
    }

    public void TriggerOnConditionCompleted(BaseCondition condition)
    {
        if (onConditionCompleted != null)
        {
            onConditionCompleted(condition);
        }
    }

    public void TriggerOnRoomCleared(Room room)
    {
        if (onRoomCleared != null)
        {
            onRoomCleared(room);
        }
    }
}
