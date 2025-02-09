using UnityEngine;

public class SkeletonAttack : StateMachineBehaviour
{
    private Skeleton _skeleton;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _skeleton = animator.GetComponent<Skeleton>();
        animator.SetBool(_skeleton.EnemyData.IsAttack, true);
        _skeleton.StopEnemyInIsIdleState();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _skeleton.FlipEnemyToTarget(_skeleton.GetPlayerTargetPoint());
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_skeleton.EnemyData.IsAttack, false);
    }
}
