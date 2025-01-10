using UnityEngine;
using Zenject;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy states"), Space]
    [SerializeField, Min(1)] protected int _attackDamage;
    [SerializeField, Min(1)] protected int _movingSpeed;
    [SerializeField, Min(1)] protected int _maxHealth;

    [Header("Other")]
    [SerializeField] protected Transform _objectForRaycast;
    [SerializeField] protected LayerMask _ignoreLayerMask;

    [field: Header("Enemy animator parameters"), Space]
    [field: SerializeField] public string IsIdle { get; protected set; }
    [field: SerializeField] public string IsLive { get; protected set; }
    [field: SerializeField] public string IsTakeDamage { get; protected set; }
    [field: SerializeField] public string IsDied { get; protected set; }
    [field: SerializeField] public string IsAttack { get; protected set; }

    [field: Header("Distance to player, for..."), Space]
    [field: SerializeField] public float AngryDistance { get; protected set; }
    [field: SerializeField] public float AttackDistance { get; protected set; }

    [field: Space]
    [field: SerializeField] public float IdleTime { get; protected set; }
    [field: SerializeField] public float RunningTime { get; protected set; }
    [field: SerializeField] public float AngryTime { get; protected set; }
 
    [Inject] public PlayerMoving PlayerMoving { get; protected set; }
    
    protected int _health;
    protected Collider2D _collider;
    protected Rigidbody2D _rigidbody2d;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    protected virtual void Init()
    {
        _health = _maxHealth;
        _collider = GetComponent<Collider2D>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    protected bool RaycastToPlayer(float distance)
    {
        Vector3 direction = PlayerMoving.transform.position - _objectForRaycast.position;

        RaycastHit2D hit = Physics2D.Raycast(_objectForRaycast.transform.position, direction, distance, ~_ignoreLayerMask);

        if (hit && hit.collider.TryGetComponent(out PlayerMoving playerMoving))
            return true;
        else
            return false;   
    }
    protected void FlipEnemyToTarget(Transform target)
    {
        if (transform.position.x < target.position.x)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
    }
    protected abstract void Died();
    protected abstract void Attack();

    public abstract void Move(Transform point);

    public virtual void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;
            Died();
        }
    }
}
