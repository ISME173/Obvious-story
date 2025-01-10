using UnityEngine;

public class BatIdle : StateMachineBehaviour
{
    private Bat _bat;
    private float _time;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat = animator.GetComponent<Bat>();
        _time = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _time += Time.deltaTime;
        if (_time >= _bat.IdleTime)
            animator.SetBool(_bat.IsIdle, false);


    }
}
