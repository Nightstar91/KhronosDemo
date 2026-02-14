using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BasicMenu
{
    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // For all setting related UI
        settingPanel.SetActive(false);
        exitGameConfirmationPanel.SetActive(false);

        settingBackButton.SetActive(false);
        if(PlayerPrefs.GetInt("Scene", 1) == 1)
        {
            resumeGameButton.SetActive(false);
        }

    }


    /*public void OpenSettingPanel()
    {
        playGameButton.SetActive(false);
        settingButton.SetActive(false);
        exitGameButton.SetActive(false);

        // For all setting related UI
        settingPanel.SetActive(true);
    }


    public void SettingBackButton()
    {

        playGameButton.SetActive(true);
        settingButton.SetActive(true);
        exitGameButton.SetActive(true);

        // For all setting related UI
        settingPanel.SetActive(false);
        settingBackButton.SetActive(false);
    }*/
}
