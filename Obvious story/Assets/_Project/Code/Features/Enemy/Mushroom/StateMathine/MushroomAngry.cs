using UnityEngine;

public class MushroomAngry : StateMachineBehaviour
{
    private Mushroom _mushroom;
    private Transform _targetObject;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _mushroom = animator.GetComponent<Mushroom>();
        _targetObject = _mushroom.GetPlayerTargetPoint();
        _mushroom.AngryMovingSpeedActivate();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _mushroom.FlipEnemyToTarget(_targetObject);
        _mushroom.Move(_targetObject);

        if (_mushroom.RaycastToPlayer(_mushroom.EnemyData.AngryDistance) == false)
            animator.SetBool(_mushroom.EnemyData.IsAngry, false);
        else
        {
            if (_mushroom.RaycastToPlayer(_mushroom.EnemyData.AttackDistance))
                animator.SetTrigger(_mushroom.EnemyData.AttackTrigger);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _mushroom.NormalMovingSpeedActivate();
    }
}
