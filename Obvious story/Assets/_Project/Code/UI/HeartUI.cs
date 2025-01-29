using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HeartUI : MonoBehaviour
{
    [Header("Animator states")]
    [SerializeField] private string _disableTrigger;
    [SerializeField] private string _enableTrigger;

    [Header("Animation states")]
    [SerializeField] private float _disableAnimationLeght;
    [SerializeField] private float _enableAnimationLeght;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        if (_disableTrigger == string.Empty || _enableTrigger == string.Empty)
            throw new ArgumentException($"{nameof(_disableTrigger)} or {nameof(_enableTrigger)} is empty");
    }

    public float Disable()
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(_disableTrigger);
        return _disableAnimationLeght;
    }
    public float Enable()
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(_enableTrigger);
        return _enableAnimationLeght;
    }
}
