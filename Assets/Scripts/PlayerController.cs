using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController s_instance;

    [SerializeField]    
    private float _moveSpeed = 3f;

    private Vector2 _moveInput;

    private Rigidbody2D _rigidBody;

    [SerializeField]    
    private Transform _gunHand;

    [SerializeField]    
    private SpriteRenderer _bodySprite;

    public SpriteRenderer BodySprite { get { return _bodySprite; } }

    private Camera _mainCamera;

    private Animator _animator;

    [SerializeField]    
    private GameObject _bullet;

    [SerializeField]    
    private Transform _firePoint;

    [SerializeField]    
    private float _autoFireRate = 0.25f;
    private float _nextShot = 0f;

    void Awake()
    {
        s_instance = this;
    }

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        if (_rigidBody == null)
        {
            Debug.LogError("PlayerController's Rigidbody2D is missing!");
        }

        _mainCamera = Camera.main;

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("PlayerController's Animator is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();

        if (_moveInput != Vector2.zero)
        {
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }

        _rigidBody.velocity = _moveInput * _moveSpeed;

        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(transform.localPosition);

        if (mousePosition.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            _gunHand.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            _gunHand.localScale = Vector3.one;
        }

        Vector2 diff = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        _gunHand.rotation = Quaternion.Euler(Vector3.forward * angle);

        if (Input.GetMouseButton(0) && Time.time > _nextShot)
        {
            _nextShot = Time.time + _autoFireRate;
            Instantiate(_bullet, _firePoint.position, _firePoint.rotation);
        }
    }
}
