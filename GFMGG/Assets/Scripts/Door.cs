using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] Transform target;

    public void Interact()
    {
        Debug.Log($"Interacted with door");
        GameManager.Instance.MovementController.Teleport(target.position);
    }
}