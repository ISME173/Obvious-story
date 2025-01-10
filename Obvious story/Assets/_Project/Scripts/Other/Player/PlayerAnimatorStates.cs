using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorStates : MonoBehaviour
{
    [SerializeField] private PlayerFallCheker _playerFallCheker;

    private Animator _animator;
    private PlayerMoving _playerMoving;
    private PlayerIsGroundTrigger _playerIsGroundTrigger;
    private PlayerAttack _playerAttack;

    [field: SerializeField] public string Idle { get; private set; }
    [field: SerializeField] public string IsGround { get; private set; }
    [field: SerializeField] public string JumpTrigger { get; private set; }
    [field: SerializeField] public string FallTrigger { get; private set; }
    [field: SerializeField] public string IdleTrigger { get; private set; }
    [field: SerializeField, Tooltip("Write the name of the trigger without the number at the end ")] public string AttackTrigger { get; private set; } = "AttackTrigger";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMoving = GetComponent<PlayerMoving>();
        _playerIsGroundTrigger = GetComponentInChildren<PlayerIsGroundTrigger>();
        _playerAttack = GetComponent<PlayerAttack>();

        if (_playerFallCheker == null)
            _playerFallCheker = FindAnyObjectByType<PlayerFallCheker>();
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
        _playerFallCheker.OnPlayerFall += (bool isFall) =>
        {
            if (isFall)
                _animator.SetTrigger(FallTrigger);
            else
                _animator.SetTrigger(IdleTrigger);
        };
        _playerAttack.PlayerAttackButtonDown += (int currentAttack) =>
        {
            _animator.SetTrigger(AttackTrigger + currentAttack);
        };

    }
}
