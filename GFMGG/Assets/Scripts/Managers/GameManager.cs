using UnityEngine;

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool InputCapture => inputCapture;
    bool inputCapture;

    public MovementController MovementController => movementController;
    [SerializeField] MovementController movementController;

    [SerializeField] Manager[] externalManagers;

    void Awake()
    {
        Instance = this;

        Manager[] managers = GetComponentsInChildren<Manager>();
        foreach (Manager manager in managers) manager.Initialize();
        foreach (Manager manager in externalManagers) manager.Initialize();
    }


    public void SetInputCapture(bool inputCapture) => this.inputCapture = inputCapture;
}