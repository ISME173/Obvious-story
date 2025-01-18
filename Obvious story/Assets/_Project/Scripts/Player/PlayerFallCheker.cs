using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerFallCheker : MonoBehaviour
{
    [SerializeField] private PlayerIsGroundTrigger _playerIsGroundTrigger;

    [Inject] private PlayerMoving _playerMoving;
    private Coroutine _playerInAirCoroutine;
    private Animator _animator;

    public event Action<bool> OnPlayerFall;

    private void Awake()
    {
        if (_playerIsGroundTrigger == null)
            _playerIsGroundTrigger = FindAnyObjectByType<PlayerIsGroundTrigger>();

        if (TryGetComponent(out Animator animator))
            _animator = animator;
        else
            _animator = FindAnyObjectByType<Animator>();
    }

    private void OnEnable()
    {
        _playerIsGroundTrigger.OnGroundExit += () =>
        {
            if (gameObject.activeSelf)
                _playerInAirCoroutine = StartCoroutine(CheckIsFall());
        };
    }
    private void Start()
    {
        _playerIsGroundTrigger.OnGroundEnter += () =>
        {
            if (_playerInAirCoroutine != null)
            {
                StopCoroutine(_playerInAirCoroutine);
                _playerInAirCoroutine = null;
            }
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("MainPlayerJump"))
            {
                OnPlayerFall?.Invoke(false);
            }
        };

        _playerIsGroundTrigger.OnGroundStay += () =>
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("MainPlayerJump"))
            {
                OnPlayerFall?.Invoke(false);
            }
        };
    }
    private IEnumerator CheckIsFall()
    {
        if (_playerMoving.Rigidbody2D.velocity.y >= 0)
        {
            while (_playerMoving.Rigidbody2D.velocity.y >= 0)
            {
                yield return null;
                continue;
            }
            OnPlayerFall?.Invoke(true);
        }
    }
}
