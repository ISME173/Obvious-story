using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private string _activateAnimatorTriggerName;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(_activateAnimatorTriggerName);
    }
}
