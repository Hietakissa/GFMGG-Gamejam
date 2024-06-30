using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] SpriteAnimator animator;
    [SerializeField] float speed;
    [SerializeField] float distancePerStep;

    Vector2 inputVector;
    Rigidbody2D rb;
    Vector3 lastPos;
    float walkedDistance;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastPos = transform.position;
    }

    void Update()
    {
        if (GameManager.Instance.InputCapture)
        {
            inputVector = Vector2.zero;
            animator.SetStateIndex(1);
            return;
        }
        else
        {
            walkedDistance += Vector3.Distance(lastPos, transform.position);
            if (walkedDistance >= distancePerStep)
            {
                walkedDistance -= distancePerStep;
                SoundManager.Instance.PlaySound(SoundType.Walk);
            }
        }

        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (inputVector.sqrMagnitude == 0f) animator.SetStateIndex(1);
        else
        {
            animator.SetStateIndex(0);

            if (inputVector.x > 0f) animator.SetSubStateIndex(1);
            else if (inputVector.x < 0f) animator.SetSubStateIndex(0);
        }

        lastPos = transform.position;
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
            lastPos = position;
            yield return UIManager.Instance.FadeWaitCor();
            yield return UIManager.Instance.FadeOutCor();
            GameManager.Instance.SetInputCapture(false);
        }
    }
}