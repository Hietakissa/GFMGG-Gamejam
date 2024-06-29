using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [SerializeField] SpriteRenderer targetSpriteRenderer;
    [SerializeField] int aboveOrder;
    [SerializeField] int belowOrder;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.sortingOrder = transform.position.y > targetSpriteRenderer.transform.position.y ? aboveOrder : belowOrder;
    }
}