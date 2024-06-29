using System.Collections;
using UnityEngine;

public abstract class Minigame
{
    public abstract IEnumerator StartCor(MinigameManager manager);
    public abstract void End(MinigameEndType minigameEndType);
    public abstract void Update();
}

public enum MinigameType
{
    None,
    Password,
    Coffee,
    Toilet,
    Billiard
}

public enum MinigameEndType
{
    None,
    Win,
    Lose
}