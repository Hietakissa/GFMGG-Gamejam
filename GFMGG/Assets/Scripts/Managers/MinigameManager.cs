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
    #region Billiard Game
    [Header("Billiard Minigame")]
    [SerializeField] GameObject billiardGameHolder;
    [SerializeField] Transform billiardGameCameraOverride;
    [SerializeField] Transform[] billiardGamePockets;
    [SerializeField] Transform billiardGameCueBall;
    [SerializeField] Transform[] billiardGameValueBalls;
    [SerializeField] Transform billiardGameCueRotate;
    [SerializeField] Transform billiardGameCueMove;
    [SerializeField] TextMeshProUGUI billiardGameShotsText;

    public GameObject BilliardGameHolder => billiardGameHolder;
    public Transform BilliardGameCameraOverride => billiardGameCameraOverride;
    public Transform[] BilliardGamePockets => billiardGamePockets;
    public Transform BilliardGameCueBall => billiardGameCueBall;
    public Transform[] BilliardGameValueBalls => billiardGameValueBalls;
    public Transform BilliardGameCueRotate => billiardGameCueRotate;
    public Transform BilliardGameCueMove => billiardGameCueMove;
    public TextMeshProUGUI BilliardGameShotsText => billiardGameShotsText;
    #endregion
    #region ShowImage Game
    [Header("Show Image Minigame")]
    [SerializeField] GameObject showImageGameHolder;
    int showImageGameIndex;

    public GameObject ShowImageGameHolder => showImageGameHolder;
    public int ShowImageGameIndex => showImageGameIndex;
    #endregion

    [SerializeField] GameObject timerBar;
    [SerializeField] Image timerfill;
    float difficultyMultiplier;

    public GameObject TimerBar => timerBar;
    public Image TimerFill => timerfill;
    public float DifficultyMultiplier => difficultyMultiplier;


    PasswordMinigame passwordMinigame = new PasswordMinigame();
    CoffeeMinigame coffeeMinigame = new CoffeeMinigame();
    ToiletMinigame toiletMinigame = new ToiletMinigame();
    BilliardMinigame billiardMinigame = new BilliardMinigame();
    ShowImageMinigame showImageMinigame = new ShowImageMinigame();
    Minigame currentMinigame;



    public override void Initialize()
    {
        Instance = this;
    }


    void Update() => currentMinigame?.Update();
    void FixedUpdate() => currentMinigame?.FixedUpdate();


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
            MinigameType.Billiard => billiardMinigame,
            MinigameType.ShowImage => showImageMinigame,
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
            if (minigameEnd == MinigameEndType.Win)
            {
                for (int i = 0; i < 3; i++)
                {
                    yield return QOL.WaitForSeconds.Get(0.3f);
                    SoundManager.Instance.PlaySound(SoundType.Success);
                }
            }
            else if (minigameEnd == MinigameEndType.Lose)
            {
                for (int i = 0; i < 2; i++)
                {
                    yield return QOL.WaitForSeconds.Get(0.15f);
                    SoundManager.Instance.PlaySound(SoundType.Fail);
                }
            }

            yield return QOL.WaitForSeconds.Get(1f);
            yield return currentMinigame.EndCor(minigameEnd);
            currentMinigame = null;
            Debug.Log($"Minigame stopped. {minigameEnd}");
        }
    }

    public string GetPassword() => passwords.RandomElement();
    public void ToiletClicked()
    {
        toiletMinigame.ToiletClicked();
    }
    public void SetImageIndex(int index) => showImageGameIndex = index;
    public void SetDifficultyMultiplier(float difficultyMultiplier) => this.difficultyMultiplier = difficultyMultiplier;
}