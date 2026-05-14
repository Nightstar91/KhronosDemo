using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class IntroMonitor : MonoBehaviour
{
    [SerializeField] GameObject chamberText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

    }


    public void TurnOnMonitor()
    {
        chamberText.SetActive(true);
    }

}
