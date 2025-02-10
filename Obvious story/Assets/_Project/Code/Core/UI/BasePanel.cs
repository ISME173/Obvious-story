using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class BasePanel : MonoBehaviour
{
    [Header("Animator states")]
    [SerializeField] protected string _disableTrigger = "Disable";
    [SerializeField] protected string _enableParameter = "Enable";

    [Header("Animation states"), Space]
    [SerializeField, Min(0)] private float _enableAnimTime;
    [SerializeField, Min(0)] private float _disableAnimTime;

    protected Animator _animator;

    public bool IsEnable { get; private set; } = false;
    public float EnableAnimTime => _enableAnimTime;
    public float DisableAnimTime => _disableAnimTime;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();

        if (_disableTrigger == string.Empty || _enableParameter == string.Empty)
            throw new ArgumentException($"{nameof(_disableTrigger)} or {nameof(_enableParameter)} is emptys");
    }

    public virtual void Disable()
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(_disableTrigger);
        IsEnable = false;
    }
    public virtual void Enable()
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(_enableParameter);
        IsEnable = true;
    }
}
