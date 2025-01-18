using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerAttackType[] _attackTypes;

    [Inject] private IUserInput _userInput;
    private PlayerHealthManager _playerHealthManager;
    private bool _canAttack = true;

    public event Action<int> PlayerAttackActivate;
    public event Action PlayerAttackEnd;

    public int LastAttackDamage { get; private set; }

    private void Awake()
    {
        _playerHealthManager = GetComponent<PlayerHealthManager>();

        if (_attackTypes.Length <= 0)
            throw new ArgumentNullException($"_playerHealthManager array is empty");

    }

    private void Start()
    {
        _playerHealthManager.PlayerDied += () =>
        {
            _userInput.OnPlayerAttackActivate -= AttackStart;
        };

        OnPlayerCollideEnterCheck[] onPlayerCollideEnterChecks = GetComponentsInChildren<OnPlayerCollideEnterCheck>();
        for (int i = 0; i < onPlayerCollideEnterChecks.Length; i++)
        {
            onPlayerCollideEnterChecks[i].OnCollideEnter += (IDamagable damagable) =>
            {
                damagable.TakeDamage(LastAttackDamage);
            };
        }
    }

    private void OnEnable() => _userInput.OnPlayerAttackActivate += AttackStart;
    private void OnDisable() => _userInput.OnPlayerAttackActivate -= AttackStart;

    private void AttackStart()
    {
        if (_canAttack)
        {
            _canAttack = false;

            PlayerAttackType playerAttackType = _attackTypes[UnityEngine.Random.Range(0, _attackTypes.Length)];

            PlayerAttackActivate?.Invoke(playerAttackType.AnimationNumber);
            StartCoroutine(CanAttackReload(playerAttackType.AnimationTime));

            LastAttackDamage = playerAttackType.AttackDamage;
        }
    }
    private IEnumerator CanAttackReload(float attackAnimationTime)
    {
        yield return new WaitForSeconds(attackAnimationTime);
        _canAttack = true;
        PlayerAttackEnd?.Invoke();
    }
}
