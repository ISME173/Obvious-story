using System;
using System.Collections;
using UnityEngine;

public class Skeleton : GroundEnemy<SkeletonData>
{
    private IEnumerator DiedStart(float diedAnimationTime, Action died)
    {
        _animator.SetTrigger(EnemyData.DiedTrigger);
        _animator.SetBool(EnemyData.IsLive, false);
        _rigidbody2d.isKinematic = true;
        _collider.isTrigger = true;

        StopEnemyInIsIdleState();

        yield return new WaitForSeconds(diedAnimationTime);

        died();
    }
    protected override void Died()
    {
        StartCoroutine(DiedStart(EnemyData.DiedAnimationTime, base.Died));
    }
    protected override void AddAttackListenersToOnDamagebleCollideCheckWithEnemy()
    {
        OnDamagableCollideCheckWithSkeleton[] onDamagableCollideCheckWithSkeletons = GetComponentsInChildren<OnDamagableCollideCheckWithSkeleton>();
        for (int i = 0; i < onDamagableCollideCheckWithSkeletons.Length; i++)
            onDamagableCollideCheckWithSkeletons[i].OnDamagebleColliderEnter += Attack;
    }
    public override void FlipEnemyToTarget(Transform target)
    {
        transform.rotation = transform.position.x < target.position.x ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }
}
