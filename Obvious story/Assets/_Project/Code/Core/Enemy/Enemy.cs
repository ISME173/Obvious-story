using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Enemy : MonoBehaviour, IDamagable, IDestroyable
{
    [Header("Enemy states"), Space]
    [SerializeField, Min(1)] protected int _attackDamage;
    [SerializeField, Min(1)] protected int _maxHeartsCount;
    [Space]
    [SerializeField, Min(1)] protected float _diedAnimationTime;
    [SerializeField, Min(1)] private int _attackVariantsCount = 1;
    [Space]
    [SerializeField, Min(1)] protected float _angryMovingSpeed;
    [SerializeField, Min(1)] protected float _normalMovingSpeed;

    [Header("Raycast states")]
    [SerializeField] protected Transform _raycastStartTransform;
    [SerializeField] protected LayerMask _ignoreLayerMaskForRaycast;

    [Header("Interaction with player")]
    [SerializeField] protected bool _canGoThroughAPlayer = false;

    protected PlayerMoving PlayerMoving;
    protected Transform[] _movingPoints;
    protected Collider2D _collider;
    protected Rigidbody2D _rigidbody2d;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected int _heartsCount;
    protected float _movingSpeed;

    public event Action<IDestroyable> Destroyed;
    public event Action<Enemy> OnDied;
    public event Action<Enemy> Disabled;

    [field: Header("Enemy animator properties"), Space]
    [field: SerializeField] public string IsIdle { get; protected set; }
    [field: SerializeField] public string IsAngry { get; protected set; }
    [field: SerializeField] public string IsLive { get; protected set; }
    [field: SerializeField] public string IsAttack { get; protected set; }
    [field: Space]
    [field: SerializeField] public string DiedTrigger { get; protected set; }
    [field: SerializeField] public string AttackTrigger { get; protected set; }
    [field: SerializeField] public string TakeDamageTrigger { get; protected set; }

    [field: Header("Distance to player, for..."), Space]
    [field: SerializeField] public float AngryDistance { get; protected set; }
    [field: SerializeField] public float AttackDistance { get; protected set; }

    [field: Space]
    [field: SerializeField] public float IdleTime { get; protected set; }
    [field: SerializeField] public float RunningTime { get; protected set; }

    public int AttackVariantsCount => _attackVariantsCount;

    private void OnDisable()
    {
        _collider.enabled = true;
        Disabled?.Invoke(this);
    }
    private void OnDestroy()
    {
        Destroyed?.Invoke(this);
    }

    protected abstract void OnCollisionEnter2D(Collision2D collision);
    protected abstract void OnTriggerExit2D(Collider2D collision);

    protected virtual void Attack(IDamagable damagable)
    {
        damagable.TakeDamage(_attackDamage);
    }
    protected virtual void Died()
    {
        OnDied?.Invoke(this);
        SetMovingSpeedToZero();
    }

    public virtual void Init(PlayerMoving playerMoving, Transform[] movingPoints, bool isInstantiated)
    {
        if (isInstantiated)
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody2d = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            OnEnemyCollideEnterCheck[] onEnemyCollideEnterChecks = GetComponentsInChildren<OnEnemyCollideEnterCheck>();
            for (int i = 0; i < onEnemyCollideEnterChecks.Length; i++)
                onEnemyCollideEnterChecks[i].OnCollideEnter += Attack;
        }

        PlayerMoving = playerMoving;
        _movingPoints = movingPoints;
        _heartsCount = _maxHeartsCount;
        _movingSpeed = _normalMovingSpeed;
        _collider.enabled = true;

        _animator.SetBool(IsLive, true);
    }

    public abstract void Move(Transform point);

    public virtual void FlipEnemyToTarget(Transform target)
    {
        transform.rotation = transform.position.x < target.position.x ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }
    public bool RaycastToPlayer(float distance)
    {
        if (GameManager.Instance.IsGameStarting == false)
            return false;

        Vector3 direction = this.PlayerMoving.TargetPoint.position - _raycastStartTransform.position;
        RaycastHit2D hit = Physics2D.Raycast(_raycastStartTransform.transform.position, direction, distance, ~_ignoreLayerMaskForRaycast);

        return hit && hit.collider.TryGetComponent(out PlayerMoving PlayerMoving);
    }
    public virtual void TakeDamage(int damage)
    {
        if (_heartsCount <= 0)
            return;

        //Debug.Log($"{gameObject.name} take damage");

        _heartsCount -= damage;
        _animator.SetTrigger(TakeDamageTrigger);

        if (_heartsCount <= 0)
        {
            _heartsCount = 0;
            Died();
        }
    }
    public Transform GetPlayerTargetPoint() => PlayerMoving.TargetPoint;
    public Transform[] GetMovingPoints() => _movingPoints;
    public void AngryMovingSpeedActivate() => _movingSpeed = _angryMovingSpeed;
    public void NormalMovingSpeedActivate() => _movingSpeed = _normalMovingSpeed;
    public void SetMovingSpeedToZero()
    {
        _rigidbody2d.velocity = Vector3.zero;
        _movingSpeed = 0;
    }
}