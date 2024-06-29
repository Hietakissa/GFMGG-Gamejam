using UnityEngine;

public class VisualizeRadius : MonoBehaviour
{
    [SerializeField] float radius;

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}