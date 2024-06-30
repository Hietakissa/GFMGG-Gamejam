using UnityEngine;

public class MinigameStarter : MonoBehaviour, IInteractable
{
    [SerializeField] MinigameType minigame;
    [SerializeField] string overrideInteractionText;
    [SerializeField] float difficultyMultiplier = 1f;
    [SerializeField]
    [ConditionalField(nameof(minigame), (int)MinigameType.ShowImage)] int imageIndex;
    [SerializeField]
    [ConditionalField(nameof(minigame), (int)MinigameType.ShowImage)] string dialogue;

    public string GetInteractionText()
    {
        if (!string.IsNullOrEmpty(overrideInteractionText)) return overrideInteractionText;

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
        if (minigame == MinigameType.ShowImage) MinigameManager.Instance.SetImageIndex(imageIndex);
        MinigameManager.Instance.SetDifficultyMultiplier(difficultyMultiplier);
        MinigameManager.Instance.StartMinigame(minigame);
    }
}