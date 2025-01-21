using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorStates : MonoBehaviour
{
    [SerializeField] private PlayerFallCheker _playerFallCheker;
    [Space]

    private Animator _animator;
    private PlayerMoving _playerMoving;
    private PlayerIsGroundTrigger _playerIsGroundTrigger;
    private PlayerAttack _playerAttack;
    private PlayerHealthManager _playerHealthManager;

    [field: SerializeField] public string IsIdle { get; private set; }
    [field: SerializeField] public string IsGround { get; private set; }
    [field: SerializeField] public string IsLive { get; private set; }
    [field: SerializeField] public string JumpTrigger { get; private set; }
    [field: SerializeField] public string FallTrigger { get; private set; }
    [field: SerializeField] public string IsIdleTrigger { get; private set; }
    [field: SerializeField] public string TakeDamageTrigger { get; private set; }
    [field: SerializeField] public string DiedTrigger { get; private set; }

    [field: SerializeField, Tooltip("Write the name of the trigger without the number at the end ")]
    public string AttackTrigger { get; private set; } = "AttackTrigger";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMoving = GetComponent<PlayerMoving>();
        _playerIsGroundTrigger = GetComponentInChildren<PlayerIsGroundTrigger>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerHealthManager = GetComponent<PlayerHealthManager>();

        if (_playerFallCheker == null)
            _playerFallCheker = FindAnyObjectByType<PlayerFallCheker>();

        _animator.SetBool(IsLive, true);
    }
    private void Start() => AddListeners();

    private void AddListeners()
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
                _animator.SetTrigger(IsIdleTrigger);
        };
        _playerAttack.PlayerAttackActivate += (int currentAttack) =>
        {
            _animator.SetTrigger(AttackTrigger + currentAttack);
        };
        _playerHealthManager.DamageTaken += (int _) =>
        {
            _animator.SetTrigger(TakeDamageTrigger);
        };
        _playerHealthManager.PlayerDied += () =>
        {
            _animator.SetBool(IsLive, false);
            _animator.SetTrigger(DiedTrigger);
        };
    }
}
