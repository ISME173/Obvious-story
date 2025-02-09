using UnityEngine;

public abstract class GroundEnemy<T> : Enemy<T> where T : EnemyData
{
    public override void Move(Transform point)
    {
        float diractionRight = point.position.x - transform.position.x;
        _rigidbody2d.velocity = new Vector2(Mathf.Clamp(diractionRight, -1, 1) * _movingSpeed, _rigidbody2d.velocity.y);
    }
}
