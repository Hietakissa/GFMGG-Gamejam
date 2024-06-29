using UnityEngine;

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Manager[] externalManagers;

    void Awake()
    {
        Instance = this;

        Manager[] managers = GetComponentsInChildren<Manager>();
        foreach (Manager manager in managers) manager.Initialize();
        foreach (Manager manager in externalManagers) manager.Initialize();
    }
}