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

    [field: Header("Boolean states")]
    [field: SerializeField] public string IsIdle { get; private set; }
    [field: SerializeField] public string IsGround { get; private set; }
    [field: SerializeField] public string IsLive { get; private set; }

    [field: Header("Triggers")]
    [field: SerializeField] public string JumpTrigger { get; private set; }
    [field: SerializeField] public string FallTrigger { get; private set; }
    [field: SerializeField] public string IsIdleTrigger { get; private set; }
    [field: SerializeField] public string TakeDamageTrigger { get; private set; }
    [field: SerializeField] public string DiedTrigger { get; private set; }

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
    private void OnEnable()
    {
        AddListeners();
    }
    private void AddListeners()
    {
        _playerIsGroundTrigger.OnGroundEnter += () =>
        {
            _animator.SetBool(IsGround, true);
        };
        _playerIsGroundTrigger.OnGroundExit += () =>
        {
            _animator.SetBool(IsGround, false);
        };

        _playerMoving.JumpActivate += () =>
        {
            _animator.SetTrigger(JumpTrigger);
        };
        _playerFallCheker.OnPlayerFall += (bool isFall) =>
        {
            string triggersActivate = isFall ? FallTrigger : IsIdleTrigger;
            _animator.SetTrigger(triggersActivate);
        };

        _playerAttack.PlayerAttackActivate += (string attackName) =>
        {
            _animator.SetTrigger(attackName);
        };

        _playerHealthManager.DamageTaken += (int _) =>
        {
            _animator.SetTrigger(TakeDamageTrigger);
        };
        _playerHealthManager.PlayerDied.AddListener((() =>
        {
            _animator.SetBool(IsLive, false);
            _animator.SetTrigger(DiedTrigger);
        }));
    }
}
