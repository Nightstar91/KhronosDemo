using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    // Sensitivity
    [SerializeField] public float sensitivityAmount;
    public GameObject sensitivitySlider;
    public GameObject sensitivityText;

    // FOV
    [SerializeField] public float fovAmount;
    public GameObject fovSlider;
    public GameObject fovText;

    //Subtitle
    [SerializeField] public int isSubtitleEnable; // 0 = false, 1 = true
    public Toggle subtitleToggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sensitivitySlider = GameObject.Find("SensSlider");
        sensitivityText = GameObject.Find("SettingSensAmountText");
        sensitivityAmount = PlayerPrefs.GetFloat("Sensitivity", 0.02f) ;

        fovSlider = GameObject.Find("FOVSlider");
        fovText = GameObject.Find("SettingFOVAmountText");
        fovAmount = PlayerPrefs.GetFloat("Fov", 70);

        subtitleToggle = GameObject.Find("SubtitleCheck").GetComponent<Toggle>();


        sensitivityText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", sensitivityAmount);
        fovText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", fovAmount);

        sensitivitySlider.GetComponent<Slider>().value = sensitivityAmount * 100;
        fovSlider.GetComponent<Slider>().value = fovAmount; 
    }


    public void UpdateSensitivity()
    {
        // Assigning the value of sensitivity based on the value of the slider
        sensitivityAmount = Convert.ToInt32(sensitivitySlider.GetComponent<Slider>().value);

        sensitivityAmount = sensitivityAmount / 100;

        // Displaying the text based on the value
        sensitivityText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", (sensitivityAmount * 100));

        SaveSensitivity();
    }

    public void UpdateFov()
    {
        // Assigning the value of sensitivity based on the value of the slider
        fovAmount = Convert.ToInt32(fovSlider.GetComponent<Slider>().value);

        // Displaying the text based on the value
        fovText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", fovAmount);

        SaveFOV();
    }

    public void UpdateSubtitle()
    {
        if(subtitleToggle.isOn)
            isSubtitleEnable = 1;
        else 
            isSubtitleEnable = 0;

        SaveSubtitleCheck();
    }


    [ContextMenu("Save")]
    private void SaveSensitivity()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivityAmount);
    }


    [ContextMenu("Save")]
    private void SaveFOV()
    {
        PlayerPrefs.SetFloat("Fov", fovAmount);
    }
    

    [ContextMenu("Save")]
    private void SaveSubtitleCheck()
    {
        PlayerPrefs.SetInt("SubtitleBool", isSubtitleEnable);
    }


    public float GetSensitivity()
    {
        return sensitivityAmount;
    }

    public float GetFOV()
    {
        return fovAmount;
    }

    public bool GetSubtitleCheck()
    {
        switch(isSubtitleEnable)
        {
            case 0:
                return false;
            case 1:
                return true;

            default:
                return true;
        }
    }
}
