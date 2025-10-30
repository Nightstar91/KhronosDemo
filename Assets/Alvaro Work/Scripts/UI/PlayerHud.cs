using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerHud : BasicMenu
{
    public string sceneName = "Main Menu";
    public OLDFPSController playerController;
    private SettingMenu settingMenu;
    private Slider speedoSlider;


    public GameObject pauseMenu;
    public GameObject mainMenuButton;
    public GameObject resumeButton;
    public bool isPaused = false;

    public virtual void Awake()
    {
        base.Awake();
        pauseMenu = GameObject.Find("Pausemenu");
        mainMenuButton = GameObject.Find("MainMenuButton");
        resumeButton = GameObject.Find("ResumeButton");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingMenu = GetComponent<SettingMenu>();
        speedoSlider = GameObject.Find("SpeedoSlider").GetComponent<Slider>();

        settingPanel.SetActive(false);
        exitGameConfirmationPanel.SetActive(false);
        pauseMenu.SetActive(false);
        settingBackButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeedometer();

        if(playerController.pauseAction.triggered)
        {
            if(!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }


    public void UpdateSpeedometer()
    {
        speedoSlider.value = playerController.characterController.velocity.magnitude;
    }
        


    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.moveAction.Disable();

        playerController.jumpAction.Disable();
    }


    public void ResumeGame()
    {
        // Updating the Sensitivity
        playerController.lookSpeedX = settingMenu.GetSensitivity();
        playerController.lookSpeedY = settingMenu.GetSensitivity();

        // Updating the Sensitivity
        playerController.playerCamera.fieldOfView = settingMenu.GetFOV();

        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController.moveAction.Enable();

        playerController.jumpAction.Enable();
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


    public void GoToMainMenu()
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }
}
