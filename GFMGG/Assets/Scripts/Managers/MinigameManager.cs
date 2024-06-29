using HietakissaUtils.QOL;
using System.Collections;
using HietakissaUtils;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MinigameManager : Manager
{
    public static MinigameManager Instance;

    #region Password Game
    [Header("Password Minigame")]
    [SerializeField] GameObject passwordGameHolder;
    [SerializeField] string[] passwords;

    [SerializeField] Color passwordGameNormalColor;
    [SerializeField] Color passwordGameNextCharacterColor;
    [SerializeField] Color passwordGameWinColor;
    [SerializeField] Color passwordGameLoseColor;

    [SerializeField] TextMeshProUGUI passwordGameTypeText;
    [SerializeField] TextMeshProUGUI passwordGameDashText;
    
    public GameObject PasswordGameHolder => passwordGameHolder;
    public Color PasswordGameNormalColor => passwordGameNormalColor;
    public Color PasswordGameNextCharacterColor => passwordGameNextCharacterColor;
    public Color PasswordGameWinColor => passwordGameWinColor;
    public Color PasswordGameLoseColor => passwordGameLoseColor;
    
    public TextMeshProUGUI PasswordGameTypeText => passwordGameTypeText;
    public TextMeshProUGUI PasswordGameDashText => passwordGameDashText;
    #endregion
    #region Coffee Game
    [Header("Coffee Minigame")]
    [SerializeField] GameObject coffeeGameHolder;
    [SerializeField] Transform coffeeGameIndicator;
    [SerializeField] Transform coffeeGameIndicatorTop;
    [SerializeField] Transform coffeeGameIndicatorBottom;
    [SerializeField] Transform coffeeGameIndicatorAbsoluteTop;
    [SerializeField] Transform coffeeGameIndicatorAbsoluteBottom;
    [SerializeField] RectTransform coffeeGameArea;
    [SerializeField] RectTransform coffeeGameBackground;

    public GameObject CoffeeGameHolder => coffeeGameHolder;
    public Transform CoffeeGameIndicator => coffeeGameIndicator;
    public Transform CoffeeGameIndicatorTop => coffeeGameIndicatorTop;
    public Transform CoffeeGameIndicatorBottom => coffeeGameIndicatorBottom;
    public Transform CoffeeGameIndicatorAbsoluteTop => coffeeGameIndicatorAbsoluteTop;
    public Transform CoffeeGameIndicatorAbsoluteBottom => coffeeGameIndicatorAbsoluteBottom;
    public RectTransform CoffeeGameArea => coffeeGameArea;
    public RectTransform CoffeeGameBackground => coffeeGameBackground;
    #endregion
    #region Toilet Game
    [Header("Toilet Minigame")]
    [SerializeField] GameObject toiletGameHolder;
    [SerializeField] GameObject toiletGameScreen1;
    [SerializeField] GameObject toiletGameScreen2;
    [SerializeField] Image toiletGameFillBar;

    public GameObject ToiletGameHolder => toiletGameHolder;
    public GameObject ToiletGameScreen1 => toiletGameScreen1;
    public GameObject ToiletGameScreen2 => toiletGameScreen2;
    public Image ToiletGameFillBar => toiletGameFillBar;
    #endregion

    PasswordMinigame passwordMinigame = new PasswordMinigame();
    CoffeeMinigame coffeeMinigame = new CoffeeMinigame();
    ToiletMinigame toiletMinigame = new ToiletMinigame();
    Minigame currentMinigame;



    public override void Initialize()
    {
        Instance = this;
    }


    void Update()
    {
        currentMinigame?.Update();
    }

    public void StartMinigame(MinigameType minigameType)
    {
        if (currentMinigame != null)
        {
            Debug.Log($"Tried to start minigame '{minigameType}', but '{currentMinigame.GetType()}' is already running");
            return;
        }

        currentMinigame = minigameType switch
        {
            MinigameType.Password => passwordMinigame,
            MinigameType.Coffee => coffeeMinigame,
            MinigameType.Toilet => toiletMinigame,
            _ => null
        };

        if (currentMinigame != null)
        {
            Debug.Log($"Starting minigame {minigameType}");
            StartCoroutine(currentMinigame.StartCor(this));
        }
        else Debug.Log($"MinigameManager StartMinigame Switch statement not implemented for '{minigameType}'");
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
    public void ToiletClicked()
    {
        toiletMinigame.ToiletClicked();
    }
}