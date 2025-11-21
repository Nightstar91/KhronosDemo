using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    public bool isPaused = false;

    public override void Awake()
    {
        base.Awake();
        pauseMenu = GameObject.Find("Pausemenu");
        mainMenuButton = GameObject.Find("MainMenuButton");
        resumeButton = GameObject.Find("ResumeButton");
        resultPanel = GameObject.Find("ResultPanel");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<FPSController>();
        settingMenu = GetComponent<SettingMenu>();
        speedoSlider = GameObject.Find("SpeedoSlider").GetComponent<Slider>();

        resultPanel.SetActive(false);
        settingPanel.SetActive(false);
        exitGameConfirmationPanel.SetActive(false);
        pauseMenu.SetActive(false);
        settingBackButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeedometer();
    }


    public void UpdateSpeedometer()
    {
        speedoSlider.value = player.characterController.velocity.magnitude;
    }
        

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        player.moveAction.Disable();
        player.jumpAction.Disable();
    }


    public void ResumeGame()
    {
        // Updating the Sensitivity
        player.lookSpeedX = settingMenu.GetSensitivity();
        player.lookSpeedY = settingMenu.GetSensitivity();

        // Updating the Sensitivity
        player.playerCamera.fieldOfView = settingMenu.GetFOV();

        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;

        player.moveAction.Enable();
        player.jumpAction.Enable();
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


    public void OpenResultPanel()
    {
        
    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }
}
