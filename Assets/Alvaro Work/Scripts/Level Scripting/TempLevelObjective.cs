using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Animations;

public class TempLevelObjective : MonoBehaviour
{

    [Header("Level Objective Parameters")]
    //[SerializeField] public bool levelHasCoin;
    public bool levelHasTimer;

    [SerializeField] private static string currentLevelScene;

    [Header("Parameters for Level Timer")]
    [SerializeField] public float levelTimer;
    public float levelOriginalTimer;
    private bool hasTimerCompleted; // This flag for failure state 
    private bool hasTimerStopped; // This flag for success state
    private bool isTimerRunning;

    public Vector3 playerSpawnPoint;

    //[Tooltip("Coin Amount Max will be automatically set by Coin Amount")]
    //[SerializeField] public int coinAmount;
    //private int coinAmountMax;
    //private int coinOriginalAmount;
    //private bool allCoinCollected;


    private FPSController player;
    private GameObject startTrigger;
    private GameObject endTrigger;

    [SerializeField] TextMeshProUGUI objectiveText;
    GameObject objectiveHud;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<FPSController>();
        startTrigger = GameObject.Find("LevelStartTrigger");
        endTrigger = GameObject.Find("EndStartTrigger");
        playerSpawnPoint = GameObject.Find("Player").transform.position; // Get the player position as soon as the scene loads

        objectiveText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        objectiveHud = GameObject.Find("ObjectiveHUD");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (levelHasTimer)
        {
            hasTimerCompleted = false;
            hasTimerStopped = false;
            isTimerRunning = false;
            levelOriginalTimer = levelTimer;
        }
        else
        {
            objectiveHud.gameObject.SetActive(false); // turning off objective ui if there is no objective
            levelTimer = -1;
            isTimerRunning = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (levelHasTimer && isTimerRunning && !hasTimerStopped)
        {
            TimerTracker();
        }
        else
        {
            return;
        }

        DisplayObjectiveUI();
    }


    private bool LevelCountdown()
    {
        if (levelTimer >= 0)
        {
            levelTimer -= 1 * Time.deltaTime;
            return false;
        }
        else
        {
            levelTimer = 0;
            return true;
        }
    }


    private void TimerTracker()
    {
        // Timer is ticking
        if (!hasTimerCompleted && !hasTimerStopped)
        {
            hasTimerCompleted = LevelCountdown();
        }

        else
        {
            player.hasFailed = true;
        }
    }


    public void ResetLevelTimer()
    {
        levelTimer = levelOriginalTimer;

        hasTimerCompleted = false;
        hasTimerStopped = false;
        isTimerRunning = false;
    }


    private void StopCountdown()
    {
        hasTimerStopped = true;
    }


    private void BeginCountdown()
    {
        isTimerRunning = true;
    }


    private void DisplayObjectiveUI()
    {
        if (levelHasTimer)
        {
            objectiveText.text = string.Format("TIMER: {0:F2}", levelTimer);
        }

        return;
    }


    public void TriggerLevelStart()
    {
        BeginCountdown();
    }


    public void TriggerLevelEnd()
    {
        StopCountdown();
    }


    public void TriggerRestart()
    {
        player.playerHud.CloseResultPanel();
        GameObject.Find("Player").transform.position = playerSpawnPoint;
        startTrigger.GetComponent<LevelTrigger>().ResetTrigger();
        endTrigger.GetComponent<LevelTrigger>().ResetTrigger();
        player.hasFailed = false;
        objectiveText.text = string.Format("OBJECTIVE");
        ResetLevelTimer();
        player.currentState = FPSController.PlayerState.STATE_IDLE;
    }
}
