using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorStates : MonoBehaviour
{
    private Animator _animator;
    private PlayerMoving _playerMoving;
    private PlayerIsGroundTrigger _playerIsGroundTrigger;
    private PlayerAttack _playerAttack;

    [field: SerializeField] public string Idle { get; private set; }
    [field: SerializeField] public string JumpTrigger { get; private set; }
    [field: SerializeField] public string FallTrigger { get; private set; }
    [field: SerializeField] public string AttackTrigger { get; private set; }
    [field: SerializeField] public string IsAttack { get; private set; }
    [field: SerializeField] public string IsGround { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMoving = GetComponent<PlayerMoving>();
        _playerIsGroundTrigger = GetComponentInChildren<PlayerIsGroundTrigger>();
        _playerAttack = GetComponent<PlayerAttack>();
    }
    private void Start()
    {
        _playerMoving.JumpActivate += () => 
        {
            _animator.SetTrigger(JumpTrigger);        
        };
        _playerIsGroundTrigger.OnGroundEnter += () => 
        {
            _animator.SetBool(IsGround, true);
        };
        _playerIsGroundTrigger.OnGroundExit += () =>
        {
           _animator.SetBool(IsGround, false);
        };
        _playerMoving.OnPlayerFall += () =>
        {
            _animator.SetTrigger(FallTrigger);
        };
        _playerAttack.PlayerAttackButtonDown += () =>
        {
            _animator.SetTrigger(AttackTrigger);
        };
    }
}
