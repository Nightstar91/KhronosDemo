using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Animations;
using System.Diagnostics;

public class LevelObjective : MonoBehaviour
{

    [Header("Level Objective Parameters")]
    [SerializeField] public bool levelHasTimer;
    [SerializeField] public bool levelHasCoin;

    [SerializeField] private static string currentLevelScene;
    
    [Header("Parameters for Level Timer")]
    [SerializeField] public float levelTimer;
    private float levelOriginalTimer;
    private bool hasTimerCompleted; // This flag for failure state 
    private bool hasTimerStopped; // This flag for success state
    private bool isTimerRunning;

    //[Tooltip("Coin Amount Max will be automatically set by Coin Amount")]
    //[SerializeField] public int coinAmount;
    //private int coinAmountMax;
    //private int coinOriginalAmount;
    //private bool allCoinCollected;


    private FPSController player;

    [SerializeField] TextMeshProUGUI objectiveText;
    GameObject objectiveHud;

    private void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        currentLevelScene = scene.name;

        objectiveText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        objectiveHud = GameObject.Find("ObjectiveHUD");

        if (levelHasTimer)
        {
            hasTimerCompleted = false;
            hasTimerStopped = false;
            isTimerRunning = true;
            levelOriginalTimer = levelTimer;
        }
        else
        {
            objectiveHud.gameObject.SetActive(false); // turning off objective ui if there is no objective
            levelTimer = -1;
            isTimerRunning = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<FPSController>();
        
    }


    // Update is called once per frame
    void Update()
    {
        if(levelHasTimer)
        {
            StartCountdown();
        }
        else
        {
            return;
        }

        DisplayObjectiveUI();
    }


    public void ResetLevelTimer()
    {
        levelTimer = levelOriginalTimer;
    }


    private string GetTimerString()
    {
        return string.Format("{0:F2}", levelTimer);
    }


    public void ResetCoin()
    {
        
    }


    private void StopCountdown()
    {
        hasTimerStopped = true;
    }


    public static void RestartScene()
    {
        SceneManager.LoadScene(LevelObjective.currentLevelScene);
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


    private void StartCountdown()
    {
        // Timer is ticking
        if (!hasTimerCompleted && !hasTimerStopped)
        {
            hasTimerCompleted = LevelCountdown();
        }
        // Timer reached 0, player has failed
        else
        {
            player.hasFailed = true;
        }
    }


    private void DisplayObjectiveUI()
    {
        if (levelHasTimer)
        {
            objectiveText.text = string.Format("TIMER: {0:F2}", levelTimer);
        }

        return;
    }
}
