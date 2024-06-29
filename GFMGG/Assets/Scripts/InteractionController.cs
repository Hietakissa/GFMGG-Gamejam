using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InteractionController : MonoBehaviour
{
    [SerializeField] float interactionRadius;
    [SerializeField] TextMeshProUGUI interactionText;

    [SerializeField] Canvas canvas;

    KeyCode[] interactionKeys = new KeyCode[] { KeyCode.E, KeyCode.F, KeyCode.Z, KeyCode.Space, KeyCode.Return };
    List<InteractablePosition> interactables = new List<InteractablePosition>();
    Collider2D[] colls = new Collider2D[10];
    Transform interactableTransform;
    Camera cam;


    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        IInteractable interactable = GetInteractable();
        if (interactable != null)
        {
            if (!interactable.IsInteractable()) return;
            if (Helpers.KeyDown(ref interactionKeys)) interactable.Interact();

            interactionText.text = interactable.GetInteractionText();

            Vector2 canvasPos = cam.WorldToViewportPoint(interactableTransform.position) * canvas.renderingDisplaySize;
            interactionText.transform.position = canvasPos;
        }
        else interactionText.text = string.Empty;
    }

    IInteractable GetInteractable()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, interactionRadius, colls);
        for (int i = 0; i < count; i++)
        {
            Collider2D coll = colls[i];
            if (coll.gameObject.TryGetComponent(out IInteractable interactable)) 
                interactables.Add(new InteractablePosition(interactable, coll.transform));
        }

        IInteractable closestInteractable = null;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < interactables.Count; i++)
        {
            InteractablePosition current = interactables[i];
            float distance = Vector2.Distance(transform.position, current.Transform.position);
            if (distance < closestDistance)
            {
                closestInteractable = current.Interactable;
                interactableTransform = current.Transform;
                closestDistance = distance;
            }
        }

        interactables.Clear();
        return closestInteractable;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}

struct InteractablePosition
{
    public readonly IInteractable Interactable;
    public readonly Transform Transform;

    public InteractablePosition(IInteractable interactable, Transform transform)
    {
        Interactable = interactable;
        Transform = transform;
    }
}