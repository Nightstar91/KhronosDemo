using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SubtitleController : MonoBehaviour
{
    static public bool isActive;
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
    private bool paused = false;
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

        // Given the parameter can only be the numbers 1-4,
        // we use a switch and the private boolean to determine whether this is the beginning or ending dialogue.
        // The boolean should be set to false by default.
        switch (subtitleType)
        {
            default:
                break;
            case 1:
                if (!used)
                {
                    Type = 1;
                    used = true;
                }
                else
                {
                    Type = 2;
                    used = false;
                }
                break;
            case 2:
                if (!used)
                {
                    Type = 3;
                    used = true;
                }
                else
                {
                    Type = 4;
                    used = false;
                }
                break;
            case 3:
                if (!used)
                {
                    Type = 5;
                    used = true;
                }
                else
                {
                    Type = 6;
                    used = false;
                }
                break;
            case 4:
                if (!used)
                {
                    Type = 7;
                    used = true;
                }
                else
                {
                    Type = 8;
                    used = false;
                }
                break;
        }

        // Now we use our new variable to start the coroutine.
        switch (Type)
        {
            default:
                break;
            case 1:
                StartCoroutine(TriggerSubtitles(Tutorial1Start));
                break;
            case 2:
                StartCoroutine(TriggerSubtitles(Tutorial1End));
                break;
            case 3:
                StartCoroutine(TriggerSubtitles(Tutorial2Start));
                break;
            case 4:
                StartCoroutine(TriggerSubtitles(Tutorial2End));
                break;
            case 5:
                StartCoroutine(TriggerSubtitles(Tutorial3Start));
                break;
            case 6:
                StartCoroutine(TriggerSubtitles(Tutorial3End));
                break;
            case 7:
                StartCoroutine(TriggerSubtitles(Tutorial4Start));
                break;
            case 8:
                StartCoroutine(TriggerSubtitles(Tutorial4End));
                break;
        }
    }

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
    }

    IEnumerator FadeCanvasGroup(CanvasGroup group, float duration, bool fadeIn)
    {
        float startAlpha = group.alpha;
        float endAlpha = fadeIn ? 1f : 0f;

        float time = 0f;

        while (time < duration)
        {
            // =========================
            // ADDED
            // Freeze fade while paused
            // =========================
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
        StopCoroutine("FadeOutRoutine");

        StartCoroutine(FadeCanvasGroup(canvasGroup, 0.25f, true));

        subs.text = text;
    }
    public void PauseSubtitles()
    {
        paused = true;
    }

    public void ResumeSubtitles()
    {
        paused = false;
        if (!string.IsNullOrEmpty(subs.text))
        {
            canvasGroup.alpha = 1f;
        }
    }
    public void HideSubtitle()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, false));
    }
}
