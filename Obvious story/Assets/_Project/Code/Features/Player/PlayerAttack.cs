using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerAttackType[] _attackTypes;

    [Inject] private IUserInput _userInput;
    private bool _canAttack = true;

    public event Action<string> PlayerAttackActivate;
    public event Action PlayerAttackEnd;

    public int LastAttackDamage { get; private set; }

    private void Awake()
    {
        if (_attackTypes.Length <= 0)
            throw new ArgumentNullException($"{nameof(_attackTypes)} array is empty");

        GameManager.Instance.OnGameOver.AddListener((() =>
        {
            _userInput.OnPlayerAttackActivate -= AttackStart;
        }));
    }

    private void Start()
    {
        OnDamagableCollideCheckWithPlayer[] onPlayerCollideEnterChecks = GetComponentsInChildren<OnDamagableCollideCheckWithPlayer>();
        for (int i = 0; i < onPlayerCollideEnterChecks.Length; i++)
        {
            onPlayerCollideEnterChecks[i].OnDamagebleColliderEnter += (IDamagable damagable) =>
            {
                damagable.TakeDamage(LastAttackDamage);
            };
        }
    }

    private void OnEnable() => _userInput.OnPlayerAttackActivate += AttackStart;
    private void OnDisable() => _userInput.OnPlayerAttackActivate -= AttackStart;

    private void AttackStart()
    {
        if (_canAttack && GameManager.Instance.IsGameStarting)
        {
            _canAttack = false;

            PlayerAttackType playerAttackType = _attackTypes[UnityEngine.Random.Range(0, _attackTypes.Length)];

            PlayerAttackActivate?.Invoke(playerAttackType.AnimationName);
            StartCoroutine(CanAttackReload(playerAttackType.AnimationTime));

            LastAttackDamage = playerAttackType.AttackDamage;
        }
    }
    private IEnumerator CanAttackReload(float attackAnimationTime)
    {
        SoundController.Instance.PlayPlayerSound(PlayerSound.PlayerSoundTypes.SwordSwim, true, true, UnityEngine.Random.Range(0.7f, 1.2f));
        yield return new WaitForSeconds(attackAnimationTime);
        _canAttack = true;
        PlayerAttackEnd?.Invoke();
    }
}
