using UnityEngine;

public class MinigameStarter : MonoBehaviour, IInteractable
{
    [SerializeField] MinigameType minigame;
    [SerializeField]
    [ConditionalField(nameof(minigame), (int)MinigameType.ShowImage)] int imageIndex;
    [SerializeField]
    [ConditionalField(nameof(minigame), (int)MinigameType.ShowImage)] string dialogue;

    public string GetInteractionText()
    {
        return minigame switch
        {
            MinigameType.Password => "Enter password",
            MinigameType.Coffee => "Brew coffee",
            MinigameType.Toilet => "Sit on the toilet",
            MinigameType.Billiard => "Play pool",
            _ => "N/A"
        };
    }
    public void Interact()
    {
        MinigameManager.Instance.StartMinigame(minigame);
        if (minigame == MinigameType.ShowImage) MinigameManager.Instance.SetImageIndex(imageIndex);
    }
}