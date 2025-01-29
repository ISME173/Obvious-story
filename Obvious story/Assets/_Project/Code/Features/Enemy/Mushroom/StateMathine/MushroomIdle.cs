using UnityEngine;

public class MushroomIdle : StateMachineBehaviour
{
    private Mushroom _mushroom;
    private float _time;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _mushroom = animator.GetComponent<Mushroom>();
        _mushroom.SetMovingSpeedToZero();
        _time = 0;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_mushroom.RaycastToPlayer(_mushroom.AngryDistance))
        {
            animator.SetBool(_mushroom.IsAngry, true);
        }
        else
        {
            _time += Time.deltaTime;
            if (_time >= _mushroom.IdleTime)
                animator.SetBool(_mushroom.IsIdle, false);
        }
    }
}
