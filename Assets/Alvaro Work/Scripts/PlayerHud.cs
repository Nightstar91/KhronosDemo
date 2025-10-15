using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerHud : MonoBehaviour
{
    [SerializeField] public string sceneName = "Main Menu";
    [SerializeField] public FPSController playerController;
    [SerializeField] private SettingMenu settingMenu;
    [SerializeField] public Slider speedoSlider;

    public GameObject pauseMenu;
    public bool isPaused = false;

    private void Awake()
    {
        pauseMenu = GameObject.Find("Pausemenu");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<FPSController>();
        settingMenu = GetComponent<SettingMenu>();
        speedoSlider = GameObject.Find("SpeedoSlider").GetComponent<Slider>();

        pauseMenu.SetActive(false);
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
        playerController.lookAction.Disable();
        playerController.jumpAction.Disable();
    }

    public void ResumeGame()
    {
        // Updating the Sensitivity
        playerController.lookSpeedX = settingMenu.GetSensitivity();
        playerController.lookSpeedY = settingMenu.GetSensitivity();

        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController.moveAction.Enable();
        playerController.lookAction.Enable();
        playerController.jumpAction.Enable();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
