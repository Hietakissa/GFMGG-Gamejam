using System.Collections;
using UnityEngine;

public class ShowImageMinigame : Minigame
{
    MinigameManager manager;
    bool running;

    KeyCode[] continueKeyCodes = new KeyCode[] { KeyCode.E, KeyCode.F, KeyCode.Z, KeyCode.Space, KeyCode.Return };
    GameObject activeImage;

    public override IEnumerator StartCor(MinigameManager manager)
    {
        this.manager = manager;
        GameManager.Instance.SetInputCapture(true);


        yield return UIManager.Instance.FadeInCor();
        
        manager.ShowImageGameHolder.SetActive(true);
        
        if (activeImage != null) activeImage.SetActive(false);
        activeImage = manager.ShowImageGameHolder.transform.GetChild(manager.ShowImageGameIndex).gameObject;
        activeImage.SetActive(true);

        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
        running = true;
    }

    public override IEnumerator EndCor(MinigameEndType minigameEndType)
    {
        yield return UIManager.Instance.FadeInCor();
        running = false;

        manager.ShowImageGameHolder.SetActive(false);

        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
        GameManager.Instance.SetInputCapture(false);
    }

    public override void Update()
    {
        if (!running) return;

        if (Helpers.KeyDown(ref continueKeyCodes, false)) Stop(MinigameEndType.None);
    }

    void Stop(MinigameEndType minigameEndType)
    {
        running = false;
        manager.StopMinigame(minigameEndType);
    }
}