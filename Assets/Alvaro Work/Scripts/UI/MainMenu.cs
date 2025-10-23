using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BasicMenu
{
    [Header("For scene name watch out for capitalization!")]
    [SerializeField] public string sceneName = "LevelDemo";

    private BasicMenu basicMenu;

    private GameObject settingBackButton;

    private GameObject sensitivitySlider;
    private GameObject sensitivityText;
    private GameObject sensitivityLabel;

    private void Awake()
    {
        basicMenu = new BasicMenu();


        settingBackButton = GameObject.Find("SettingBackButton");

        sensitivitySlider = GameObject.Find("SensSlider");
        sensitivityText = GameObject.Find("SettingSensAmountText");
        sensitivityLabel = GameObject.Find("SettingSensLabel");
    }

    private void Start()
    {
        // For all setting related UI
        basicMenu.settingPanel.SetActive(false);

        settingBackButton.SetActive(false);
        basicMenu.exitGameConfirmationPanel.SetActive(false);
    }


    public void OpenSettingPanel()
    {
        basicMenu.playGameButton.SetActive(false);
        basicMenu.settingButton.SetActive(false);
        basicMenu.exitGameButton.SetActive(false);

        // For all setting related UI
        basicMenu.settingPanel.SetActive(true);
    }


    public void SettingBackButton()
    {

        basicMenu.playGameButton.SetActive(true);
        basicMenu.settingButton.SetActive(true);
        basicMenu.exitGameButton.SetActive(true);

        // For all setting related UI
        settingPanel.SetActive(false);
        settingBackButton.SetActive(false);
    }
}
