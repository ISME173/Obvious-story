using UnityEngine;

public abstract class FlyingEnemy<T> : Enemy<T> where T : EnemyData
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMoving playerMoving))
        {
            if (EnemyData.CanGoThroughAPlayerAndEnemy)
                _collider.isTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMoving playerMoving))
        {
            if (EnemyData.CanGoThroughAPlayerAndEnemy)
                _collider.isTrigger = false;
        }
    }

    public override void Move(Transform point)
    {
        transform.position = Vector2.MoveTowards(transform.position, point.position, _movingSpeed * Time.deltaTime);
    }
}
