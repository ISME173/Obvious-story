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
    private PlayerIsGroundTrigger _isGroundTrigger;
    private PlayerAnimatorStates _playerAnimatorStates;

    public event Action PlayerJumpStart;

    public float Speed => _speed;
    public bool IsJump { get; private set; } = false;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _isGroundTrigger = GetComponentInChildren<PlayerIsGroundTrigger>();
        _playerAnimatorStates = GetComponent<PlayerAnimatorStates>();

        _userInput.OnPlayerJumpButtonDown += Jump;
        _playerAnimatorStates.PlayerJumpEnd += (() => 
        {
            IsJump = false;
        });
    }

    private void FixedUpdate()
    {
        _rigidbody2d.velocity = new Vector2(_userInput.GetPlayerMovingHorizontalInput(Speed), _rigidbody2d.velocity.y);
        _spriteRenderer.flipX = _rigidbody2d.velocity.x < 0 ? true : false;
    }
    private void Jump()
    {
        if (_isGroundTrigger.IsGround)
        {
            PlayerJumpStart?.Invoke();
            _rigidbody2d.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            IsJump = true;
        }
    }
}