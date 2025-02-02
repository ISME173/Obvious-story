using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObject/Enemy/EnemyData")]
public abstract class EnemyData : ScriptableObject
{
    [Header("Enemy states"), Space]
    [SerializeField, Min(1)] protected int _attackDamage;
    [SerializeField, Min(1)] protected int _maxHeartsCount;
    [Space]
    [SerializeField, Min(1)] protected float _diedAnimationTime;
    [SerializeField, Min(1)] protected int _attackVariantsCount = 1;
    [Space]
    [SerializeField, Min(1)] protected float _angryMovingSpeed;
    [SerializeField, Min(1)] protected float _normalMovingSpeed;
    [Space]
    [SerializeField] protected bool _usingVariationForStates;
    [ShowIf(nameof(_usingVariationForStates))]
    [SerializeField, Min(0.1f)] protected float _variationForStates = 0.1f;

    [Header("Raycast states"), Space]
    [SerializeField] protected Transform _raycastStartTransform;
    [SerializeField] protected LayerMask _ignoreLayerMaskForRaycast;

    [Header("Interaction with player and any enemy"), Space]
    [SerializeField] protected bool _canGoThroughAPlayerAndEnemy = true;
    [SerializeField] protected bool _canTakeDamageAnyEnemy = false;

    [field: Header("State settings"), Space]
    [field: SerializeField, Min(0.1f)] public float AngryDistance { get; protected set; }
    [field: SerializeField, Min(0.1f)] public float AttackDistance { get; protected set; }
    [field: Space]
    [field: SerializeField, Min(0.1f)] public float IdleTime { get; protected set; }
    [field: SerializeField, Min(0)] public float RunningTime { get; protected set; }
    [field: Space]
    [field: SerializeField, Min(0)] public float StoppingDistance { get; protected set; } = 0.5f;

    [field: Header("Enemy animator properties"), Space]
    [field: SerializeField] public string IsIdle { get; protected set; }
    [field: SerializeField] public string IsAngry { get; protected set; }
    [field: SerializeField] public string IsLive { get; protected set; }
    [field: SerializeField] public string IsAttack { get; protected set; }
    [field: Space]
    [field: SerializeField] public string DiedTrigger { get; protected set; }
    [field: SerializeField] public string AttackTrigger { get; protected set; }
    [field: SerializeField] public string TakeDamageTrigger { get; protected set; }
}
