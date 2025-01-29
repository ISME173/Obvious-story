using System;
using System.Collections;
using UnityEngine;

public class Bat : FlyingEnemy
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
}
