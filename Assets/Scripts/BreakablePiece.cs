using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePiece : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sprite;
    [SerializeField]
    private float _lifetime = .5f;
    [SerializeField]
    private float _moveSpeed = 3f;
    [SerializeField]
    private float _deceleration = .5f;
    [SerializeField]
    private float _fadeTime = 2.5f;

    private Vector3 _moveDirection;

    void Start()
    {
        _moveDirection.x = Random.Range(-_moveSpeed, _moveSpeed);
        _moveDirection.y = Random.Range(-_moveSpeed, _moveSpeed);

        StartCoroutine(FadeToZeroRoutine());
    }

    void Update()
    {
        transform.position += _moveDirection * Time.deltaTime;
        _moveDirection = Vector3.Lerp(_moveDirection, Vector3.zero, _deceleration * Time.deltaTime);
    }

    IEnumerator FadeToZeroRoutine()
    {
        float fadeTime = 0f;
        while (fadeTime < _fadeTime)
        {
            fadeTime += Time.deltaTime;
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, Mathf.MoveTowards(_sprite.color.a, 0f, Time.deltaTime));
            yield return null;
        }

        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }
}
