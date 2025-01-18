using UnityEngine;
using Zenject;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    [Header("Enemy states"), Space]
    [SerializeField, Min(1)] protected int _attackDamage;
    [SerializeField, Min(1)] protected int _maxHeartsCount;
    [Space]
    [SerializeField, Min(1)] protected float _diedAnimationTime;
    [Space]
    [SerializeField, Min(1)] protected float _angryMovingSpeed;
    [SerializeField, Min(1)] protected float _normalMovingSpeed;

    [Header("Other")]
    [SerializeField] protected Transform _raycastStartTransform;
    [SerializeField] protected Transform _attackTriggers;
    [SerializeField] protected LayerMask _ignoreLayerMask;

    [field: Header("Enemy animator properties"), Space]
    [field: SerializeField] public string IsIdle { get; protected set; }
    [field: SerializeField] public string IsLive { get; protected set; }
    [field: SerializeField] public string IsTakeDamage { get; protected set; }
    [field: SerializeField] public string IsDied { get; protected set; }
    [field: SerializeField] public string IsAttack { get; protected set; }
    [field: SerializeField] public string IsAngry { get; protected set; }

    [field: Header("Distance to player, for..."), Space]
    [field: SerializeField] public float AngryDistance { get; protected set; }
    [field: SerializeField] public float AttackDistance { get; protected set; }

    [field: Space]
    [field: SerializeField] public float IdleTime { get; protected set; }
    [field: SerializeField] public float RunningTime { get; protected set; }

    [Inject] public PlayerMoving PlayerMoving { get; protected set; }

    protected Collider2D _collider;
    protected Rigidbody2D _rigidbody2d;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected int _heartsCount;
    protected float _movingSpeed;

    protected virtual void Init()
    {
        _heartsCount = _maxHeartsCount;
        _collider = GetComponent<Collider2D>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.SetBool(IsLive, true);

        OnEnemyCollideEnterCheck[] onEnemyCollideEnterChecks = GetComponentsInChildren<OnEnemyCollideEnterCheck>();
        for (int i = 0; i < onEnemyCollideEnterChecks.Length; i++)
            onEnemyCollideEnterChecks[i].OnCollideEnter += Attack;
    }
    protected virtual void Attack(IDamagable damagable)
    {
        damagable.TakeDamage(_attackDamage);
    }
    protected abstract void Died();


    public abstract void Move(Transform point);

    public virtual void FlipEnemyToTarget(Transform target)
    {
        //_spriteRenderer.flipX = transform.position.x < target.position.x ? true : false;
        transform.rotation = transform.position.x < target.position.x ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }
    public bool RaycastToPlayer(float distance)
    {
        Vector3 direction = PlayerMoving.TargetPoint.position - _raycastStartTransform.position;
        RaycastHit2D hit = Physics2D.Raycast(_raycastStartTransform.transform.position, direction, distance, ~_ignoreLayerMask);

        return hit && hit.collider.TryGetComponent(out PlayerMoving playerMoving);
    }
    public virtual void TakeDamage(int damage)
    {
        if (_heartsCount <= 0)
            return;

        _heartsCount -= damage;
        Debug.Log(_heartsCount);
        _animator.SetTrigger(IsTakeDamage);

        if (_heartsCount <= 0)
        {
            _heartsCount = 0;
            Died();
        }
    }
    public void AngryMovingSpeedActivate() => _movingSpeed = _angryMovingSpeed;
    public void NormalMovingSpeedActivate() => _movingSpeed = _normalMovingSpeed;
}
