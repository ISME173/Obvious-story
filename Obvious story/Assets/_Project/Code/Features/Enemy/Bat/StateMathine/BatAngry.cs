using UnityEngine;

public class BatAngry : StateMachineBehaviour
{
    private Bat _bat;
    private Transform _targetObject;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat = animator.GetComponent<Bat>();
        _targetObject = _bat.GetPlayerTargetPoint();
        _bat.AngryMovingSpeedActivate();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat.FlipEnemyToTarget(_targetObject);
        _bat.Move(_targetObject);

        if (_bat.RaycastToPlayer(_bat.EnemyData.AngryDistance) == false)
            animator.SetBool(_bat.EnemyData.IsAngry, false);

        else
        {
            if (_bat.RaycastToPlayer(_bat.EnemyData.AttackDistance))
                animator.SetBool(_bat.EnemyData.IsAttack, true);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat.NormalMovingSpeedActivate();
    }
}
