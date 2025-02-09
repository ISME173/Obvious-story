using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private Transform _attackTriggersObject;

    [Inject] private IUserInput _userInput;
    private Rigidbody2D _rigidbody2d;
    private PlayerIsGroundTrigger _playerIsGroundTrigger;
    private Collider2D _collider2D;
    private bool _canMoving = true;

    public event Action JumpActivate;

    public Vector2 TargetVelosity { get; private set; }
    public Rigidbody2D Rigidbody2D => _rigidbody2d;
    public Transform TargetPoint => _targetPoint;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _playerIsGroundTrigger = GetComponentInChildren<PlayerIsGroundTrigger>();
    }

    private void Start()
    {
        GameManager.Instance.OnGameOver.AddListener((() =>
        {
            _canMoving = false;

            _playerIsGroundTrigger.OnGroundStay += () =>
            {
                _rigidbody2d.isKinematic = true;
                _rigidbody2d.simulated = false;
            };

            _collider2D.enabled = false;
            _userInput.OnPlayerJumpButtonDown -= Jump;
            _rigidbody2d.velocity = Vector2.zero;
            _speed = 0;
        }));
        GameManager.Instance.OnGamePause.AddListener(() =>
        {
            _rigidbody2d.velocity = new Vector2(0, _rigidbody2d.velocity.y);
            TargetVelosity = _rigidbody2d.velocity;
        });
    }

    private void OnEnable()
    {
        _userInput.OnPlayerJumpButtonDown += Jump;
    }
    private void OnDisable()
    {
        _userInput.OnPlayerJumpButtonDown -= Jump;
    }

    private void FixedUpdate()
    {
        if (_canMoving == false || GameManager.Instance.IsGameStarting == false)
            return;

        TargetVelosity = new Vector2(_userInput.GetPlayerMovingHorizontalInput(_speed), _rigidbody2d.velocity.y);
        _rigidbody2d.velocity = TargetVelosity;

        if (_rigidbody2d.velocity.x != 0)
        {
            _spriteRenderer.flipX = _rigidbody2d.velocity.x < 0 ? true : false;
            _attackTriggersObject.rotation = _spriteRenderer.flipX ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        }
    }
    private void Jump()
    {
        if ((_canMoving && _playerIsGroundTrigger.IsGround) && GameManager.Instance.IsGameStarting)
        {
            JumpActivate?.Invoke();
            _rigidbody2d.velocity = Vector2.up * _jumpForce;
        }
    }
}