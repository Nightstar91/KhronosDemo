using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class LevelObjective : MonoBehaviour
{
    public enum HonorValue
    {
        Neutral,
        Good,
        Bad
    }

    [Header("Level Objective Parameters")]
    public bool levelHasTimer;
  
    //[SerializeField] private static string currentLevelScene;

    [Header("Dialogue Parameters")]
    [SerializeField] private int levelSelectValue; // Assign per level in Inspector
    [SerializeField] private EventReference levelDialogue;
    private EventInstance dialogueInstance;
    private HonorValue honorValue = HonorValue.Neutral; // 0 = Neutral, 1 = Good, 2 = Bad

    [Header("Parameters for Level Timer")]
    [SerializeField] public float levelTimer;
    private bool isTimerRunning;

    public float[] thirdplaceTimeData;

    public float thirdPlaceTime;
    public float thirdPlaceLapOneTime;
    public float thirdPlaceLapTwoTime;
    public float thirdPlaceLapThreeTime;
    public float thirdPlaceLapFourTime;

    public Vector3 playerSpawnPoint;

    private FPSController player;
    private Leaderboard leaderboard;
    private GameObject startTrigger;
    private GameObject endTrigger;

    [SerializeField] TextMeshProUGUI objectiveText;
    GameObject objectiveHud;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<FPSController>();
        leaderboard = GetComponent<Leaderboard>(); 
        startTrigger = GameObject.Find("LevelStartTrigger");
        endTrigger = GameObject.Find("LevelEndTrigger");
        playerSpawnPoint = GameObject.Find("Player").transform.position; // Get the player position as soon as the scene loads

        objectiveText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        objectiveHud = GameObject.Find("ObjectiveHUD");

        thirdplaceTimeData = new float[5];
        ;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (levelHasTimer)
        {
            isTimerRunning = false;

            thirdPlaceTime = leaderboard.GetThirdPlace();
        }
        else
        {
            objectiveHud.gameObject.SetActive(false); // turning off objective ui if there is no objective
            levelTimer = -1;
            isTimerRunning = false;
        }

        PlayDialogue(); //play once at level load
    }


    // Update is called once per frame
    void Update()
    {
        LevelTimer();
        DisplayObjectiveUI();
    }


    private void LevelTimer()
    {
        if (isTimerRunning)
        {
            levelTimer += 1 * Time.deltaTime;
        }

        else
        {
            return;
        }
    }


    public void ResetLevelTimer()
    {
        levelTimer = 0f;

        isTimerRunning = false;
    }


    private void StopCountdown()
    {
        isTimerRunning = false;
    }


    private void BeginCountdown()
    {
        isTimerRunning = true;
    }


    private void DisplayObjectiveUI()
    {
        if (levelHasTimer)
        {
            objectiveText.text = string.Format("{0:F2}", levelTimer);
        }

        return;
    }


    public void TriggerLevelStart()
    {
        BeginCountdown();
    }


    public void TriggerLevelEnd()
    {


        //StopCountdown();
        //Honor();
        //PlayDialogue(); //play again after honor is determined
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



    public void Honor()
    {
        Debug.Log("Honor function called");
        if (levelTimer < thirdPlaceTime)
        {
            honorValue = HonorValue.Good; // Good
            Debug.Log("Good Honor");
            return;
        }
        else if (levelTimer == thirdPlaceTime)
        {
            Debug.Log("Neutral Honor");
            honorValue = HonorValue.Neutral; // Neutral
            return;
        }
        else
        {
            Debug.Log("Bad Honor");
            honorValue = HonorValue.Bad; // Bad
        }
       
       
    }


    private void PlayDialogue()
    {
        int honorNumber = (int)honorValue;

        dialogueInstance = RuntimeManager.CreateInstance(levelDialogue);

        dialogueInstance.setParameterByName("Honor", honorNumber);
        dialogueInstance.setParameterByName("LevelSelect", levelSelectValue);

        dialogueInstance.start();
        dialogueInstance.release();
    }
}
