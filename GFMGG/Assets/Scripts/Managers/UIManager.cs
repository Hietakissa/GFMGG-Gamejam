using UnityEngine;

public class UIManager : Manager
{
    public static UIManager Instance;

    public override void Initialize()
    {
        Instance = this;
    }
}