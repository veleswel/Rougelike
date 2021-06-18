using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    static CameraController _instance;
    public static CameraController Instance { get { return _instance; } }

    [SerializeField]
    private float _transitionSpeed = 30f;

    [SerializeField]
    private Transform _target;

    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        if (_target == null)
        {
            return;
        }
        
        Vector3 targetPosition = new Vector3(_target.position.x, _target.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _transitionSpeed);
    }

    public void MoveToTarget(Transform target)
    {
        _target = target;
    }
}
