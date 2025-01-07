using UnityEngine;

public class MainPlayerIdle : StateMachineBehaviour
{
    private PlayerMoving _playerMoving;
    private PlayerAnimatorStates _playerAnimatorStates;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMoving = animator.GetComponent<PlayerMoving>();
        _playerAnimatorStates = animator.GetComponent<PlayerAnimatorStates>();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerMoving.MovingVelosity.x != 0)
            animator.SetBool(_playerAnimatorStates.Idle, false);
    }
}
