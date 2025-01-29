using UnityEngine;

public class SkeletonAngry : StateMachineBehaviour
{
    private Skeleton _skeleton;
    private Transform _targetObject;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _skeleton = animator.GetComponent<Skeleton>();
        _targetObject = _skeleton.GetPlayerTargetPoint();
        _skeleton.AngryMovingSpeedActivate();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _skeleton.FlipEnemyToTarget(_targetObject);
        _skeleton.Move(_targetObject);

        if (_skeleton.RaycastToPlayer(_skeleton.AngryDistance) == false)
            animator.SetBool(_skeleton.IsAngry, false);
        else
        {
            if (_skeleton.RaycastToPlayer(_skeleton.AttackDistance))
                animator.SetTrigger(_skeleton.AttackTrigger);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _skeleton.NormalMovingSpeedActivate();
    }
}
