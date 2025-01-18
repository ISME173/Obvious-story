using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public class Bat : Enemy
{
    [SerializeField] private Transform[] _movingPoints;

    private Vector2 _beforePositiion;

    private void Awake() => Init();

    protected override void Init()
    {
        base.Init();

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        for (int i = 0; i < _movingPoints.Length; i++)
            _movingPoints[i].SetParent(null);
    }

    protected override void Died() => StartCoroutine(DiedStart(_diedAnimationTime));
    private IEnumerator DiedStart(float diedAnimationTime)
    {
        _animator.SetTrigger(IsDied);
        _animator.SetBool(IsLive, false);

        yield return new WaitForSeconds(diedAnimationTime);

        gameObject.SetActive(false);
    }

    public override void Move(Transform point)
    {
        _beforePositiion = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, point.position, _movingSpeed * Time.deltaTime);
    }
    public Transform[] GetMovingPoints() => _movingPoints;
}
