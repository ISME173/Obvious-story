using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
            enemy.TakeDamage(10);
    }
}
