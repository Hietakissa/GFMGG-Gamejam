using System.Collections;
using UnityEngine;

public class CoffeeMinigame : Minigame
{
    MinigameManager manager;
    bool running = false;


    public override IEnumerator StartCor(MinigameManager manager)
    {
        this.manager = manager;

        GameManager.Instance.SetInputCapture(true);

        yield return UIManager.Instance.FadeInCor();
        manager.CoffeeGameHolder.SetActive(true);
        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
        running = true;

        Debug.Log($"Started coffee game");
    }

    public override void End(MinigameEndType minigameEndType)
    {
        GameManager.Instance.SetInputCapture(false);
        manager.CoffeeGameHolder.SetActive(false);
    }

    public override void Update()
    {
        if (!running) return;

        float t = Mathf.PingPong(Time.time, 1);
    }

    void Stop(MinigameEndType minigameEndType)
    {
        manager.StopMinigame(minigameEndType);
    }
}