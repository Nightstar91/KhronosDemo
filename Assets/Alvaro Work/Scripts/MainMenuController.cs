using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("For scene name watch out for capitalization!")]
    [SerializeField] public string sceneName = "LevelDemo";
    

    private GameObject playButton;
    private GameObject settingButton;
    private GameObject settingBackButton;
    private GameObject exitButton;

    private GameObject sensitivitySlider;
    private GameObject settingPanel;
    private GameObject sensitivityText;
    private GameObject sensitivityLabel;

    private void Awake()
    {
        playButton = GameObject.Find("PlayButton");
        settingButton = GameObject.Find("SettingButton");
        settingBackButton = GameObject.Find("SettingBackButton");
        exitButton = GameObject.Find("ExitButton");

        settingPanel = GameObject.Find("SettingPanel");
        sensitivitySlider = GameObject.Find("SensSlider");
        sensitivityText = GameObject.Find("SettingSensAmountText");
        sensitivityLabel = GameObject.Find("SettingSensLabel");
    }

    private void Start()
    {
        // For all setting related UI
        settingPanel.SetActive(false);
        sensitivitySlider.SetActive(false);
        sensitivityText.SetActive(false);
        sensitivityLabel.SetActive(false);
        settingBackButton.SetActive(false);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(sceneName);
    }


    public void SettingButton()
    {
        playButton.SetActive(false);
        settingButton.SetActive(false);
        exitButton.SetActive(false);

        // For all setting related UI
        settingPanel.SetActive(true);
        sensitivitySlider.SetActive(true);
        sensitivityText.SetActive(true);
        sensitivityLabel.SetActive(true);
        settingBackButton.SetActive(true);
    }


    public void SettingBackButton()
    {

        playButton.SetActive(true);
        settingButton.SetActive(true);
        exitButton.SetActive(true);

        // For all setting related UI
        settingPanel.SetActive(false);
        settingBackButton.SetActive(false);
    }


    


    public void ExitButton()
    {
        Application.Quit();
    }
}
