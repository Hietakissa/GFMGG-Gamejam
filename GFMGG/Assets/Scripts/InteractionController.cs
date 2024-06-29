using UnityEngine;
using TMPro;

public class InteractionController : MonoBehaviour
{
    [SerializeField] float interactionRadius;
    [SerializeField] TextMeshProUGUI interactionText;

    KeyCode[] interactionKeys = new KeyCode[] { KeyCode.E, KeyCode.Z };
    Collider2D[] colls = new Collider2D[10];
    Camera cam;

    [SerializeField] Canvas canvas;


    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        bool wasOnInteractable = false;
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, interactionRadius, colls);
        for (int i = 0; i < count; i++)
        {
            Collider2D coll = colls[i];
            if (!coll.gameObject.TryGetComponent(out IInteractable interactable)) break;
            if (!interactable.IsInteractable()) break;
            wasOnInteractable = true;

            Vector2 canvasPos = cam.WorldToViewportPoint(coll.transform.position) * canvas.renderingDisplaySize;
            //interactionText.transform.position = coll.transform.position;
            interactionText.transform.position = canvasPos;

            interactionText.text = interactable.GetInteractionText();

            if (Helpers.KeyDown(ref interactionKeys)) interactable.Interact();
            break;
        }

        if (!wasOnInteractable) interactionText.text = string.Empty;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}