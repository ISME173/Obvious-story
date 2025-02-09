using System.Collections;
using UnityEngine;
using System;

public class Mushroom : GroundEnemy<MushroomData>
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
    protected override void AddAttackListenersToOnDamagebleCollideCheckWithEnemy()
    {
        OnDamagableCollideCheckWithMushroom[] onDamagableCollideCheckWithMushrooms = GetComponentsInChildren<OnDamagableCollideCheckWithMushroom>();
        for (int i = 0; i < onDamagableCollideCheckWithMushrooms.Length; i++)
            onDamagableCollideCheckWithMushrooms[i].OnDamagebleColliderEnter += Attack;
    }
    protected override void Died()
    {
        StartCoroutine(DiedStart(EnemyData.DiedAnimationTime, base.Died));
    }
}
