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
        //1st place
        firstPlaceTimeText.text = ConvertFloatToTime(leaderboard[0].totalTime);
        firstPlaceLapOneTimeText.text = ConvertFloatToTime(leaderboard[0].lapTimes[0]);
        firstPlaceLapTwoTimeText.text = ConvertFloatToTime(leaderboard[0].lapTimes[1]);
        firstPlaceLapThreeTimeText.text = ConvertFloatToTime(leaderboard[0].lapTimes[2]);
        firstPlaceLapFourTimeText.text = ConvertFloatToTime(leaderboard[0].lapTimes[3]);
        firstPlaceNameText.text = leaderboard[0].name;

        // 2nd place
        secondPlaceTimeText.text = ConvertFloatToTime(leaderboard[1].totalTime);
        secondPlaceLapOneTimeText.text = ConvertFloatToTime(leaderboard[1].lapTimes[0]);
        secondPlaceLapTwoTimeText.text = ConvertFloatToTime(leaderboard[1].lapTimes[1]);
        secondPlaceLapThreeTimeText.text = ConvertFloatToTime(leaderboard[1].lapTimes[2]);
        secondPlaceLapFourTimeText.text = ConvertFloatToTime(leaderboard[1].lapTimes[3]);
        secondPlaceNameText.text = leaderboard[1].name;

        // 3rd place
        thirdPlaceTimeText.text = ConvertFloatToTime(leaderboard[2].totalTime);
        thirdPlaceLapOneTimeText.text = ConvertFloatToTime(leaderboard[2].lapTimes[0]);
        thirdPlaceLapTwoTimeText.text = ConvertFloatToTime(leaderboard[2].lapTimes[1]);
        thirdPlaceLapThreeTimeText.text = ConvertFloatToTime(leaderboard[2].lapTimes[2]);
        thirdPlaceLapFourTimeText.text = ConvertFloatToTime(leaderboard[2].lapTimes[3]);
        thirdPlaceNameText.text = leaderboard[2].name;
    }


    public void SetDefaultScore()
    {
        //1st
        leaderboard[0].name = "Dev1";
        leaderboard[0].totalTime = 240;
        leaderboard[0].lapTimes[0] = 35;
        leaderboard[0].lapTimes[1] = 45;
        leaderboard[0].lapTimes[2] = 65;
        leaderboard[0].lapTimes[3] = 95;

        //2nd
    }


    public float GetThirdPlace()
    {
        LoadLeaderBoard();
        Bubblesort(3);

        return leaderboard[2].totalTime;
    }


    public string ConvertFloatToTime(float conversion)
    {

        string output;

        int minutes = Mathf.FloorToInt(conversion / 60);
        int seconds = Mathf.FloorToInt(conversion % 60);

        output = string.Format("{0}:{1:D2}", minutes, seconds);

        return output;
    }

}
