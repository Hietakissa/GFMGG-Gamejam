using HietakissaUtils.QOL;
using System.Collections;
using UnityEngine;

public class UIManager : Manager
{
    public static UIManager Instance;

    [SerializeField] CanvasGroup fadeGroup;
    [SerializeField] float fadeInDuration;
    [SerializeField] float fadeOutDuration;
    [SerializeField] float fadeWaitDuration;

    public override void Initialize()
    {
        Instance = this;
    }


    public void FadeIn() => StartCoroutine(FadeInCor());
    public IEnumerator FadeInCor()
    {
        float time = 0f;
        float fadeSpeed = 1f / fadeInDuration;
        while (time < 1f)
        {
            time += fadeSpeed * Time.deltaTime;
            fadeGroup.alpha = time;
            yield return null;
        }
    }

    public void FadeOut() => StartCoroutine(FadeOutCor());
    public IEnumerator FadeOutCor()
    {
        float time = 0f;
        float fadeSpeed = 1f / fadeOutDuration;
        while (time < 1f)
        {
            time += fadeSpeed * Time.deltaTime;
            fadeGroup.alpha = 1f - time;
            yield return null;
        }
    }

    public IEnumerator FadeWaitCor()
    {
        yield return QOL.WaitForSeconds.Get(fadeWaitDuration);
    }
}