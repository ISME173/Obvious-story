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
        _playerIsGroundTrigger.OnGroundEnter += () =>
        {
            if (_playerInAirCoroutine != null)
            {
                StopCoroutine(_playerInAirCoroutine);
                _playerInAirCoroutine = null;
            }
        };
        _playerIsGroundTrigger.OnGroundExit += () =>
        {
            if (gameObject.activeSelf)
                _playerInAirCoroutine = StartCoroutine(CheckIsFall());
        };
    }
    private IEnumerator CheckIsFall()
    {
        if (_playerMoving.Rigidbody2D.velocity.y >= 0)
        {
            while (_playerMoving.Rigidbody2D.velocity.y >= 0)
                yield return null;

            OnPlayerFall?.Invoke();
        }
        else if(_playerMoving.Rigidbody2D.velocity.y < 0)
        {
            OnPlayerFall?.Invoke();
        }
    }
}
