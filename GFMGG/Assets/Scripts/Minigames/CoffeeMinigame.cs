using System.Collections;
using HietakissaUtils;
using UnityEngine;

public class CoffeeMinigame : Minigame
{
    MinigameManager manager;

    KeyCode[] stopKeyCodes = new KeyCode[] { KeyCode.E, KeyCode.F, KeyCode.Z, KeyCode.Space, KeyCode.Return };

    bool running = false;

    float indicatorHeightPercent;
    float areaHeightPercent;
    float areaCenter;
    float indicatorSpeed;

    public override IEnumerator StartCor(MinigameManager manager)
    {
        this.manager = manager;

        GameManager.Instance.SetInputCapture(true);

        float backgroundHeight = manager.CoffeeGameBackground.sizeDelta.y;
        areaHeightPercent = Random.Range(0.05f, 0.15f);
        float areaHeight = areaHeightPercent * backgroundHeight;

        indicatorSpeed = Random.Range(1.5f, 4f);
        indicatorHeightPercent = ((RectTransform)manager.CoffeeGameIndicator).sizeDelta.y / backgroundHeight;
        areaCenter = Random.Range(0.25f, 0.75f);

        manager.CoffeeGameArea.sizeDelta = manager.CoffeeGameArea.sizeDelta.SetY(areaHeight);
        manager.CoffeeGameArea.position = Vector3.Lerp(manager.CoffeeGameIndicatorAbsoluteTop.position, manager.CoffeeGameIndicatorAbsoluteBottom.position, areaCenter);
        //manager.CoffeeGameArea.position = Vector3.Lerp(manager.CoffeeGameIndicatorTop.position, manager.CoffeeGameIndicatorBottom.position, areaCenter);

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

        float t = Mathf.PingPong(Time.time * indicatorSpeed, 1);
        manager.CoffeeGameIndicator.position = Vector3.Lerp(manager.CoffeeGameIndicatorAbsoluteTop.position, manager.CoffeeGameIndicatorAbsoluteBottom.position, t);

        if (Helpers.KeyDown(ref stopKeyCodes, false))
        {
            Debug.Log($"Coffee game stopped, t: {t}, range: {areaCenter - areaHeightPercent * 0.5f} - {areaCenter + areaHeightPercent * 0.5f}");

            if (t > areaCenter - areaHeightPercent * 0.5f - indicatorHeightPercent * 0.5f && t < areaCenter + areaHeightPercent * 0.5f + indicatorHeightPercent * 0.5f)
            {
                Stop(MinigameEndType.Win);
            }
            else Stop(MinigameEndType.Lose);
        }
    }

    void Stop(MinigameEndType minigameEndType)
    {
        running = false;
        manager.StopMinigame(minigameEndType);
    }
}