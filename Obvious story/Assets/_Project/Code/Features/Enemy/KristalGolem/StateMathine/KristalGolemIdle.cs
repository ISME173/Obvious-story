using UnityEngine;

public class KristalGolemIdle : StateMachineBehaviour
{
    private KristalGolem _kristalGolem;
    private float _time;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _kristalGolem = animator.GetComponent<KristalGolem>();
        _kristalGolem.SetMovingSpeedToZero();
        _time = 0;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_kristalGolem.RaycastToPlayer(_kristalGolem.AngryDistance))
        {
            animator.SetBool(_kristalGolem.IsAngry, true);
        }
        else
        {
            _time += Time.deltaTime;
            if (_time >= _kristalGolem.IdleTime)
                animator.SetBool(_kristalGolem.IsIdle, false);
        }
    }
}
