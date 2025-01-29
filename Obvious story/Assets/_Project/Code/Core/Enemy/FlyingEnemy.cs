using UnityEngine;

public abstract class FlyingEnemy : Enemy
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out PlayerMoving playerMoving) && _canGoThroughAPlayer)
        {
            _collider.isTrigger = true;
        }
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out PlayerMoving playerMoving) && _canGoThroughAPlayer)
        {
            _collider.isTrigger = false;
        }
    }

    public override void Move(Transform point)
    {
        transform.position = Vector2.MoveTowards(transform.position, point.position, _movingSpeed * Time.deltaTime);
    }
}
