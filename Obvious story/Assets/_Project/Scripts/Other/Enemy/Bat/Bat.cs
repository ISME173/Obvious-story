using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public class Bat : Enemy
{
    [SerializeField] private Transform[] _movingPoints;

    private Vector2 _beforePositiion;
 
    private void Awake() => Init();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMoving playerMoving)) ;
            //.Log("PlayerTakeDamage");
    }

    protected override void Init()
    {
        base.Init();

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        for (int i = 0; i < _movingPoints.Length; i++)
            _movingPoints[i].SetParent(null);
    }
    protected override void Died()
    {
        _animator.SetBool(IsLive, false);
        _animator.SetTrigger(IsDied);

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            return;

        gameObject.SetActive(false);
    }
    public override void Move(Transform point)
    {
        _beforePositiion = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, point.position, _movingSpeed * Time.deltaTime);
        _spriteRenderer.flipX = transform.position.x < _beforePositiion.x ? false : true;
    }
    public Transform[] GetMovingPoints() => _movingPoints;
}
