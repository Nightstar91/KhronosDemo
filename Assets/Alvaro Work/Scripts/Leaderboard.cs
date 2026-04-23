using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Leaderboard : MonoBehaviour
{

    // Setup for struct
    private const int TOTAL_ENTRIES = 4;
    private const int TOTAL_LAPS = 4;

    string[] placePrefix = { "1st", "2nd", "3rd", "user" };
    string[] lapNames = { "First", "Second", "Third", "Fourth" };

    public LeaderboardEntry[] leaderboard = new LeaderboardEntry[TOTAL_ENTRIES];


    // Related to the player score/time
    public float currentTime;
    public float[] currentLapTime = new float[4];
    public string playerInput = string.Empty;


    [SerializeField] TextMeshProUGUI firstPlaceTimeText;
    [SerializeField] TextMeshProUGUI firstPlaceLapOneTimeText;
    [SerializeField] TextMeshProUGUI firstPlaceLapTwoTimeText;
    [SerializeField] TextMeshProUGUI firstPlaceLapThreeTimeText;
    [SerializeField] TextMeshProUGUI firstPlaceLapFourTimeText;
    [SerializeField] TextMeshProUGUI firstPlaceNameText;

    [SerializeField] TextMeshProUGUI secondPlaceTimeText;
    [SerializeField] TextMeshProUGUI secondPlaceLapOneTimeText;
    [SerializeField] TextMeshProUGUI secondPlaceLapTwoTimeText;
    [SerializeField] TextMeshProUGUI secondPlaceLapThreeTimeText;
    [SerializeField] TextMeshProUGUI secondPlaceLapFourTimeText;
    [SerializeField] TextMeshProUGUI secondPlaceNameText;

    [SerializeField] TextMeshProUGUI thirdPlaceTimeText;
    [SerializeField] TextMeshProUGUI thirdPlaceLapOneTimeText;
    [SerializeField] TextMeshProUGUI thirdPlaceLapTwoTimeText;
    [SerializeField] TextMeshProUGUI thirdPlaceLapThreeTimeText;
    [SerializeField] TextMeshProUGUI thirdPlaceLapFourTimeText;
    [SerializeField] TextMeshProUGUI thirdPlaceNameText;

    [System.Serializable]
    public struct LeaderboardEntry
    {
        public string name;
        public float totalTime;
        public float[] lapTimes; // length = number of laps
    }

    private void Start()
    {
        LoadLeaderBoard();
        DisplayCurrentLeaderboard();
    }


    public void LoadLeaderBoard()
    {
        for (int i = 0; i < TOTAL_ENTRIES; i++)
        {
            leaderboard[i].name = PlayerPrefs.GetString($"{placePrefix[i]}PlaceName", $"Dev{i + 1}");
            leaderboard[i].totalTime = PlayerPrefs.GetFloat($"{placePrefix[i]}PlaceTimer", 0f);
            leaderboard[i].lapTimes = new float[TOTAL_LAPS];

            for (int lap = 0; lap < TOTAL_LAPS; lap++)
            {
                string key = $"{placePrefix[i]}{lapNames[lap]}LapTime";
                leaderboard[i].lapTimes[lap] = PlayerPrefs.GetFloat(key, 0f);
            }
        }
    }


    public void Bubblesort(int numberOfEntries)
    {
        for (int i = 0; i < numberOfEntries - 1; i++)
        {
            for (int j = 0; j < numberOfEntries - 1 - i; j++)
            {
                if (leaderboard[j].totalTime > leaderboard[j + 1].totalTime)
                {
                    LeaderboardEntry temp = leaderboard[j];
                    leaderboard[j]        = leaderboard[j + 1];
                    leaderboard[j + 1]    = temp;
                }
            }
        }
    }


    public void DisplayCurrentLeaderboard()
    {
        //// 1st place
        //firstPlaceTimeText.text = 
        //firstPlaceLapOneTimeText.text = 
        //firstPlaceLapTwoTimeText.text = 
        //firstPlaceLapThreeTimeText.text = 
        //firstPlaceLapFourTimeText.text = 
        //firstPlaceNameText.text = 

        //// 2nd place
        //secondPlaceTimeText.text = 
        //secondPlaceLapOneTimeText.text = 
        //secondPlaceLapTwoTimeText.text = 
        //secondPlaceLapThreeTimeText.text = 
        //secondPlaceLapFourTimeText.text = 
        //secondPlaceNameText.text = 

        //// 3rd place
        //thirdPlaceTimeText.text = 
        //thirdPlaceLapOneTimeText.text = 
        //thirdPlaceLapTwoTimeText.text = 
        //thirdPlaceLapThreeTimeText.text = 
        //thirdPlaceLapFourTimeText.text = 
        //thirdPlaceNameText.text = 
    }


    public void SetCurrentHighscore()
    {

    }


    public string ConvertFloatToTime(float conversion)
    {

        string output;

        int minutes = Mathf.FloorToInt(conversion / 60);
        int seconds = Mathf.FloorToInt(conversion % 60);

        output = string.Format("{0}:{1:D2}", minutes, seconds);

        return output;
    }




    private void ResetFloatArray(float[] arrayToReset)
    {
        for(int i = 0; arrayToReset.Length > i; i++)
        {
            arrayToReset[i] = 0;
        }
    }


    private void AddLapTime(int positionInArray, float lapTime)
    {
        if(0 >= positionInArray && positionInArray <= currentLapTime.Length - 1)
        {
            currentLapTime[positionInArray] = lapTime;  
        }
        else
        {
            Debug.Log("NOT IN RANGE OF ARRAY (0-3)");
        }
    }
}
