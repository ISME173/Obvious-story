using UnityEngine;

[System.Serializable]
public struct PlayerAttackType
{
    [SerializeField, Min(1)] private int _animationNumber;
    [SerializeField, Min(0)] private float _animationTime;
    [SerializeField, Min(1)] private int _attackDamage;

    public int AnimationNumber => _animationNumber;
    public float AnimationTime => _animationTime;
    public int AttackDamage => _attackDamage;
}
