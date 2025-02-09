using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamagable, IDestroyable
{
    public abstract event Action<IDestroyable> Destroyed;

    public abstract void TakeDamage(int damage);
}
