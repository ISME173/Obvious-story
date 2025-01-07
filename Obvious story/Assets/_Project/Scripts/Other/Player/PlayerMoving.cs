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
    public event Action OnPlayerFall;

    public float Speed => _speed;
    public Vector2 MovingVelosity { get; private set; }

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _playerIsGroundTrigger = GetComponentInChildren<PlayerIsGroundTrigger>();

        _userInput.OnPlayerJumpButtonDown += Jump;
        _playerIsGroundTrigger.OnGroundEnter += () =>
        {
            CheckIsFallState();
        };
        
    }

    private void FixedUpdate()
    {
        MovingVelosity = new Vector2(_userInput.GetPlayerMovingHorizontalInput(Speed), _rigidbody2d.velocity.y);
        _rigidbody2d.velocity = MovingVelosity;

        if (_rigidbody2d.velocity.x != 0)
            _spriteRenderer.flipX = _rigidbody2d.velocity.x < 0 ? true : false;
    }
    private void CheckIsFallState()
    {
        while (_rigidbody2d.velocity.y >= 0)
            continue;

        OnPlayerFall?.Invoke();
    }
    private void Jump()
    {
        if (_playerIsGroundTrigger.IsGround)
        {
            JumpActivate?.Invoke();
            _rigidbody2d.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }
    private void OnDisable() => _userInput.OnPlayerJumpButtonDown -= Jump;
}