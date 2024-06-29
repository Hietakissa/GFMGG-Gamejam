using HietakissaUtils;
using UnityEngine;
using TMPro;
using System.Collections;
using HietakissaUtils.QOL;

public class MinigameManager : Manager
{
    public static MinigameManager Instance;

    [Header("Password Minigame")]
    [SerializeField] GameObject passwordGameHolder;
    [SerializeField] string[] passwords;
    [SerializeField] TextMeshProUGUI passwordGameTypeText;
    [SerializeField] TextMeshProUGUI passwordGameDashText;
    [SerializeField] Color passwordGameNormalColor;
    [SerializeField] Color passwordGameNextCharacterColor;
    [SerializeField] Color passwordGameWinColor;
    [SerializeField] Color passwordGameLoseColor;

    PasswordMinigame passwordMinigame = new PasswordMinigame();
    Minigame currentMinigame;

    public GameObject PasswordGameHolder => passwordGameHolder;
    public Color PasswordGameNormalColor => passwordGameNormalColor;
    public Color PasswordGameNextCharacterColor => passwordGameNextCharacterColor;
    public Color PasswordGameWinColor => passwordGameWinColor;
    public Color PasswordGameLoseColor => passwordGameLoseColor;
    public TextMeshProUGUI PasswordGameTypeText => passwordGameTypeText;
    public TextMeshProUGUI PasswordGameDashText => passwordGameDashText;


    public override void Initialize()
    {
        Instance = this;
    }


    void Update()
    {
        currentMinigame?.Update();

        if (Input.GetKeyDown(KeyCode.P) && currentMinigame == null) StartMinigame(MinigameType.Password);
    }

    public void StartMinigame(MinigameType minigameType)
    {
        if (currentMinigame != null)
        {
            Debug.Log($"Tried to start minigame '{minigameType}', but '{currentMinigame.GetType()}' is already running");
            return;
        }


        switch (minigameType)
        {
            case MinigameType.Password:
                currentMinigame = passwordMinigame;
                break;
        }

        if (currentMinigame != null)
        {
            Debug.Log($"Starting minigame {minigameType}");
            StartCoroutine(currentMinigame.StartCor(this));
        }
    }

    public void StopMinigame(MinigameEndType minigameEnd)
    {
        StartCoroutine(StopMinigameCor());

        IEnumerator StopMinigameCor()
        {
            yield return QOL.WaitForSeconds.Get(1f);
            currentMinigame?.End(minigameEnd);
            currentMinigame = null;
            Debug.Log($"Minigame stopped. {minigameEnd}");
        }
    }

    public string GetPassword() => passwords.RandomElement();
}