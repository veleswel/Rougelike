using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController current;

    [SerializeField]
    private float _transitionSpeed = 30f;

    [SerializeField]
    private Transform _target;

    void Awake()
    {
        current = this;
    }

    public void MoveToTarget(Transform target)
    {
        _target = target;
        Vector3 targetPosition = new Vector3(_target.position.x, _target.position.y, transform.position.z);
        StartCoroutine(MoveToTargetRoutine(targetPosition));
    }

    IEnumerator MoveToTargetRoutine(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            yield return null;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _transitionSpeed);
        }
    }
}
