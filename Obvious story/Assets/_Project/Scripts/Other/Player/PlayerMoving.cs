using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Inject] private IUserInput _userInput;
    private Rigidbody2D _rigidbody2d;
    private PlayerIsGroundTrigger _playerIsGroundTrigger;

    public event Action JumpActivate;

    public Vector2 TargetVelosity { get; private set; }
    public Rigidbody2D Rigidbody2D => _rigidbody2d;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _playerIsGroundTrigger = GetComponentInChildren<PlayerIsGroundTrigger>();

        _userInput.OnPlayerJumpButtonDown += Jump;
    }

    private void FixedUpdate()
    {
        TargetVelosity = new Vector2(_userInput.GetPlayerMovingHorizontalInput(_speed), _rigidbody2d.velocity.y);
        _rigidbody2d.velocity = TargetVelosity;

        if (_rigidbody2d.velocity.x != 0)
            _spriteRenderer.flipX = _rigidbody2d.velocity.x < 0 ? true : false;
    }
    private void Jump()
    {
        if (_playerIsGroundTrigger.IsGround)
        {
            JumpActivate?.Invoke();
            _rigidbody2d.velocity = Vector2.up * _jumpForce;
        }
    }
    private void OnDisable() => _userInput.OnPlayerJumpButtonDown -= Jump;
}