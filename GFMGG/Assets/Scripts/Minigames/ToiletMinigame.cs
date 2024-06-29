using System.Collections;
using UnityEngine;

public class ToiletMinigame : Minigame
{
    MinigameManager manager;
    bool running;
    bool waitingForToilet;

    KeyCode[] poopKeyCodes = new KeyCode[] { KeyCode.E, KeyCode.F, KeyCode.Z, KeyCode.Space, KeyCode.Return };

    float flatulence;
    float bowelFillRate = 0.4f;
    float emptyPerClick = 0.1f;
    float rampingUpSpeed;

    public override IEnumerator StartCor(MinigameManager manager)
    {
        this.manager = manager;
        GameManager.Instance.SetInputCapture(true);

        flatulence = 0.5f;
        rampingUpSpeed = 1f;

        yield return UIManager.Instance.FadeInCor();
        manager.ToiletGameHolder.SetActive(true);
        manager.ToiletGameScreen1.SetActive(true);
        manager.ToiletGameScreen2.SetActive(false);
        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
        waitingForToilet = true;
    }

    public override IEnumerator EndCor(MinigameEndType minigameEndType)
    {
        yield return UIManager.Instance.FadeInCor();
        running = false;
        waitingForToilet = false;
        flatulence = 1f;

        manager.ToiletGameHolder.SetActive(false);

        manager.ToiletGameScreen2.SetActive(false);
        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
        GameManager.Instance.SetInputCapture(false);
    }

    public override void Update()
    {
        if (!running) return;

        const float CONST_MAX_RAMP_UP_SPEED = 2f;
        if (rampingUpSpeed < CONST_MAX_RAMP_UP_SPEED) rampingUpSpeed += 0.5f * Time.deltaTime;
        else if (rampingUpSpeed > CONST_MAX_RAMP_UP_SPEED) rampingUpSpeed = CONST_MAX_RAMP_UP_SPEED;

        flatulence -= bowelFillRate * rampingUpSpeed * Time.deltaTime;
        if (Helpers.KeyDown(ref poopKeyCodes, false))
        {
            flatulence += emptyPerClick;
        }

        flatulence = Mathf.Clamp01(flatulence);
        manager.ToiletGameFillBar.fillAmount = flatulence;

        if (flatulence == 0f) Stop(MinigameEndType.Lose);
        else if (flatulence == 1f) Stop(MinigameEndType.Win);
    }

    void Stop(MinigameEndType minigameEndType)
    {
        running = false;
        manager.StopMinigame(minigameEndType);
    }

    public void ToiletClicked()
    {
        if (waitingForToilet)
        {
            waitingForToilet = false;
            running = true;

            manager.ToiletGameScreen1.SetActive(false);
            manager.ToiletGameScreen2.SetActive(true);
        }
    }
}