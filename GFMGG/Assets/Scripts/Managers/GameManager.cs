using HietakissaUtils.QOL;

using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool InputCapture => inputCapture;
    bool inputCapture;

    public MovementController MovementController => movementController;
    [SerializeField] MovementController movementController;

    public CameraController CameraController => cameraController;
    [SerializeField] CameraController cameraController;

    [SerializeField] Manager[] externalManagers;

    KeyCode[] pauseKeyCodes = new KeyCode[] { KeyCode.Escape, KeyCode.Tab };
    bool paused;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] AudioMixer mixer;


    void Awake()
    {
        Instance = this;

        Manager[] managers = GetComponentsInChildren<Manager>();
        foreach (Manager manager in managers) manager.Initialize();
        foreach (Manager manager in externalManagers) manager.Initialize();
    }

    void Update()
    {
        if (Helpers.KeyDown(ref pauseKeyCodes))
        {
            paused = !paused;

            pauseMenu.SetActive(paused);
        }
    }

    public void Unpause()
    {
        paused = false;
        pauseMenu.SetActive(paused);
    }
    public void Quit()
    {
        QOL.Quit();
    }
    public void SetVolume(float volume) => mixer.SetFloat("Master Volume", volume);
    public void SetInputCapture(bool inputCapture) => this.inputCapture = inputCapture;
}