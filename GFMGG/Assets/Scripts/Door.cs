using UnityEngine.Events;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] UnityEvent onEnter;
    [SerializeField] Transform target;

    public string GetInteractionText() => $"Enter";
    public void Interact()
    {
        onEnter?.Invoke();
        GameManager.Instance.MovementController.Teleport(target.position);
    }
}