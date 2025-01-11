using UnityEngine;

public class BatIdle : StateMachineBehaviour
{
    private Bat _bat;
    private float _time;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat = animator.GetComponent<Bat>();
        _time = 0;

        animator.SetBool(_bat.IsAttack, false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_bat.RaycastToPlayer(_bat.AngryDistance))
        {
            if (_bat.RaycastToPlayer(_bat.AttackDistance))
                animator.SetBool(_bat.IsAttack, true);
            else
                animator.SetBool(_bat.IsAngry, true);
        }
        else
        {
            _time += Time.deltaTime;
            if (_time >= _bat.IdleTime)
                animator.SetBool(_bat.IsIdle, false);
        }
    }
}
