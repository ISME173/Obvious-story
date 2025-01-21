using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[Serializable]
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
    private PlayerHealthManager _playerHealthManager;
    private bool _canMoving = true;

    public event Action JumpActivate;

    public Vector2 TargetVelosity { get; private set; }
    public Rigidbody2D Rigidbody2D => _rigidbody2d;
    public Transform TargetPoint => _targetPoint;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _playerIsGroundTrigger = GetComponentInChildren<PlayerIsGroundTrigger>();
        _playerHealthManager = GetComponent<PlayerHealthManager>();

        _userInput.OnPlayerJumpButtonDown += Jump;

        _playerHealthManager.PlayerDied += () =>
        {
            _canMoving = false;
            _userInput.OnPlayerJumpButtonDown -= Jump;
            _rigidbody2d.velocity = Vector2.zero;
        };
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
        if (_playerIsGroundTrigger.IsGround && GameManager.Instance.IsGameStarting)
        {
            JumpActivate?.Invoke();
            _rigidbody2d.velocity = Vector2.up * _jumpForce;
        }
    }
    private void OnDisable() => _userInput.OnPlayerJumpButtonDown -= Jump;
}