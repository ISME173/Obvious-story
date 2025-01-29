using UnityEngine;

public abstract class GroundEnemy : Enemy
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out PlayerMoving playerMoving) && _canGoThroughAPlayer)
        {
            _collider.isTrigger = true;
            _rigidbody2d.isKinematic = true;
        }
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out PlayerMoving playerMoving) && _canGoThroughAPlayer)
        {
            _collider.isTrigger = false;
            _rigidbody2d.isKinematic = false;
        }
    }

    public override void Move(Transform point)
    {
        if (_rigidbody2d.isKinematic == false)
        {
            float diractionRight = point.position.x - transform.position.x;
            _rigidbody2d.velocity = new Vector2(Mathf.Clamp(diractionRight, -1, 1) * _movingSpeed, _rigidbody2d.velocity.y);
        }
        else
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = new Vector3(point.position.x, currentPosition.y, currentPosition.z);
            Vector3 direction = (targetPosition - currentPosition).normalized;

            transform.position += direction * _movingSpeed * Time.deltaTime;
        }
    }
}
