using System.Collections;
using UnityEngine;

public abstract class Minigame
{
    public abstract IEnumerator StartCor(MinigameManager manager);
    public abstract IEnumerator EndCor(MinigameEndType minigameEndType);
    public abstract void Update();

    public virtual void FixedUpdate() { }
}

public enum MinigameType
{
    None,
    Password,
    Coffee,
    Toilet,
    Billiard,
    ShowImage,
    Boxing
}

public enum MinigameEndType
{
    None,
    Win,
    Lose
}