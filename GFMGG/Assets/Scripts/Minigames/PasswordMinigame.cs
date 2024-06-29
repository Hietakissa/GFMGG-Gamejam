using System.Collections;
using System.Text;
using UnityEngine;

public class PasswordMinigame : Minigame
{
    MinigameManager manager;
    bool running = false;

    int passwordCharIndex = 0;
    string password;
    string nextChar;

    public override IEnumerator StartCor(MinigameManager manager)
    {
        this.manager = manager;

        GameManager.Instance.SetInputCapture(true);

        password = manager.GetPassword();
        passwordCharIndex = 0;
        nextChar = password[passwordCharIndex].ToString();

        yield return UIManager.Instance.FadeInCor();
        manager.PasswordGameHolder.SetActive(true);
        UpdateText(MinigameEndType.None);
        yield return UIManager.Instance.FadeWaitCor();
        yield return UIManager.Instance.FadeOutCor();
        running = true;

        Debug.Log($"Started password game. Password: {password}");
    }

    public override void End(MinigameEndType minigameEndType)
    {
        GameManager.Instance.SetInputCapture(false);
        manager.PasswordGameHolder.SetActive(false);
    }

    public override void Update()
    {
        if (!running) return;

        string input = Input.inputString.ToLower();
        if (string.IsNullOrEmpty(input)) return;

        if (input == nextChar)
        {
            passwordCharIndex++;

            if (passwordCharIndex >= password.Length) Stop(MinigameEndType.Win);
            else
            {
                nextChar = password[passwordCharIndex].ToString();
                UpdateText(MinigameEndType.None);
            }
        }
        else Stop(MinigameEndType.Lose);
    }

    void Stop(MinigameEndType minigameEndType)
    {
        running = false;
        UpdateText(minigameEndType);
        manager.StopMinigame(minigameEndType);
    }

    void UpdateText(MinigameEndType minigameEndType)
    {
        StringBuilder dashSB = new StringBuilder();
        dashSB.Insert(0, "-", password.Length);
        manager.PasswordGameDashText.text = dashSB.ToString();

        string written = string.Empty;
        if (passwordCharIndex > 0) written = password.Substring(0, passwordCharIndex);

        StringBuilder passwordSB = new StringBuilder();
        if (minigameEndType == MinigameEndType.None)
        {
            // Currently written
            passwordSB.Append($"<color=#{ColorUtility.ToHtmlStringRGB(manager.PasswordGameNormalColor)}>");
            passwordSB.Append(written);
            passwordSB.Append("</color>");

            // Next character
            passwordSB.Append($"<color=#{ColorUtility.ToHtmlStringRGB(manager.PasswordGameNextCharacterColor)}>");
            passwordSB.Append(nextChar);
            passwordSB.Append("</color>");

            // Question marks
            int questionMarkNum = password.Length - passwordCharIndex - 1;

            Debug.Log($"Written: {written}, Next char: {nextChar}, Question marks: {questionMarkNum}");
            passwordSB.Append($"<color=#{ColorUtility.ToHtmlStringRGB(manager.PasswordGameNormalColor)}>");
            passwordSB.Insert(passwordSB.Length, "?", questionMarkNum);
            passwordSB.Append("</color>");
        }
        else if (minigameEndType == MinigameEndType.Win)
        {
            passwordSB.Append($"<color=#{ColorUtility.ToHtmlStringRGB(manager.PasswordGameWinColor)}>");
            passwordSB.Append(password);
            passwordSB.Append("</color>");
        }
        else if (minigameEndType == MinigameEndType.Lose)
        {
            passwordSB.Append($"<color=#{ColorUtility.ToHtmlStringRGB(manager.PasswordGameLoseColor)}>");
            passwordSB.Append(written);
            passwordSB.Append(Input.inputString.ToLower());

            int questionMarkNum = password.Length - (written.Length + 1);
            passwordSB.Insert(passwordSB.Length, "?", questionMarkNum);
            passwordSB.Append("</color>");
        }

        manager.PasswordGameTypeText.text = passwordSB.ToString();
    }
}