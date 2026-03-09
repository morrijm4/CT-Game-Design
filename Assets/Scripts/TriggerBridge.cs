using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ColliderEvent : UnityEvent<Collider2D> { }

public class TriggerBridge : MonoBehaviour
{
    public ColliderEvent OnEntry;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnEntry?.Invoke(other);
    }
}