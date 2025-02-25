using UnityEngine;

public class KristalGolemAngry : StateMachineBehaviour
{
    private KristalGolem _kristalGolem;
    private Transform _targetObject;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _kristalGolem = animator.GetComponent<KristalGolem>();
        _targetObject = _kristalGolem.GetPlayerTargetPoint();
        _kristalGolem.AngryMovingSpeedActivate();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _kristalGolem.FlipEnemyToTarget(_targetObject);
        _kristalGolem.Move(_targetObject);

        if (_kristalGolem.RaycastToPlayer(_kristalGolem.EnemyData.AngryDistance) == false)
            animator.SetBool(_kristalGolem.EnemyData.IsAngry, false);
        else
        {
            if (_kristalGolem.RaycastToPlayer(_kristalGolem.EnemyData.AttackDistance))
                animator.SetTrigger(_kristalGolem.EnemyData.AttackTrigger);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _kristalGolem.NormalMovingSpeedActivate();
    }
}
