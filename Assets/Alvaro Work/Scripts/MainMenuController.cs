using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("For scene name watch out for capitalization!")]
    [SerializeField] public string sceneName = "LevelDemo";

    public Boolean exitConfirmCheck;

    private GameObject playButton;
    private GameObject settingButton;
    private GameObject settingBackButton;
    private GameObject exitButton;

    private GameObject sensitivitySlider;
    private GameObject settingPanel;
    private GameObject exitConfirmationPanel;
    private GameObject sensitivityText;
    private GameObject sensitivityLabel;

    private void Awake()
    {
        exitConfirmCheck = false;

        playButton = GameObject.Find("PlayButton");
        settingButton = GameObject.Find("SettingButton");
        settingBackButton = GameObject.Find("SettingBackButton");
        exitButton = GameObject.Find("ExitButton");

        settingPanel = GameObject.Find("SettingPanel");
        exitConfirmationPanel = GameObject.Find("ExitGameConfirmationPanel");
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
        exitConfirmationPanel.SetActive(false);
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
        if(!exitConfirmCheck)
        {
            exitConfirmCheck = true;
            exitConfirmationPanel.SetActive(true);
            exitButton.SetActive(false);
        }
    }


    public void ConfirmExitButton()
    {
        Application.Quit();
    }


    public void CancelExitButton()
    {
        exitConfirmCheck = false;
        exitConfirmationPanel.SetActive(false);
        exitButton.SetActive(true);
    }
}
