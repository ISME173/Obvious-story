using UnityEngine;
using Zenject;

public class PlayerIdle : StateMachineBehaviour
{
    [Inject] private IUserInput _userInput;
    private PlayerMoving _playerMoving;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerMoving = animator.GetComponent<PlayerMoving>();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_userInput.GetPlayerMovingHorizontalInput(_playerMoving.Speed) != 0)
        {
            animator.SetBool("IsIdle", false);
        }
    }
}