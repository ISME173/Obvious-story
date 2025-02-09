using UnityEngine;

public class BatIsIdle : StateMachineBehaviour
{
    private Bat _bat;
    private float _time;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat = animator.GetComponent<Bat>();
        _time = 0;

        animator.SetBool(_bat.EnemyData.IsAttack, false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_bat.RaycastToPlayer(_bat.EnemyData.AngryDistance))
        {
            animator.SetBool(_bat.EnemyData.IsAngry, true);
        }
        else
        {
            _time += Time.deltaTime;
            if (_time >= _bat.IdleTime)
                animator.SetBool(_bat.EnemyData.IsIdle, false);
        }
    }
}
