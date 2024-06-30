using HietakissaUtils.QOL;
using System.Collections;
using HietakissaUtils;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] Dialogue[] dialogue;
    [SerializeField] string npcName;

    [Header("Jari Only")]
    [SerializeField] Dialogue barbaariDialogue;
    [SerializeField] float barbaariChance;
    [SerializeField] GameObject barbaariCues;
    bool barbaari;

    public string GetInteractionText() => $"Talk to {npcName}";
    public void Interact()
    {
        Dialogue dialogue;
        if (barbaari)
        {
            dialogue = barbaariDialogue;
            barbaari = false;
        }
        else dialogue = this.dialogue.RandomElement();

        StartCoroutine(DisplayDialogue(dialogue));
    }

    IEnumerator DisplayDialogue(Dialogue dialogue)
    {
        GameManager.Instance.SetInputCapture(true);

        if (dialogue.TextBubble)
        {
            dialogue.TextBubble.SetActive(true);
            yield return QOL.WaitForSeconds.Get(dialogue.TextDuration);
            dialogue.TextBubble.SetActive(false);
        }

        GameManager.Instance.SetInputCapture(false);

        if (dialogue.ImageIndex != -1)
        {
            MinigameManager.Instance.SetImageIndex(dialogue.ImageIndex);
            MinigameManager.Instance.StartMinigame(MinigameType.ShowImage);
        }
    }

    public void RandomizeBarbaari()
    {
        barbaari = Maf.RandomBool(barbaariChance);
        barbaariCues.SetActive(barbaari);
    }


    [System.Serializable]
    struct Dialogue
    {
        [SerializeField] GameObject textBubble;
        [SerializeField] float textDuration;
        [SerializeField] int imageIndex;

        public GameObject TextBubble => textBubble;
        public float TextDuration => textDuration;
        public int ImageIndex => imageIndex;
    }
}