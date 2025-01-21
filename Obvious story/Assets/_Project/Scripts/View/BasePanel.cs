using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class BasePanel : MonoBehaviour
{
    [Header("Animator states")]
    [SerializeField] protected string _disableParameter;
    [SerializeField] protected string _enableParameter;

    protected Animator _animator;

    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();

        if (_disableParameter == string.Empty || _enableParameter == string.Empty)
            throw new ArgumentException($"{nameof(_disableParameter)} or {nameof(_enableParameter)} is emptys");
    }

    public virtual void Disable()
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(_disableParameter);
    }
    public virtual void Enable()
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(_enableParameter);
    }
}
