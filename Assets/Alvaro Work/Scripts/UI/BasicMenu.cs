using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicMenu : MonoBehaviour 
{
    public string mainMenuScene;
    public string loadLevelScene;
    private LevelTransition lvlTrans;

    public bool exitConfirmCheck;

    public GameObject playGameButton;
    public GameObject resumeGameButton;
    public GameObject exitGameButton;
    public GameObject settingButton;
    public GameObject settingBackButton;

    public GameObject exitConfirmButton;
    public GameObject exitCancelButton;

    public GameObject settingPanel;
    public GameObject exitGameConfirmationPanel;

    public virtual void Awake()
    {
        lvlTrans = GetComponentInParent<LevelTransition>();

        mainMenuScene = lvlTrans.allScenes[0];
        loadLevelScene = lvlTrans.allScenes[1];

        exitConfirmCheck = false;

        playGameButton = GameObject.Find("PlayButton");
        resumeGameButton = GameObject.Find("ResumeButton");
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

    public void ResumeGame()
    {
        SceneManager.LoadScene(lvlTrans.allScenes[PlayerPrefs.GetInt("Scene")]);
    }

    public virtual void CloseSetting()
    {
        settingPanel.SetActive(false);

        playGameButton.SetActive(true);
        settingButton.SetActive(true);
        exitGameButton.SetActive(true);
        settingBackButton.SetActive(false);
    }


    public virtual void OpenSetting()
    {
        settingPanel.SetActive(true);

        playGameButton.SetActive(false);
        settingButton.SetActive(false);
        exitGameButton.SetActive(false);
        settingBackButton.SetActive(true);
    }


    public virtual void OpenExitConfirmation()
    {
        exitConfirmCheck = true;
        exitGameConfirmationPanel.SetActive(true);
        exitGameButton.SetActive(false);
    }


    public virtual void CloseExitConfirmation()
    {
        exitConfirmCheck = false;
        exitGameConfirmationPanel.SetActive(false);
        exitGameButton.SetActive(true);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
