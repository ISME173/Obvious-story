using NaughtyAttributes;
using UnityEngine;

public abstract class EnemyData : ScriptableObject
{
    [Header("Enemy states"), Space]
    [SerializeField, Min(1)] private int _attackDamage;
    [SerializeField, Min(1)] private int _maxHeartsCount;
    [Space]
    [SerializeField, Min(1)] private float _diedAnimationTime;
    [SerializeField, Min(1)] private int _attackVariantsCount;
    [Space]
    [SerializeField, Min(1)] private float _angryMovingSpeed;
    [SerializeField, Min(1)] private float _normalMovingSpeed;
    [Space]
    [Tooltip("scatter for Enemy parameters such as quiet time and movement time")]
    [SerializeField] private bool _usingVariationForStates;
    [ShowIf(nameof(_usingVariationForStates))]
    [SerializeField, Min(0.1f)] private float _variationForStates;

    [Header("Raycast states"), Space]
    [SerializeField] private LayerMask _ignoreLayerMaskForRaycast;

    [Header("Interaction with player and any enemy"), Space]
    [SerializeField] private bool _canTakeDamageAnyEnemy = false;
    [SerializeField] private bool _canGoThroughAPlayerAndEnemy = true;
    [Space]
    [SerializeField] private string _groundLayerNameForEnemy;

    [Header("State settings"), Space]
    [SerializeField, Min(0.1f)] private float _angryDistance;
    [SerializeField, Min(0.1f)] private float _attackDistance;
    [Space]
    [SerializeField, Min(0.1f)] private float _idleTime;
    [SerializeField, Min(0)] private float _runningTime;
    [Space]
    [SerializeField, Min(0)] private float _stoppingDistance = 0.5f;

    [Header("Enemy animator properties"), Space]
    [SerializeField] private string _isIdle;
    [SerializeField] private string _isAngry;
    [SerializeField] private string _isLive;
    [SerializeField] private string _isAttack;
    [Space]
    [SerializeField] private string _diedTrigger;
    [SerializeField] private string _attackTrigger;
    [SerializeField] private string _takeDamageTrigger;

    public int AttackDamage => _attackDamage;
    public int MaxHeartsCount => _maxHeartsCount;

    public float DiedAnimationTime => _diedAnimationTime;
    public int AttackVariantsCount => _attackVariantsCount;

    public float NormalMovingSpeed => _normalMovingSpeed;
    public float AngryMovingSpeed => _angryMovingSpeed;

    public bool UsingVariationForStates => _usingVariationForStates;
    public float VariationForStates => _variationForStates;

    public LayerMask IgnoreLayerMaskForRaycast => _ignoreLayerMaskForRaycast;

    public bool CanGoThroughAPlayerAndEnemy => _canGoThroughAPlayerAndEnemy;
    public bool CanTakeDamageAnyEnemy => _canTakeDamageAnyEnemy;

    public float AngryDistance => _angryDistance;
    public float AttackDistance => _attackDistance;

    public float IdleTime => _idleTime;
    public float RunningTime => _runningTime;

    public float StoppingDistance => _stoppingDistance;

    public string IsIdle => _isIdle;
    public string IsAngry => _isAngry;
    public string IsAttack => _isAttack;
    public string IsLive => _isLive;

    public string DiedTrigger => _diedTrigger;
    public string TakeDamageTrigger => _takeDamageTrigger;
    public string AttackTrigger => _attackTrigger;
}
