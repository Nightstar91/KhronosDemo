using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicMenu : MonoBehaviour 
{
    public string mainMenuScene;
    public string loadLevelScene;

    public bool exitConfirmCheck;

    public GameObject playGameButton;
    public GameObject exitGameButton;
    public GameObject settingButton;
    public GameObject settingBackButton;

    public GameObject exitConfirmButton;
    public GameObject exitCancelButton;

    public GameObject settingPanel;
    public GameObject exitGameConfirmationPanel;

    public virtual void Awake()
    {
        mainMenuScene = "Main Menu";
        loadLevelScene = "Level 1";

        exitConfirmCheck = false;

        playGameButton = GameObject.Find("PlayButton");
        exitGameButton = GameObject.Find("ExitButton");
        settingButton = GameObject.Find("SettingButton");
        settingBackButton = GameObject.Find("SettingBackButton");

        exitConfirmButton = GameObject.Find("ExitGameConfirmButton");
        exitCancelButton = GameObject.Find("ExitGameCancelButton");

        settingPanel = GameObject.Find("SettingPanel");
        exitGameConfirmationPanel = GameObject.Find("ExitGameConfirmationPanel");
    }


    public void PlayGame()
    {
        SceneManager.LoadScene(loadLevelScene);
    }


    public void CloseSetting()
    {
        settingPanel.SetActive(false);

        playGameButton.SetActive(true);
        settingButton.SetActive(true);
        exitGameButton.SetActive(true);
        settingBackButton.SetActive(false);
    }


    public void OpenSetting()
    {
        settingPanel.SetActive(true);

        playGameButton.SetActive(false);
        settingButton.SetActive(false);
        exitGameButton.SetActive(false);
        settingBackButton.SetActive(true);
    }


    public void OpenExitConfirmation()
    {
        exitConfirmCheck = true;
        exitGameConfirmationPanel.SetActive(true);
        exitGameButton.SetActive(true);
    }


    public void CloseExitConfirmation()
    {
        exitConfirmCheck = false;
        exitGameConfirmationPanel.SetActive(false);
        exitGameButton.SetActive(false);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
