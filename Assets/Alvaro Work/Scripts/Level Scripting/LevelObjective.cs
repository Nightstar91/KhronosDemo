using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelObjective : MonoBehaviour
{
    public enum lvlLength
    {
        None,
        Short,
        Medium,
        Long,
        XtraLong
    }

    [Header("Level Lengths")]
    [SerializeField] public lvlLength[] lvlLengths;

    [Header("Level Objective Parameter")]
    [SerializeField] public bool levelHasTimer;
    //[SerializeField] public bool levelHasCoin;
    //[SerializeField] public bool levelHasKey;
    [SerializeField] public float levelTimer;
    //[Tooltip("Coin Amount Max will be automatically set by Coin Amount")]
    //[SerializeField] public int coinAmount;

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
    private LevelTransition lvlTrans;

    TMP_Text objText;

    private void Awake()
    {
        hasTimerCompleted = false;
        hasTimerStopped = false;
        hasTimerStarted = true;

        levelOriginalTimer = levelTimer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<FPSController>();
        //levelStartCollider = GameObject.Find("LevelStartAreaCollision").GetComponent<MeshCollider>();
        //levelEndCollider = GameObject.Find("LevelEndAreaCollision").GetComponent<MeshCollider>();
        objText = GameObject.Find("TimerText").GetComponent<TMP_Text>();
        lvlTrans = this.GetComponent<LevelTransition>();

        switch (lvlLengths[lvlTrans.FindScene()])
        {
            case lvlLength.None:
                levelHasTimer = false;
                break;
            case lvlLength.Short:
                levelTimer = 120f;
                break;
            case lvlLength.Medium:
                levelTimer = 300f;
                break;
            case lvlLength.Long:
                levelTimer = 6000f;
                break;
            case lvlLength.XtraLong:
                levelTimer = 9999f;
                break;
        }

        if (!levelHasTimer)
        {
            levelTimer = 9999f;
        }
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

    private void StopCountdown()
    {
        hasTimerStopped = true;
    }


    private bool LevelCountdown()
    {
        if (levelTimer >= 0)
        {
            levelTimer -= 1 * Time.deltaTime;
            objText.text = GetTimerString();
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
