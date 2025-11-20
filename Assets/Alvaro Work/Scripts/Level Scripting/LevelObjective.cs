using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelObjective : MonoBehaviour
{

    [Header("Level Objective Parameter")]
    [SerializeField] public bool hasTimer;
    [SerializeField] public bool hasCoin;
    [SerializeField] public bool hasKey;
    [SerializeField] public float levelTimer;
    [Tooltip("Coin Amount Max will be automatically set by Coin Amount")]
    [SerializeField] public int coinAmount;
    [Tooltip("Name it the same as the current scene name (WATCH FOR CAP)")]
    [SerializeField] public string currentLevelScene;
    [Tooltip("Name for next scene name (WATCH FOR CAP)")]
    [SerializeField] public string nextLevelScene;

    private int coinAmountMax;
    private int coinOriginalAmount;
    private float levelOriginalTimer;

    private bool hasTimerCompleted;

    private FPSController player;
    private MeshCollider levelStartCollider;
    private MeshCollider levelEndCollider;



    private void Awake()
    {
        coinAmountMax = coinAmount;
        levelOriginalTimer = levelTimer;
        hasTimerCompleted = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<FPSController>();
        levelStartCollider = GameObject.Find("LevelStartAreaCollision").GetComponent<MeshCollider>();
        levelEndCollider = GameObject.Find("LevelEndAreaCollision").GetComponent<MeshCollider>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void ResetLevelTimer()
    {
        levelTimer = levelOriginalTimer;
    }


    private string GetTimerString()
    {
        return string.Format("{0}",levelTimer);
    }


    public void ResetCoin()
    {
        coinAmount = coinOriginalAmount;
    }


    public void ResetLevel()
    {
        SceneManager.LoadScene(currentLevelScene);
    }


    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevelScene);
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
}
