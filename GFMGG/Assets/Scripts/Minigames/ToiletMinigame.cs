using System.Collections;
using UnityEngine;

public class ToiletMinigame : Minigame
{
    MinigameManager manager;
    bool running;
    bool waitingForToilet;

    KeyCode[] poopKeyCodes = new KeyCode[] { KeyCode.E, KeyCode.F, KeyCode.Space, KeyCode.Return };

    float flatulence;
    float bowelFillRate = 0.4f;
    float emptyPerClick = 0.1f;

    public override IEnumerator StartCor(MinigameManager manager)
    {
        this.manager = manager;
        GameManager.Instance.SetInputCapture(true);

        flatulence = 1f;

        yield return UIManager.Instance.FadeInCor();
        manager.ToiletGameHolder.SetActive(true);
        MinigameManager.Instance.ToiletGameScreen1.SetActive(true);
        MinigameManager.Instance.ToiletGameScreen2.SetActive(false);
        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
        waitingForToilet = true;
    }

    public override void End(MinigameEndType minigameEndType)
    {
        running = false;
        waitingForToilet = false;
        flatulence = 1f;

        GameManager.Instance.SetInputCapture(false);
        manager.ToiletGameHolder.SetActive(false);

        MinigameManager.Instance.ToiletGameScreen2.SetActive(false);
    }

    public override void Update()
    {
        if (!running) return;

        flatulence -= bowelFillRate * Time.deltaTime;
        if (Helpers.KeyDown(ref poopKeyCodes, false))
        {
            flatulence += emptyPerClick;
        }

        flatulence = Mathf.Clamp01(flatulence);
        MinigameManager.Instance.ToiletGameFillBar.fillAmount = flatulence;

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

            MinigameManager.Instance.ToiletGameScreen1.SetActive(false);
            MinigameManager.Instance.ToiletGameScreen2.SetActive(true);
        }
    }
}