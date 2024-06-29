using UnityEngine;

public class MinigameStarter : MonoBehaviour, IInteractable
{
    [SerializeField] MinigameType minigame;

    public string GetInteractionText() => $"Play {minigame} game";
    public void Interact()
    {
        MinigameManager.Instance.StartMinigame(minigame);
    }
}