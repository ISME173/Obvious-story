using UnityEngine;

public class SkeletonIdle : StateMachineBehaviour
{
    private Skeleton _skeleton;
    private float _time;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _skeleton = animator.GetComponent<Skeleton>();
        _skeleton.StopEnemyInIsIdleState();
        _time = 0;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_skeleton.RaycastToPlayer(_skeleton.EnemyData.AngryDistance))
        {
            animator.SetBool(_skeleton.EnemyData.IsAngry, true);
        }
        else
        {
            _time += Time.deltaTime;
            if (_time >= _skeleton.IdleTime)
                animator.SetBool(_skeleton.EnemyData.IsIdle, false);
        }
    }
}
