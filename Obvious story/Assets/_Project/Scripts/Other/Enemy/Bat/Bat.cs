using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public class Bat : Enemy
{
    [SerializeField] private Transform[] _movingPoints;

    private Vector2 _beforePositiion;
    private int _attackType = 1;
    private bool _isAttack = false;
 
    private void Start() => Init();

    private void Update()
    {
        if (RaycastToPlayer(AttackDistance) && _isAttack == false)
        {
            Attack();
        }
    }
    protected override void Init()
    {
        base.Init();

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        for (int i = 0; i < _movingPoints.Length; i++)
            _movingPoints[i].SetParent(null);
    }

    protected override void Attack()
    {
        _isAttack = true;
        FlipEnemyToTarget(PlayerMoving.transform);
        _animator.SetTrigger(IsAttack + _attackType);
        _attackType = Random.Range(1, 3);
    }
    protected override void Died()
    {
        _animator.SetBool(IsLive, false);
        _animator.SetTrigger(IsDied);
        gameObject.SetActive(false);
    }
    public override void Move(Transform point)
    {
        _beforePositiion = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, point.position, _movingSpeed * Time.deltaTime);
        _spriteRenderer.flipX = transform.position.x < _beforePositiion.x ? false : true;
    }
    public Transform[] GetMovingPoints() => _movingPoints;
    public void AttackEnd() => _isAttack = false;
}
