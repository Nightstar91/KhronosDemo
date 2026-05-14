using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class IntroMonitor : MonoBehaviour
{
    [SerializeField] TextMeshPro chamberText;
    [SerializeField] UnityEvent turnOnEvent; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chamberText = GetComponentInChildren<TextMeshPro>();

        chamberText.gameObject.SetActive(false);
    }


    public void TurnOnMonitor()
    {
        chamberText.gameObject.SetActive(true);
    }

}
