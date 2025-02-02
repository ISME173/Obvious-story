using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class FinishInLevelTrigger : MonoBehaviour
{
    private Collider2D _triggerZone;

    [HideInInspector] public UnityEvent OnPlayerFinishTriggerEnter, OnPlayerFinishTriggerExit;

    private void Awake()
    {
        _triggerZone = GetComponent<Collider2D>();
        _triggerZone.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMoving playerMoving))
        {
            OnPlayerFinishTriggerEnter?.Invoke();
            //Debug.Log("PlayerFinishEnter");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMoving playerMoving))
        {
            OnPlayerFinishTriggerExit?.Invoke();
            //Debug.Log("PlayerFinishExit");
        }
    }
}
