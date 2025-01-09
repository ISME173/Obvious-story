using System;
using UnityEngine;

public class PlayerIsGroundTrigger : MonoBehaviour
{
    [SerializeField] private string _sortingTriggersToLayer;

    public event Action OnGroundEnter, OnGroundExit;

    public bool IsGround { get; private set; } = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(_sortingTriggersToLayer))
        {
            IsGround = true;
            OnGroundEnter?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(_sortingTriggersToLayer))
        {
            IsGround = false;
            OnGroundExit?.Invoke();
        }
    }
}
