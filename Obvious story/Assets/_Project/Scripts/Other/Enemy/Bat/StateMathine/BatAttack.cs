using UnityEngine;

public class BatAttack : StateMachineBehaviour
{
    private Bat _bat;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat = animator.GetComponent<Bat>();

        if (_bat.RaycastToPlayer(_bat.AttackDistance) == false)
            animator.SetBool(_bat.IsAttack, false);
        else
            animator.SetTrigger("AttackTrigger" + Random.Range(1, 3));
    }
}
