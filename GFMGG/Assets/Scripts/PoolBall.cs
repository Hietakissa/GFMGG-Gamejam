using UnityEngine;

public class PoolBall : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude < 0.2f) return;

        if (collision.transform.CompareTag("Pool Ball") && transform.GetInstanceID() > collision.transform.GetInstanceID()) SoundManager.Instance.PlaySound(SoundType.BallHitBall);
        else if (collision.transform.CompareTag("Pool Wall")) SoundManager.Instance.PlaySound(SoundType.BallHitWall);
    }
}