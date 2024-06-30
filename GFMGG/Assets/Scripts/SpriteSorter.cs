using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [SerializeField] SpriteRenderer targetSpriteRenderer;
    [SerializeField] int aboveOrder;
    [SerializeField] int belowOrder;
    [SerializeField] SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.sortingOrder = transform.position.y > targetSpriteRenderer.transform.position.y ? aboveOrder : belowOrder;
    }
}