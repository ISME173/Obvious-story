using System;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IDamagable
{
    [SerializeField, Min(1)] private int _fullHeartsCount = 5;

    private int _heartsCount;

    public event Action<int> HeartsCountChanged;
    public event Action PlayerDied;

    public int HeartsCount => _heartsCount;

    private void Start() => _heartsCount = _fullHeartsCount;

    private void Died() => PlayerDied?.Invoke();
    public void TakeDamage(int damage)
    {
        if (_heartsCount <= 0)
            return;

        _heartsCount = Mathf.Clamp(_heartsCount - damage, 0, _fullHeartsCount);
        HeartsCountChanged?.Invoke(HeartsCount);
        Debug.Log($"Player take damage! Hearts count: {HeartsCount}");

        if (_heartsCount <= 0)
            Died();
    }
}
