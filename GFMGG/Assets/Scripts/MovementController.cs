using System.Collections;

using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] float speed;

    Vector2 inputVector;
    Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.Instance.InputCapture)
        {
            inputVector = Vector2.zero;
            return;
        }

        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        Vector2 moveVector = inputVector.normalized * speed * 50 * Time.fixedDeltaTime;
        rb.velocity = moveVector;
    }


    public void Teleport(Vector3 position)
    {
        StartCoroutine(TeleportCor());

        IEnumerator TeleportCor()
        {
            GameManager.Instance.SetInputCapture(true);
            yield return UIManager.Instance.FadeInCor();
            rb.position = position;
            yield return UIManager.Instance.FadeWaitCor();
            yield return UIManager.Instance.FadeOutCor();
            GameManager.Instance.SetInputCapture(false);
        }
    }
}