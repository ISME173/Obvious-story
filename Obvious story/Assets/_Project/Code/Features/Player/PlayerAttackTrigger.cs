using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    private IDamagable _playerDamagable;
    private PlayerAttack _playerAttack;

    private void Awake()
    {
        _playerDamagable = GetComponentInParent<IDamagable>();
        _playerAttack = GetComponentInParent<PlayerAttack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable damagable))
        {
            if (damagable != _playerDamagable)
                damagable.TakeDamage(_playerAttack.LastAttackDamage);
        }
    }
}
