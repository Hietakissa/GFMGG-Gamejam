using System.Collections;
using HietakissaUtils;
using UnityEngine;

public class BoxingMinigame : Minigame
{
    MinigameManager manager;
    bool running;

    KeyCode[] hitKeyCodes = new KeyCode[] { KeyCode.E, KeyCode.F, KeyCode.Z, KeyCode.Return };
    KeyCode[] blockKeyCodes = new KeyCode[] { KeyCode.Space };

    float jariBasePunchDelay = 1f;
    float jariPunchDelay;
    float jariPunchTime;
    float jariBaseMaxPunchCooldown = 3f;
    float jariMaxPunchCooldown;
    float jariBaseMinPunchCooldown = 1f;
    float jariMinPunchCooldown;

    float jariPunchCooldown;
    int jariPunchDirection;
    bool jariWaitingForPunch;

    bool blocking;
    int blockDirection;


    float punchDelay = 0.2f;
    float punchDelayTime;

    int jariMaxLives = 100;
    int jariLives;
    int maxLives = 3;
    int lives;

    public override IEnumerator StartCor(MinigameManager manager)
    {
        this.manager = manager;
        GameManager.Instance.SetInputCapture(true);

        jariPunchDelay = jariBasePunchDelay / manager.DifficultyMultiplier;
        jariPunchTime = 0f;
        jariPunchCooldown = Random.Range(2f, 5f);
        jariMaxPunchCooldown = jariBaseMaxPunchCooldown / manager.DifficultyMultiplier;
        jariMinPunchCooldown = jariBaseMinPunchCooldown / manager.DifficultyMultiplier;
        jariWaitingForPunch = false;
        jariLives = (jariMaxLives * manager.DifficultyMultiplier).RoundDown();
        lives = maxLives;
        manager.BoxingGameJariLivesFill.fillAmount = 1f;
        manager.BoxingGamePlayerLivesFill.fillAmount = 1f;

        yield return UIManager.Instance.FadeInCor();
        manager.BoxingGameHolder.SetActive(true);
        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
        running = true;
    }

    public override IEnumerator EndCor(MinigameEndType minigameEndType)
    {
        yield return UIManager.Instance.FadeInCor();
        running = false;

        manager.BoxingGameHolder.SetActive(false);

        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
        GameManager.Instance.SetInputCapture(false);
    }

    public override void Update()
    {
        if (!running) return;

        if (jariPunchCooldown > 0f) jariPunchCooldown -= Time.deltaTime;
        else
        {
            if (!jariWaitingForPunch)
            {
                jariWaitingForPunch = true;
                jariPunchDirection = Random.Range(0, 3); // 0 left 1 middle 2 right (relative to player)
                for (int i = 0; i < 3; i++)
                {
                    manager.BoxingGameExclamationSprites[i].SetActive(false);
                }
                manager.BoxingGameExclamationSprites[jariPunchDirection].SetActive(true);
            }

            jariPunchTime += Time.deltaTime;
            if (jariPunchTime >= jariPunchDelay)
            {
                jariPunchTime = 0f;
                JariPunch();
            }
        }
        

        if (punchDelayTime > 0f)
        {
            punchDelayTime -= Time.deltaTime;
            manager.BoxingGamePlayerHandsSprite.sprite = manager.BoxingGamePlayerSprites[3];
        }
        else manager.BoxingGamePlayerHandsSprite.sprite = manager.BoxingGamePlayerSprites[4];

        if (!blocking && punchDelayTime <= 0f && Helpers.KeyDown(ref hitKeyCodes, false)) Punch();
        else if (punchDelayTime <= 0f && Helpers.Key(ref blockKeyCodes, false))
        {
            Block();
            blocking = true;
        }
        else blocking = false;
    }

    void JariPunch()
    {
        bool successfulBlock = blocking && jariPunchDirection == blockDirection;
        Debug.Log($"Jari punched. Blocked: {successfulBlock}");

        jariPunchCooldown = Random.Range(jariMinPunchCooldown, jariMaxPunchCooldown);
        jariWaitingForPunch = false;

        for (int i = 0; i < 3; i++)
        {
            manager.BoxingGameExclamationSprites[i].SetActive(false);
        }

        if (!successfulBlock)
        {
            lives--;

            manager.BoxingGamePlayerLivesFill.fillAmount = lives / (float)maxLives;
            if (lives <= 0) Stop(MinigameEndType.Lose);
        }
    }

    void Punch()
    {
        punchDelayTime = punchDelay;

        Debug.Log("Punched");

        jariLives--;
        manager.BoxingGameJariLivesFill.fillAmount = jariLives / (float)(jariMaxLives);

        if (jariLives <= 0)
        {
            Stop(MinigameEndType.Win);
        }
    }

    void Block()
    {
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (inputVector.x < 0f) blockDirection = 0;
        else if (inputVector.y > 0f || inputVector.y < 0f) blockDirection = 1;
        else if (inputVector.x > 0f) blockDirection = 2;
        else blockDirection = -1;

        if (blockDirection == -1) manager.BoxingGamePlayerHandsSprite.sprite = manager.BoxingGamePlayerSprites[4];
        else manager.BoxingGamePlayerHandsSprite.sprite = manager.BoxingGamePlayerSprites[blockDirection];
    }

    void Stop(MinigameEndType minigameEndType)
    {
        running = false;
        manager.StopMinigame(minigameEndType);
    }
}