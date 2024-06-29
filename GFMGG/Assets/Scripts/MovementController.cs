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
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        //Vector2 localInputVector = transform.TransformDirection(inputVector);
        Vector2 moveVector = inputVector.normalized * speed * 50 * Time.fixedDeltaTime;
        rb.velocity = moveVector;
    }
}