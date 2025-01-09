using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerFallCheker : MonoBehaviour
{
    [SerializeField] private PlayerIsGroundTrigger _playerIsGroundTrigger;

    [Inject] private PlayerMoving _playerMoving;
    private Coroutine _playerInAirCoroutine;

    public event Action OnPlayerFall;

    private void Awake()
    {
        if (_playerIsGroundTrigger == null)
            _playerIsGroundTrigger = FindAnyObjectByType<PlayerIsGroundTrigger>();
    }

    private void Start()
    {
        _playerIsGroundTrigger.OnGroundExit += () =>
        {
            _playerInAirCoroutine = StartCoroutine(CheckIsFall());
        };

        _playerIsGroundTrigger.OnGroundEnter += () =>
        {
            if (_playerInAirCoroutine != null)
                StopCoroutine(_playerInAirCoroutine);
            _playerInAirCoroutine = null;
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
            OnPlayerFall?.Invoke();
        }
        else
        {
            OnPlayerFall?.Invoke();
        }
    }
}
