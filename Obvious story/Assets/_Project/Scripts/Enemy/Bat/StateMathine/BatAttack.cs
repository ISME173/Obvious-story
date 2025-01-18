using UnityEngine;

public class BatAttack : StateMachineBehaviour
{
    private Bat _bat;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat = animator.GetComponent<Bat>();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat.FlipEnemyToTarget(_bat.PlayerMoving.TargetPoint);

        if (_bat.RaycastToPlayer(_bat.AttackDistance) == false)
            animator.SetBool(_bat.IsAttack, false);
        else
            animator.SetTrigger("AttackTrigger" + Random.Range(1, 3));
    }
}
