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

    public LeaderboardEntry[] leaderboard;

    public bool canUserSubmit;

    // Related to the player score/time
    public float currentTime;
    public float[] currentLapTime = new float[4];
    public string playerInput = string.Empty;

    [SerializeField] GameObject SubmitPanel;
    [SerializeField] TextMeshProUGUI UserHeaderText;

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



    [SerializeField]
    public struct LeaderboardEntry
    {
        public string name;
        public float totalTime;
        public float[] lapTimes; // length = number of laps

        public LeaderboardEntry(int lapCount)
    {
        name = "";
        totalTime = 0f;
        lapTimes = new float[lapCount];
    }
    }

    private void Awake()
    {
        TempScore.cameFromChallenge = true;
        InitializeLeaderboard();
        LoadLeaderBoard();
    }

    private void Start()
    {   
        DisplayCurrentLeaderboard();
        ManipulateUserPanel();
    }


    public void LoadLeaderBoard()
    {
        for (int i = 0; i < TOTAL_ENTRIES; i++)
        {
            leaderboard[i].name = PlayerPrefs.GetString($"{placePrefix[i]}PlaceName");
            leaderboard[i].totalTime = PlayerPrefs.GetFloat($"{placePrefix[i]}PlaceTimer");
            leaderboard[i].lapTimes = new float[TOTAL_LAPS];

            for (int lap = 0; lap < TOTAL_LAPS; lap++)
            {
                string key = $"{placePrefix[i]}{lapNames[lap]}LapTime";
                leaderboard[i].lapTimes[lap] = PlayerPrefs.GetFloat(key);
            }
        }
    }


    public void SaveLeaderBoard()
    {
        for (int i = 0; i < TOTAL_ENTRIES; i++)
        {
            PlayerPrefs.SetString($"{placePrefix[i]}PlaceName", leaderboard[i].name);
            PlayerPrefs.SetFloat($"{placePrefix[i]}PlaceTimer", leaderboard[i].totalTime);

            for (int lap = 0; lap < TOTAL_LAPS; lap++)
            {
                string key = $"{placePrefix[i]}{lapNames[lap]}LapTime";
                PlayerPrefs.SetFloat(key, leaderboard[i].lapTimes[lap]);
            }
        }

        PlayerPrefs.Save();
    }

    private void InitializeLeaderboard()
    {
        leaderboard = new LeaderboardEntry[TOTAL_ENTRIES];
        for (int i = 0; i < TOTAL_ENTRIES; i++)
        {
            leaderboard[i] = new LeaderboardEntry(TOTAL_LAPS);
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
        leaderboard[1].name = "Dev2";
        leaderboard[1].totalTime = 420;
        leaderboard[1].lapTimes[0] = 65;
        leaderboard[1].lapTimes[1] = 85;
        leaderboard[1].lapTimes[2] = 105;
        leaderboard[1].lapTimes[3] = 160;

        //3rd
        leaderboard[2].name = "Dev3";
        leaderboard[2].totalTime = 600;
        leaderboard[2].lapTimes[0] = 95;
        leaderboard[2].lapTimes[1] = 115;
        leaderboard[2].lapTimes[2] = 155;
        leaderboard[2].lapTimes[3] = 235;

        SaveLeaderBoard();
    }


    public float GetThirdPlace()
    {
        LoadLeaderBoard();
        Bubblesort(3);

        return leaderboard[2].totalTime;
    }


    public bool IsNewRecord(float totalTime)
    {
        // Checking if usertime is lower than third place
        return totalTime < leaderboard[2].totalTime;
    }


    // Separate the submission so UI can prompt for name first before confirming
    public void SubmitEntry(string playerName, float totalTime, float[] lapTimes)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry(TOTAL_LAPS);
        LeaderboardEntry blankEntry = new LeaderboardEntry(TOTAL_LAPS);
        newEntry.name = playerName;
        newEntry.totalTime = totalTime;
        lapTimes.CopyTo(newEntry.lapTimes, 0);

        // Add to temp variable for bubble sort
        leaderboard[3] = newEntry;

        Bubblesort(4);

        // After everything is sorted correctly, delete previous record
        leaderboard[3] = blankEntry;

        SaveLeaderBoard();
    }

    public void ManipulateUserPanel()
    {
        // For player coming from the main menu
        if(!TempScore.cameFromChallenge && !canUserSubmit)
        {
            UserHeaderText.text = "This is the Leaderboard! Here is an example entry:";
            SubmitPanel.SetActive(false);
        }
        // For player coming from the final level with a score but doesn't get record
        else if (!canUserSubmit)
        {
            UserHeaderText.text = "No new record!\nBetter luck next time!";
            SubmitPanel.SetActive(false);
        }
        // For player coming from the final level with a score but does get record
        else if (canUserSubmit)
        {
            UserHeaderText.text = "A brand new record!\nGood job player!";
        }

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
