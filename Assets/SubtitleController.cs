using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using TMPro;

public class SubtitleController : MonoBehaviour
{
    static public bool isActive;
    public string[] subtitles;
    public EventInstance instance;
    private float lastValue = -1f;

    public TextMeshPro subs;

    private void Start()
    {
        subs = GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        instance.getParameterByName("SubtitleIndex", out float value);
        Debug.Log(value);

        if (value != lastValue)
        {
            lastValue = value;
            Debug.Log("Changed Subtitles");
            ChangeSub((int)value);
        }
    }

    void ShowSub()
    {

    }

    void HideSub()
    {

    }

    void ChangeSub(int value)
    {
        if (!isActive)
        {
            return;
        }

        subs.text = subtitles[value];
    }
}
