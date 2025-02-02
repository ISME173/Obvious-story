using UnityEngine;

public abstract class FlyingEnemy : Enemy
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out PlayerMoving playerMoving) && _canGoThroughAPlayerAndEnemy)
        {
            _collider.isTrigger = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out PlayerMoving playerMoving) && _canGoThroughAPlayerAndEnemy)
        {
            _collider.isTrigger = true;
        }
    }
    public override void Move(Transform point)
    {
        transform.position = Vector2.MoveTowards(transform.position, point.position, _movingSpeed * Time.deltaTime);
    }
}
