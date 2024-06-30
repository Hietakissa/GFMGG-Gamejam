using System.Collections;
using UnityEngine;

public class ToiletMinigame : Minigame
{
    MinigameManager manager;
    bool running;
    bool waitingForToilet;

    KeyCode[] poopKeyCodes = new KeyCode[] { KeyCode.E, KeyCode.F, KeyCode.Z, KeyCode.Space, KeyCode.Return };

    float flatulence;
    float baseBowelFillRate = 0.4f;
    float bowelFillRate;
    float emptyPerClick = 0.1f;
    float rampingUpSpeed;
    float baseMaxRampUp = 1.5f;
    float maxRampUp = 1.5f;
    public override IEnumerator StartCor(MinigameManager manager)
    {
        this.manager = manager;
        GameManager.Instance.SetInputCapture(true);

        flatulence = 0.5f;
        rampingUpSpeed = 1f;
        bowelFillRate = baseBowelFillRate * manager.DifficultyMultiplier;
        maxRampUp = Mathf.Max(baseMaxRampUp, baseMaxRampUp * (manager.DifficultyMultiplier * 0.5f));

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
        if (waitingForToilet)
        {
            if (Helpers.KeyDown(ref poopKeyCodes, false))
            {
                ToiletClicked();
            }
        }
        if (!running) return;

        if (rampingUpSpeed < maxRampUp) rampingUpSpeed += 0.25f * Time.deltaTime;
        else if (rampingUpSpeed > maxRampUp) rampingUpSpeed = maxRampUp;

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