using System;
using System.Collections;
using UnityEngine;

public class Bat : FlyingEnemy<BatData>
{
    private IEnumerator DiedStart(float diedAnimationTime, Action died)
    {
        _rigidbody2d.isKinematic = true;
        _animator.SetTrigger(EnemyData.DiedTrigger);
        _animator.SetBool(EnemyData.IsLive, false);

        yield return new WaitForSeconds(diedAnimationTime);

        died();
    }
    protected override void AddAttackListenersToOnDamagebleCollideCheckWithEnemy()
    {
        OnDamagableCollideCheckWithBat[] onDamagableCollideCheckWithBats = GetComponentsInChildren<OnDamagableCollideCheckWithBat>();
        for (int i = 0; i < onDamagableCollideCheckWithBats.Length; i++)
            onDamagableCollideCheckWithBats[i].OnDamagebleColliderEnter += Attack;
    }
    protected override void Died()
    {
        StartCoroutine(DiedStart(EnemyData.DiedAnimationTime, base.Died));
    }
}
