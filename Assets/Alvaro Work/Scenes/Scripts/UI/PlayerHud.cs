using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMODUnity;


public class PlayerHud : BasicMenu
{
    public string sceneName = "Main Menu";
    public FPSController player;
    private SettingMenu settingMenu;
    private Slider speedoSlider;


    public GameObject pauseMenu;
    public GameObject mainMenuButton;
    public GameObject resumeButton;
    public GameObject resultPanel;
    public GameObject subtitlePanel;
    public bool isPaused = false;
    public bool isSubtitleEnabled;

    private SubtitleController subtitleController;

    public override void Awake()
    {
        base.Awake();
        pauseMenu = GameObject.Find("Pausemenu");
        mainMenuButton = GameObject.Find("MainMenuButton");
        resumeButton = GameObject.Find("ResumeButton");
        subtitlePanel = GameObject.Find("SubtitleUI");
        //resultPanel = GameObject.Find("ResultPanel");

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<FPSController>();
        settingMenu = GetComponent<SettingMenu>();
        speedoSlider = GameObject.Find("SpeedoSlider").GetComponent<Slider>();


        //resultPanel.SetActive(false);
        settingPanel.SetActive(false);
        exitGameConfirmationPanel.SetActive(false);
        pauseMenu.SetActive(false);
        settingBackButton.SetActive(false);

        player.moveAction.Enable();
        player.jumpAction.Enable();
        player.slideAction.Enable();

        subtitleController = FindObjectOfType<SubtitleController>();

        //if (settingMenu.GetSubtitleCheck())
        //    OpenSubtitlePanel();
        //else
        //    CloseSubtitlePanel();
    }

    // Update is called once per frame
    void Update()
    {
        

        UpdateSpeedometer();
    }


    public void UpdateSpeedometer()
    {
        speedoSlider.value = player.GetVelocity();
    }
        

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        RuntimeManager.StudioSystem.setParameterByName("Pause", 1f);
        if (FMODTriggerEvent.ActiveDialogue != null)
        {
            FMODTriggerEvent.ActiveDialogue.PauseDialogue();
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        player.moveAction.Disable();
        player.jumpAction.Disable();
        player.slideAction.Disable();

        isPaused = true;
    }


    public void ResumeGame()
    {
        // Updating the Sensitivity
        player.lookSpeedX = settingMenu.GetSensitivity();
        player.lookSpeedY = settingMenu.GetSensitivity();

        // Updating the FOV
        player.playerCamera.fieldOfView = settingMenu.GetFOV();

        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        RuntimeManager.StudioSystem.setParameterByName("Pause", 0f);
        if (FMODTriggerEvent.ActiveDialogue != null)
        {
            FMODTriggerEvent.ActiveDialogue.ResumeDialogue();
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;

        player.moveAction.Enable();
        player.jumpAction.Enable();
        player.slideAction.Enable();
    }

    public override void OpenSetting()
    {
        settingPanel.SetActive(true);

        settingButton.SetActive(false);
        exitGameButton.SetActive(false);
        settingBackButton.SetActive(true);
    }

    public override void CloseSetting()
    {
        settingPanel.SetActive(false);

        settingButton.SetActive(true);
        exitGameButton.SetActive(true);
        settingBackButton.SetActive(false);
    }

    public override void OpenExitConfirmation()
    {
        exitConfirmCheck = true;
        exitGameConfirmationPanel.SetActive(true);
        resumeButton.SetActive(false);
        exitGameButton.SetActive(false);
    }


    public override void CloseExitConfirmation()
    {
        exitConfirmCheck = true;
        exitGameConfirmationPanel.SetActive(false);
        resumeButton.SetActive(true);
        exitGameButton.SetActive(true);
    }

    //public void OpenSubtitlePanel()
    //{
    //    subtitlePanel.SetActive(true);

    //    if (subtitleController != null)
    //        subtitleController.EnableSubtitles();
    //}


    //public void CloseSubtitlePanel()
    //{
    //    subtitlePanel.SetActive(false);

    //    if (subtitleController != null)
    //        subtitleController.DisableSubtitles();
    //}


    public void OpenResultPanel(bool failed)
    {
        if(failed)
        {
            resultPanel.SetActive(true);
        }
    }


    public void CloseResultPanel()
    {
        resultPanel.SetActive(false);
    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }
}
