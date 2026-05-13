using System.Collections;
using TMPro;
using UnityEngine;

public class SubtitleController : MonoBehaviour
{
    public static bool isActive;

    [System.Serializable]
    public class SubtitleEntry
    {
        public float time;
        public string text;
    }

    public TMP_Text subs;
    public CanvasGroup canvasGroup;

    public SubtitleEntry[] Tutorial1Start;
    public SubtitleEntry[] Tutorial1End;
    public SubtitleEntry[] Tutorial2Start;
    public SubtitleEntry[] Tutorial2End;
    public SubtitleEntry[] Tutorial3Start;
    public SubtitleEntry[] Tutorial3End;
    public SubtitleEntry[] Tutorial4Start;
    public SubtitleEntry[] Tutorial4End;

    private bool used = false;

    // =========================
    // FIX: pause state
    // =========================
    private bool paused = false;

    // =========================
    // FIX: prevent coroutine stacking
    // =========================
    private Coroutine subtitleRoutine;

    private void Start()
    {
        subs = GetComponentInChildren<TMP_Text>();
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        subs.text = "";
    }

    public void StartSubs(int subtitleType)
    {
        int Type = 0;

        switch (subtitleType)
        {
            case 1:
                if (!used) { Type = 1; used = true; }
                else { Type = 2; used = false; }
                break;

            case 2:
                if (!used) { Type = 3; used = true; }
                else { Type = 4; used = false; }
                break;

            case 3:
                if (!used) { Type = 5; used = true; }
                else { Type = 6; used = false; }
                break;

            case 4:
                if (!used) { Type = 7; used = true; }
                else { Type = 8; used = false; }
                break;
        }

        // =========================
        // FIX: stop previous subtitle flow
        // =========================
        if (subtitleRoutine != null)
        {
            StopCoroutine(subtitleRoutine);
        }

        switch (Type)
        {
            case 1: subtitleRoutine = StartCoroutine(TriggerSubtitles(Tutorial1Start)); break;
            case 2: subtitleRoutine = StartCoroutine(TriggerSubtitles(Tutorial1End)); break;
            case 3: subtitleRoutine = StartCoroutine(TriggerSubtitles(Tutorial2Start)); break;
            case 4: subtitleRoutine = StartCoroutine(TriggerSubtitles(Tutorial2End)); break;
            case 5: subtitleRoutine = StartCoroutine(TriggerSubtitles(Tutorial3Start)); break;
            case 6: subtitleRoutine = StartCoroutine(TriggerSubtitles(Tutorial3End)); break;
            case 7: subtitleRoutine = StartCoroutine(TriggerSubtitles(Tutorial4Start)); break;
            case 8: subtitleRoutine = StartCoroutine(TriggerSubtitles(Tutorial4End)); break;
        }
    }

    // =========================
    // FIX: pause-safe timing system
    // =========================
    IEnumerator TriggerSubtitles(SubtitleEntry[] subtitles)
    {
        float elapsed = 0f;
        int index = 0;

        while (index < subtitles.Length)
        {
            if (paused)
            {
                yield return null;
                continue;
            }

            elapsed += Time.deltaTime;

            if (elapsed >= subtitles[index].time)
            {
                ShowSubtitle(subtitles[index].text);
                index++;
            }

            yield return null;
        }

        subtitleRoutine = null;
    }

    // =========================
    // FIX: pause-safe fade system
    // =========================
    IEnumerator FadeCanvasGroup(CanvasGroup group, float duration, bool fadeIn)
    {
        float startAlpha = group.alpha;
        float endAlpha = fadeIn ? 1f : 0f;

        float time = 0f;

        while (time < duration)
        {
            if (paused)
            {
                yield return null;
                continue;
            }

            time += Time.deltaTime;

            float t = time / duration;
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            yield return null;
        }

        group.alpha = endAlpha;
    }

    public void ShowSubtitle(string text)
    {
        // ensure fade-in always triggers correctly
        if (canvasGroup.alpha <= 0.01f)
        {
            StartCoroutine(FadeCanvasGroup(canvasGroup, 0.25f, true));
        }

        subs.text = text;
    }

    public void HideSubtitle()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, false));
    }

    // =========================
    // FIX: pause API
    // =========================
    public void PauseSubtitles()
    {
        paused = true;
    }

    public void ResumeSubtitles()
    {
        paused = false;

        // =========================
        // FIX: restore visibility if text exists
        // =========================
        if (!string.IsNullOrEmpty(subs.text))
        {
            canvasGroup.alpha = 1f;
        }
    }
}