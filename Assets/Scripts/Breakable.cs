using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _brokenPieces;
    [SerializeField]
    private int _maxPiecesToDrop = 5;

    [SerializeField]
    private int _breakSFXIdx;

    void Start()
    {
        if (_maxPiecesToDrop > _brokenPieces.Length)
        {
            Debug.LogError("Breakable: Max Pieces To Drop '" + "' is bigger that the number of pieces available '" + _brokenPieces.Length + "'");
            _maxPiecesToDrop = _brokenPieces.Length;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Player" && PlayerController.current.IsDashing()) || other.gameObject.tag == "PlayerBullet")
        {
            AudioManager.current.PlaySoundEffect(_breakSFXIdx);

            BreakIntoPieces();

            PickupDrop drop = GetComponent<PickupDrop>();
            if (drop != null)
            {
                drop.DropPickups();
            }

            Destroy(this.gameObject);
        }
    }

    void BreakIntoPieces()
    {
        int numPiecesToDrop = UnityEngine.Random.Range(1, _maxPiecesToDrop);

        List<int> listToDrop = GenerateUniqueRandomIndecesList(numPiecesToDrop, _brokenPieces.Length);

        for (int i = 0; i < listToDrop.Count; i++)
        {
            Instantiate(_brokenPieces[listToDrop[i]], transform.position, Quaternion.identity);
        }
    }

    List<int> GenerateUniqueRandomIndecesList(int length, int maxIndex)
    {
        List<int> list = new List<int>();

        for (int i = 0; i < length; i++)
        {
            int num = UnityEngine.Random.Range(0, maxIndex);
            while (list.Contains(num))
            {
                num = UnityEngine.Random.Range(0, maxIndex);
            }
                
            list.Add(num);
        }

        return list;
    }
}
