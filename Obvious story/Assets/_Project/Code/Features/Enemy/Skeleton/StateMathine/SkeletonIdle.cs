using UnityEngine;

public class SkeletonIdle : StateMachineBehaviour
{
    private Skeleton _skeleton;
    private float _time;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _skeleton = animator.GetComponent<Skeleton>();
        _skeleton.SetMovingSpeedToZero();
        _time = 0;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_skeleton.RaycastToPlayer(_skeleton.AngryDistance))
        {
            animator.SetBool(_skeleton.IsAngry, true);
        }
        else
        {
            _time += Time.deltaTime;
            if (_time >= _skeleton.IdleTime)
                animator.SetBool(_skeleton.IsIdle, false);
        }
    }
}
