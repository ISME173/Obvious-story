using System;
using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private float _timeToJump;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Inject] private IUserInput _userInput;
    private Rigidbody2D _rigidbody2d;
    private PlayerIsGroundTrigger _isGroundTrigger;
    private PlayerAnimatorStates _playerAnimatorStates;

    private Coroutine _jump;

    public event Action PlayerJumpStart;

    public float Speed => _speed;
    public bool IsJump { get; private set; } = false;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _isGroundTrigger = GetComponentInChildren<PlayerIsGroundTrigger>();
        _playerAnimatorStates = GetComponent<PlayerAnimatorStates>();

        _userInput.OnPlayerJumpButtonDown += JumpActivate;
        _playerAnimatorStates.PlayerJumpEnd += (() =>
        {
            if (_jump != null)
                StopCoroutine(_jump);
            IsJump = false;
        });
    }

    private void FixedUpdate()
    {
        _rigidbody2d.velocity = new Vector2(_userInput.GetPlayerMovingHorizontalInput(Speed), _rigidbody2d.velocity.y);

        if (_rigidbody2d.velocity.x != 0)
            _spriteRenderer.flipX = _rigidbody2d.velocity.x < 0 ? true : false;
    }
    private void JumpActivate()
    {
        if (_isGroundTrigger.IsGround && IsJump == false)
        {
            IsJump = true;
            PlayerJumpStart?.Invoke();
            _jump = StartCoroutine(Jump(_timeToJump));
        }
    }
    private IEnumerator Jump(float timeToJump)
    {
        yield return new WaitForSeconds(timeToJump);
        _rigidbody2d.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}