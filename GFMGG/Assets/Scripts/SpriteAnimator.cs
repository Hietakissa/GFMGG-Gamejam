using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteAnimationState[] animationStates;

    [SerializeField] int stateIndex;
    [SerializeField] int subStateIndex;
    [SerializeField] float frameDuration;
    int frame;
    float frameTime;

    public void SetStateIndex(int stateIndex)
    {
        if (this.stateIndex != stateIndex)
        {
            this.stateIndex = stateIndex;
            RefreshSprite();
        }
    }
    public void SetSubStateIndex(int subStateIndex)
    {
        if (this.subStateIndex != subStateIndex)
        {
            this.subStateIndex = subStateIndex;
            RefreshSprite();
        }
    }


    void RefreshSprite()
    {
        spriteRenderer.sprite = animationStates[stateIndex].SubStates[subStateIndex].Frames[frame];
    }

    void Update()
    {
        frameTime += Time.deltaTime;
        if (frameTime >= frameDuration)
        {
            frameTime -= frameDuration;
            frame++;

            frame %= animationStates[stateIndex].SubStates[subStateIndex].Frames.Length;
            RefreshSprite();
        }
    }
}

[System.Serializable]
struct SpriteAnimationState
{
    [SerializeField] string name;
    [SerializeField] SpriteAnimationSubState[] subStates;

    [HideInInspector] public SpriteAnimationSubState[] SubStates => subStates;
}

[System.Serializable]
struct SpriteAnimationSubState
{
    [SerializeField] string name;
    [SerializeField] Sprite[] frames;

    [HideInInspector] public Sprite[] Frames => frames;
}