using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorStates : MonoBehaviour
{
    private Animator _animator;
    private PlayerMoving _playerMoving;
    private PlayerIsGroundTrigger _playerIsGroundTrigger;
    private PlayerAttack _playerAttack;

    public event Action PlayerJumpEnd;

    [field: SerializeField] public string Idle { get; private set; }
    [field: SerializeField] public string IsJump { get; private set; }
    [field: SerializeField] public string IsJumpSecond { get; private set; }
    [field: SerializeField] public string IsAttack { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMoving = GetComponent<PlayerMoving>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerIsGroundTrigger = GetComponentInChildren<PlayerIsGroundTrigger>();
    }

    private void Start()
    {
        _playerMoving.PlayerJumpStart += (() =>
        {
            _animator.SetBool(IsJump, true);
        });

        _playerIsGroundTrigger.OnGroundEnter += (() =>
        {
            if (_playerMoving.IsJump)
                _animator.SetBool(IsJumpSecond, true);
        });

        _playerAttack.PlayerAttackButtonDown += (() => 
        {
            _animator.SetBool(IsAttack, true);
            _animator.SetBool(IsJumpSecond, false);
            _animator.SetBool(IsJump, false);
        });
    }

    public void JumpEnd()
    {
        _animator.SetBool(IsJumpSecond, false);
        _animator.SetBool(IsJump, false);
        PlayerJumpEnd?.Invoke();
    }
    public void AttackEnd() => _animator.SetBool(IsAttack, false);
}
