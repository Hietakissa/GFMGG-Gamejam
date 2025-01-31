using System.Collections.Generic;
using System.Collections;
using HietakissaUtils;
using UnityEngine;

public class BilliardMinigame : Minigame
{
    MinigameManager manager;
    bool running;

    Camera cam;

    const int CONST_MAX_STROKES = 30;
    float cueTurnSpeed = 9f;
    float hitForce = 250f;
    float ballStopThreshold = 0.015f;
    float woundUpForce;

    float pocketSuckThreshold = 0.2f;
    float pocketSunkThreshold = 0.12f;

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

    List<Rigidbody2D> pocketingBalls = new List<Rigidbody2D>();


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
        manager.BilliardGameShotsText.text = $"{strokes}\n---\n{CONST_MAX_STROKES}";

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
        if (!running || AreBallsMoving() || Time.time - lastShot < 1f || pocketingBalls.Count > 0) return;

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

                manager.BilliardGameShotsText.text = $"{strokes}\n---\n{CONST_MAX_STROKES}";

                SoundManager.Instance.PlaySound(SoundType.BallHit);
            }
        }
    }
    public override void FixedUpdate()
    {
        if (!running) return;

        for (int i = 0; i < allBallRBs.Length; i++)
        {
            Rigidbody2D ballRB = allBallRBs[i];
            if (!ballRB.gameObject.activeSelf || pocketingBalls.Contains(ballRB)) continue;

            for (int p = 0; p < manager.BilliardGamePockets.Length; p++)
            {
                Transform pocket = manager.BilliardGamePockets[p];
                if (Vector2.Distance(ballRB.position, pocket.position) - 0.07f <= pocketSunkThreshold)
                {
                    //if (ballRB == cueBallRB)
                    //{
                    //    ballRB.gameObject.SetActive(false);
                    //    Stop(MinigameEndType.Lose);
                    //}
                    //else
                    //{
                    //    PocketBall(ballRB);
                    //}
                    PocketBall(ballRB);
                }
                else if (Vector2.Distance(ballRB.position, pocket.position) - 0.07f <= pocketSuckThreshold)
                {

                    Vector2 dirToPocket = (Vector2)pocket.position - ballRB.position;
                    Debug.Log($"In suck threshold. Force vector: {dirToPocket}");
                    ballRB.AddForce(dirToPocket * 6.5f);
                }
            }
        }
    }

    void PocketBall(Rigidbody2D ballRB)
    {
        manager.StartCoroutine(PocketBallCor());

        IEnumerator PocketBallCor()
        {
            ballRB.velocity = ballRB.velocity * 0.03f;

            pocketingBalls.Add(ballRB);
            TrailRenderer trail = ballRB.GetComponent<TrailRenderer>();

            float time = 1f;
            while (time > 0f)
            {
                time -= 2f * Time.deltaTime;
                ballRB.transform.localScale = Vector3.one * time;
                if (trail) trail.widthMultiplier = time;
                yield return null;
            }

            ballRB.gameObject.SetActive(false);
            ballRB.transform.localScale = Vector3.one;
            if (trail) trail.widthMultiplier = 1f;

            SoundManager.Instance.PlaySound(SoundType.BallPocketed);

            pocketingBalls.Remove(ballRB);
            
            if (ballRB == cueBallRB)
            {
                Stop(MinigameEndType.Lose);
            }
            else
            {
                ballsSunk++;
                if (ballsSunk >= 15) Stop(MinigameEndType.Win);
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
        cueBallRB.Sleep();
        //cueBallRB.position = cueBallStartPosition;
        cueBallRB.transform.position = cueBallStartPosition;
        cueBallRB.rotation = 0f;
        cueBallRB.gameObject.SetActive(true);
        //cueBallRB.Sleep();

        for (int i = 0; i < valueBallRBs.Length; i++)
        {
            Rigidbody2D rb = valueBallRBs[i];
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
            rb.Sleep();
            //rb.position = valueBallStartPositions[i];
            rb.transform.position = valueBallStartPositions[i];
            rb.rotation = 0f;
            rb.gameObject.SetActive(true);
            //rb.Sleep();
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