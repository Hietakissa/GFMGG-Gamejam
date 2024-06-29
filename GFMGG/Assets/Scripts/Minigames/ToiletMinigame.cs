using System.Collections;
using UnityEngine;

public class ToiletMinigame : Minigame
{
    public override IEnumerator StartCor(MinigameManager manager)
    {
        yield return UIManager.Instance.FadeInCor();
        manager.CoffeeGameHolder.SetActive(true);
        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
    }

    public override void End(MinigameEndType minigameEndType)
    {
        
    }

    public override void Update()
    {
        
    }
}