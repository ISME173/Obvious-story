using System;
using System.Collections;
using UnityEngine;

public class Skeleton : GroundEnemy
{
    private IEnumerator DiedStart(float diedAnimationTime, Action died)
    {
        _animator.SetTrigger(DiedTrigger);
        _animator.SetBool(IsLive, false);

        yield return new WaitForSeconds(diedAnimationTime);

        died();
    }
    protected override void Died()
    {
        StartCoroutine(DiedStart(_diedAnimationTime, base.Died));
    }
    public override void FlipEnemyToTarget(Transform target)
    {
        transform.rotation = transform.position.x < target.position.x ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }
}
