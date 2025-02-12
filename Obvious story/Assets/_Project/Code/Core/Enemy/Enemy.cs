using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Enemy<T> : EnemyBase where T : EnemyData
{
    [SerializeField] protected Transform _raycastStartTransform;
    [SerializeField] protected T _enemyData;

    protected PlayerMoving _playerMoving;
    protected Transform[] _movingPoints;
    protected Collider2D _collider;
    protected Rigidbody2D _rigidbody2d;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected int _heartsCount;
    protected float _movingSpeed;
    protected bool _isLive = true;
    protected bool _isGround = false;

    public override event Action<IDestroyable> Destroyed;
    public event Action<Enemy<T>> OnDied;
    public event Action<Enemy<T>> Disabled;

    public T EnemyData => _enemyData;

    public float IdleTime { get; private set; }
    public float RunningTime { get; private set; }

    private void Awake()
    {
        if (_enemyData.UsingVariationForStates)
        {
            IdleTime = _enemyData.IdleTime.WithDeviation(_enemyData.VariationForStates);
            RunningTime = _enemyData.RunningTime.WithDeviation(_enemyData.VariationForStates);
        }
        else
        {
            IdleTime = _enemyData.IdleTime;
            RunningTime = _enemyData.RunningTime;
        }
    }

    private void OnDisable()
    {
        _collider.enabled = true;
        Disabled?.Invoke(this);
    }
    private void OnDestroy()
    {
        Destroyed?.Invoke(this);
    }

    protected virtual void Attack(IDamagable damagable)
    {
        damagable.TakeDamage(_enemyData.AttackDamage);
    }
    protected abstract void AddAttackListenersToOnDamagebleCollideCheckWithEnemy();
    protected virtual void Died()
    {
        OnDied?.Invoke(this);
    }

    public virtual void Init(PlayerMoving playerMoving, Transform[] movingPoints, bool isInstantiate)
    {
        if (isInstantiate)
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody2d = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            AddAttackListenersToOnDamagebleCollideCheckWithEnemy();
        }

        Physics2D.IgnoreCollision(_collider, playerMoving.GetComponent<Collider2D>(), EnemyData.CanGoThroughAPlayerAndEnemy);

        _playerMoving = playerMoving;
        _movingPoints = movingPoints;
        _heartsCount = _enemyData.MaxHeartsCount;
        _movingSpeed = _enemyData.NormalMovingSpeed;
        _collider.enabled = true;
        _isLive = true;

        _animator.SetBool(_enemyData.IsLive, _isLive);
    }

    public abstract void Move(Transform point);

    public virtual void FlipEnemyToTarget(Transform target)
    {
        transform.rotation = transform.position.x < target.position.x ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }
    protected RaycastHit2D RaycastToTarget(Transform target, float distance)
    {
        Vector3 direction = target.position - _raycastStartTransform.position;
        RaycastHit2D hit = Physics2D.Raycast(_raycastStartTransform.transform.position, direction, distance, ~_enemyData.IgnoreLayerMaskForRaycast);

        return hit;
    }
    public virtual bool RaycastToPlayer(float distance)
    {
        if (GameEvents.Instance.IsGameStarting == false)
            return false;

        RaycastHit2D raycastHit2D = RaycastToTarget(_playerMoving.TargetPoint, distance);
        return raycastHit2D && raycastHit2D.collider.TryGetComponent(out PlayerMoving PlayerMoving);
    }
    public override void TakeDamage(int damage)
    {
        if (_isLive == false)
            return;

        _heartsCount -= damage;
        _animator.SetTrigger(_enemyData.TakeDamageTrigger);

        //Debug.Log($"{gameObject.name} take damage. Hearts count: {_heartsCount}");

        if (_heartsCount <= 0)
        {
            _heartsCount = 0;
            _isLive = false;
            Died();
        }
    }
    public Transform GetPlayerTargetPoint() => _playerMoving.TargetPoint;
    public Transform[] GetMovingPoints() => _movingPoints;
    public void AngryMovingSpeedActivate()
    {
        _movingSpeed = _enemyData.UsingVariationForStates ? _enemyData.AngryMovingSpeed.WithDeviation(_enemyData.VariationForStates) : _enemyData.AngryMovingSpeed;
    }
    public void NormalMovingSpeedActivate()
    {
        _movingSpeed = _enemyData.UsingVariationForStates ? _enemyData.NormalMovingSpeed.WithDeviation(_enemyData.VariationForStates) : _enemyData.NormalMovingSpeed;
    }
    public void StopEnemyInIsIdleState()
    {
        _rigidbody2d.velocity = Vector3.zero;
        _movingSpeed = 0;
    }
}