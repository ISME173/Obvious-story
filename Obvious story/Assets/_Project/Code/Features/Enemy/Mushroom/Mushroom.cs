using System.Collections;
using UnityEngine;
using System;

public class Mushroom : GroundEnemy
{
    private IEnumerator DiedStart(float diedAnimationTime, Action died)
    {
        _animator.SetTrigger(DiedTrigger);
        _animator.SetBool(IsLive, false);
        SetMovingSpeedToZero();

        yield return new WaitForSeconds(diedAnimationTime);

        died();
    }
    protected override void Died()
    {
        StartCoroutine(DiedStart(_diedAnimationTime, base.Died));
    }
}
