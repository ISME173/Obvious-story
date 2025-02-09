using UnityEngine;

public class KristalGolemAttack : StateMachineBehaviour
{
    private KristalGolem _kristalGolem;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _kristalGolem = animator.GetComponent<KristalGolem>();
        animator.SetBool(_kristalGolem.EnemyData.IsAttack, true);
        _kristalGolem.StopEnemyInIsIdleState();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _kristalGolem.FlipEnemyToTarget(_kristalGolem.GetPlayerTargetPoint());
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_kristalGolem.EnemyData.IsAttack, false);
    }
}
