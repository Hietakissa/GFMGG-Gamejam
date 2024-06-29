using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    Transform overrideTarget;

    void LateUpdate()
    {
        transform.position = (overrideTarget == null ? target.position : overrideTarget.position) - Vector3.forward * 10;
        //transform.position = target.position - Vector3.forward * 10;
    }

    public void SetOverrideTarget(Transform overrideTarget) => this.overrideTarget = overrideTarget;
}