using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthManager : MonoBehaviour, IDamagable
{
    [SerializeField, Min(1)] private int _maxHeartsCount = 5;

    private int _heartsCount;

    public event Action<int> DamageTaken;
    public  UnityEvent PlayerDied {  get; private set; } = new UnityEvent();
    public event Action<int> HealingHearts;

    public int HeartsCount => _heartsCount;
    public int FullHeartsCount => _maxHeartsCount;

    private void Awake() => _heartsCount = _maxHeartsCount;

    private void Update()
    {
        // ��� ��� �������������� ��������. ������ ��� ����� ������� ������������. ��� �������������, ����� ������������� � ����.
        /*if (Input.GetKeyDown(KeyCode.F))
            Healing(1); */
    }
    private void Died()
    {
        PlayerDied?.Invoke();
    }
    public void TakeDamage(int damage)
    {
        if (_heartsCount <= 0 || GameEvents.Instance.IsGameStarting == false)
            return;

        _heartsCount = Mathf.Clamp(_heartsCount - Math.Abs(damage), 0, _maxHeartsCount);
        DamageTaken?.Invoke(damage);

        if (_heartsCount <= 0)
            Died();
    }
    private void Healing(int HealingHeartsCount)
    {
        _heartsCount = Mathf.Clamp(_heartsCount + HealingHeartsCount, 0, _maxHeartsCount);
        HealingHearts?.Invoke(HeartsCount);
    }
}
