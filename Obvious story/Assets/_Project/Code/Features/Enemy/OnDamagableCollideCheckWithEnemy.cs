using UnityEngine;

public class OnDamagableCollideCheckWithEnemy<EnemyType, Data> : OnDamagableCollideCheck<EnemyType> where EnemyType : Enemy<Data> where Data : EnemyData
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.TryGetComponent(out IDamagable damagable)))
        {
            if (_ignoreDamagable.EnemyData.CanTakeDamageAnyEnemy == false)
            {
                if (damagable as EnemyBase == false)
                {
                    base.OnTriggerEnter2D(collision);
                }
            }
            else
            {
                base.OnTriggerEnter2D(collision);
            }
        }
    }
}
