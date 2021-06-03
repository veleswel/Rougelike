using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDrop : MonoBehaviour
{
    [SerializeField]
    private ItemToDropDescription[] _itemsToDrop;

    public void DropPickups()
    {
        PickupDropManger.current.DropItems(_itemsToDrop, transform.position, 0.5f);
    }
}