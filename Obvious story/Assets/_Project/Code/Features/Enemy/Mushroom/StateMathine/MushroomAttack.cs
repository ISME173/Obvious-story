using UnityEngine;

public class MushroomAttack : StateMachineBehaviour
{
    private Mushroom _mushroom;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _mushroom = animator.GetComponent<Mushroom>();
        animator.SetBool(_mushroom.IsAttack, true);
        _mushroom.SetMovingSpeedToZero();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _mushroom.FlipEnemyToTarget(_mushroom.GetPlayerTargetPoint());
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_mushroom.IsAttack, false);
    }
}
