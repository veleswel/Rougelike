using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemToDropDescription
{
    public GameObject item;
    public float chance;
}

public class PickupDropManger : MonoBehaviour
{
    public static PickupDropManger current;
    
    void Awake()
    {
        current = this;
    }

    public void DropItems(ItemToDropDescription[] itemsToDrop, Vector3 initialPosition, float delay = 0f)
    {
        StartCoroutine(DropItemsRoutine(itemsToDrop, initialPosition, delay));
    }

    IEnumerator DropItemsRoutine(ItemToDropDescription[] itemsToDrop, Vector3 initialPosition, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        
        foreach(ItemToDropDescription itemDesc in itemsToDrop)
        {
            float dropChance = Random.Range(0f, 100f);
            if (dropChance < itemDesc.chance)
            {
                Vector3 randomOffset = new Vector3();
                randomOffset.x = Random.Range(-1f, 1f);
                randomOffset.y = Random.Range(-1f, 1f);

                Instantiate(itemDesc.item, initialPosition + randomOffset, Quaternion.identity);
            }
        }
    }
}
