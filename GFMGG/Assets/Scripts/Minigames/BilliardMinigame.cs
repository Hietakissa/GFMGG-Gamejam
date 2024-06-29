using System.Collections;
using HietakissaUtils;
using UnityEngine;

public class BilliardMinigame : Minigame
{
    MinigameManager manager;
    bool running;

    Camera cam;

    const int CONST_MAX_STROKES = 20;
    float cueTurnSpeed = 7f;
    float hitForce = 250f;
    float ballStopThreshold = 0.015f;
    float woundUpForce;

    float pocketSuckThreshold = 0.275f;
    float pocketSunkThreshold = 0.115f;

    Rigidbody2D[] allBallRBs = new Rigidbody2D[16];
    Rigidbody2D[] valueBallRBs = new Rigidbody2D[15];
    Vector3[] valueBallStartPositions = new Vector3[15];
    Rigidbody2D cueBallRB;
    Vector3 cueBallStartPosition;

    bool aiming;
    bool winding;

    float lastShot;
    int ballsSunk;
    int strokes;

    public override IEnumerator StartCor(MinigameManager manager)
    {
        this.manager = manager;
        GameManager.Instance.SetInputCapture(true);

        if (cueBallRB == null)
        {
            cam = Camera.main;

            cueBallRB = manager.BilliardGameCueBall.GetComponent<Rigidbody2D>();
            cueBallStartPosition = manager.BilliardGameCueBall.position;
            allBallRBs[0] = cueBallRB;

            for (int i = 0; i < manager.BilliardGameValueBalls.Length; i++)
            {
                valueBallRBs[i] = manager.BilliardGameValueBalls[i].GetComponent<Rigidbody2D>();
                valueBallStartPositions[i] = manager.BilliardGameValueBalls[i].position;
                allBallRBs[i + 1] = valueBallRBs[i];
            }
        }

        aiming = true;
        ballsSunk = 0;
        strokes = 0;

        yield return UIManager.Instance.FadeInCor();
        manager.BilliardGameHolder.SetActive(true);
        RestoreBalls();
        GameManager.Instance.CameraController.SetOverrideTarget(MinigameManager.Instance.BilliardGameCameraOverride);
        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
        running = true;
    }

    public override IEnumerator EndCor(MinigameEndType minigameEndType)
    {
        yield return UIManager.Instance.FadeInCor();

        manager.BilliardGameHolder.SetActive(false);
        GameManager.Instance.CameraController.SetOverrideTarget(null);
        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
        GameManager.Instance.SetInputCapture(false);
    }

    public override void Update()
    {
        if (!running || AreBallsMoving() || Time.time - lastShot < 1f) return;

        if (strokes >= CONST_MAX_STROKES)
        {
            Stop(MinigameEndType.Lose);
            return;
        }

        if (aiming)
        {
            manager.BilliardGameCueMove.gameObject.SetActive(true);

            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dirToMouse = (cueBallRB.position - mousePos).normalized;
            float angle = Mathf.Atan2(dirToMouse.x, -dirToMouse.y) * Mathf.Rad2Deg;
            Quaternion rotToMouse = Quaternion.AngleAxis(angle, Vector3.forward);

            manager.BilliardGameCueRotate.rotation = Quaternion.Slerp(manager.BilliardGameCueRotate.rotation, rotToMouse, cueTurnSpeed * Time.deltaTime);

            if (Input.GetMouseButtonDown(0))
            {
                aiming = false;
                winding = true;
                woundUpForce = 0f;
            }
        }
        else if (winding)
        {
            if (woundUpForce < 3f) woundUpForce += Time.deltaTime;
            else if (woundUpForce > 3f) woundUpForce = 3f;

            manager.BilliardGameCueMove.localPosition = Vector3.down * woundUpForce * 0.25f;

            if (Input.GetMouseButtonDown(0))
            {
                aiming = true;
                winding = false;
                manager.BilliardGameCueMove.localPosition = Vector3.zero;
                Vector2 force = manager.BilliardGameCueRotate.up * woundUpForce * hitForce;
                cueBallRB.AddForce(force);
                manager.BilliardGameCueMove.gameObject.SetActive(false);

                strokes++;
                lastShot = Time.time;
            }
        }
    }
    public override void FixedUpdate()
    {
        if (!running) return;

        for (int i = 0; i < allBallRBs.Length; i++)
        {
            Rigidbody2D ballRB = allBallRBs[i];
            if (!ballRB.gameObject.activeSelf) continue;

            for (int p = 0; p < manager.BilliardGamePockets.Length; p++)
            {
                Transform pocket = manager.BilliardGamePockets[p];
                if (Vector2.Distance(ballRB.position, pocket.position) <= pocketSunkThreshold)
                {
                    if (ballRB == cueBallRB)
                    {
                        ballRB.gameObject.SetActive(false);
                        Stop(MinigameEndType.Lose);
                    }
                    else
                    {
                        ballRB.gameObject.SetActive(false);
                        ballsSunk++;

                        if (ballsSunk >= 15) Stop(MinigameEndType.Win);
                    }
                }
                else if (Vector2.Distance(ballRB.position, pocket.position) <= pocketSuckThreshold)
                {
                    Vector2 dirToPocket = (Vector2)pocket.position - ballRB.position;
                    ballRB.AddForce(dirToPocket * 3.5f);
                }
            }
        }
    }

    void Stop(MinigameEndType minigameEndType)
    {
        running = false;
        manager.StopMinigame(minigameEndType);
    }


    void RestoreBalls()
    {
        cueBallRB.velocity = Vector3.zero;
        cueBallRB.angularVelocity = 0f;
        cueBallRB.position = cueBallStartPosition;
        cueBallRB.transform.position = cueBallStartPosition;
        cueBallRB.rotation = 0f;
        cueBallRB.gameObject.SetActive(true);

        for (int i = 0; i < valueBallRBs.Length; i++)
        {
            Rigidbody2D rb = valueBallRBs[i];
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
            rb.position = valueBallStartPositions[i];
            rb.rotation = 0f;
            rb.gameObject.SetActive(true);
        }
    }

    bool AreBallsMoving()
    {
        for (int i = 0; i < allBallRBs.Length; i++)
        {
            Rigidbody2D rb = allBallRBs[i];
            if (rb.velocity.magnitude > ballStopThreshold) return true;
            else
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
        return false;
    }
}