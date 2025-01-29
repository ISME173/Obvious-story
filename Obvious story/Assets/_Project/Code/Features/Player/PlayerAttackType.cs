using UnityEngine;

[System.Serializable]
public struct PlayerAttackType
{
    [SerializeField, Min(1)] private string _animationName;
    [SerializeField, Min(0)] private float _animationTime;
    [SerializeField, Min(1)] private int _attackDamage;

    public string AnimationName => _animationName;
    public float AnimationTime => _animationTime;
    public int AttackDamage => _attackDamage;
}
