using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    public enum CurrentObjective
    {
        None,
        Timer,
        TimerAndCoin
    }

    [Header("Level Objective Parameter")]
    [SerializeField] public CurrentObjective objectiveType;
    [SerializeField] public float levelTimer;
    [SerializeField] public int coinAmount;
    [Tooltip("Coin Amount Max will be automatically set by Coin Amount")]
    public int coinAmountMax;

    private void Awake()
    {
        coinAmountMax = coinAmount;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    private void TrackCoin()
    {

    }
}
