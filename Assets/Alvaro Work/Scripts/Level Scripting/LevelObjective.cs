using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelObjective : MonoBehaviour
{

    [Header("Level Objective Parameter")]
    [SerializeField] public bool levelHasTimer;
    //[SerializeField] public bool levelHasCoin;
    //[SerializeField] public bool levelHasKey;
    [SerializeField] public float levelTimer;
    //[Tooltip("Coin Amount Max will be automatically set by Coin Amount")]
    //[SerializeField] public int coinAmount;
    [Tooltip("Name it the same as the current scene name (WATCH FOR CAP)")]
    [SerializeField] public string currentLevelScene;
    [Tooltip("Name for next scene name (WATCH FOR CAP)")]
    [SerializeField] public string nextLevelScene;

    private int coinAmountMax;
    private int coinOriginalAmount;
    private float levelOriginalTimer;

    private bool hasTimerCompleted;
    private bool hasTimerStarted;
    private bool hasTimerStopped;

    private bool allCoinCollected;

    private bool hasKeyCollected;

    private FPSController player;
    private MeshCollider levelStartCollider;
    private MeshCollider levelEndCollider;

    GameObject objectiveText;

    private void Awake()
    {
        if (!levelHasTimer)
        {
            levelTimer = 9999f;
        }

        hasTimerCompleted = false;
        hasTimerStopped = false;
        hasTimerStarted = false;

        levelOriginalTimer = levelTimer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<FPSController>();
        levelStartCollider = GameObject.Find("LevelStartAreaCollision").GetComponent<MeshCollider>();
        levelEndCollider = GameObject.Find("LevelEndAreaCollision").GetComponent<MeshCollider>();
        objectiveText = GameObject.Find("TimerText");
    }


    // Update is called once per frame
    void Update()
    {
        if(hasTimerStarted)
        {
            StartCountdown();
        }
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
        //coinAmount = coinOriginalAmount;
    }


    public void ResetLevel()
    {
        SceneManager.LoadScene(currentLevelScene);
    }


    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevelScene);
    }

    private void StopCountdown()
    {
        hasTimerStopped = true;
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
        // Timer has reached zero
        else if (levelHasTimer && hasTimerCompleted)
        {
            player.currentState = FPSController.PlayerState.STATE_DEAD;
        }
        // Player reached the end of level before timer ran out
        else
        {
            return;
        }
    }



}
