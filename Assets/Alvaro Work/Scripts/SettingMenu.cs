using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] public float sensitivityAmount;

    public GameObject sensitivitySlider;
    public GameObject sensitivityText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sensitivitySlider = GameObject.Find("SensSlider");
        sensitivityText = GameObject.Find("SettingSensAmountText");
        sensitivityAmount = PlayerPrefs.GetFloat("Sensitivity", 2);

        sensitivitySlider.GetComponent<Slider>().value = sensitivityAmount;
    }


    public void UpdateSensitivity()
    {
        // Assigning the value of sensitivity based on the value of the slider
        sensitivityAmount = Convert.ToInt32(sensitivitySlider.GetComponent<Slider>().value);

        // Displaying the text based on the value
        sensitivityText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", sensitivityAmount);

        SaveSensitivity();
    }


    [ContextMenu("Save")]
    private void SaveSensitivity()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivityAmount);
    }
}
