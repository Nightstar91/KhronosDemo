using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Leaderboard : MonoBehaviour
{
    TimeSpan time = TimeSpan.Zero;

    // 0 being the fastest
    public float[] leaderboardTime = new float[4];
    public string[] leaderboardName = new string[3];

    public float[] firstPlaceLapTime = new float[4];
    public float[] secondPlaceLapTime = new float[4];
    public float[] thirdPlaceLapTime = new float[4];

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

    private void Start()
    {
        LoadLeaderBoard();
        DisplayCurrentLeaderboard();
    }


    public void LoadLeaderBoard()
    {
        // First place position
        leaderboardTime[0] = PlayerPrefs.GetFloat("FirstPlaceTimer", 240);
        leaderboardName[0] = PlayerPrefs.GetString("FirstPlaceName", "Dev1");

        firstPlaceLapTime[0] = PlayerPrefs.GetFloat("1stFirstLapTime", 35);
        firstPlaceLapTime[1] = PlayerPrefs.GetFloat("1stSecondLapTime", 45);
        firstPlaceLapTime[2] = PlayerPrefs.GetFloat("1stThirdLapTime", 65);
        firstPlaceLapTime[3] = PlayerPrefs.GetFloat("1stFourthLapTime", 95);

        // Second place
        leaderboardTime[1] = PlayerPrefs.GetFloat("SecondPlaceTimer", 420);
        leaderboardName[1] = PlayerPrefs.GetString("SecondPlaceName", "Dev2");

        secondPlaceLapTime[0] = PlayerPrefs.GetFloat("2ndFirstLapTime", 65);
        secondPlaceLapTime[1] = PlayerPrefs.GetFloat("2ndSecondLapTime", 85);
        secondPlaceLapTime[2] = PlayerPrefs.GetFloat("2ndThirdLapTime", 105);
        secondPlaceLapTime[3] = PlayerPrefs.GetFloat("2ndFourthLapTime", 160);

        // Third place
        leaderboardTime[2] = PlayerPrefs.GetFloat("ThirdPlaceTimer", 600);
        leaderboardName[2] = PlayerPrefs.GetString("ThirdPlaceName", "Dev3");

        thirdPlaceLapTime[0] = PlayerPrefs.GetFloat("3rdFirstLapTime", 130);
        thirdPlaceLapTime[1] = PlayerPrefs.GetFloat("3rdSecondLapTime", 180);
        thirdPlaceLapTime[2] = PlayerPrefs.GetFloat("3rdThirdLapTime", 290);
        thirdPlaceLapTime[3] = PlayerPrefs.GetFloat("3rdFourthLapTime", 400);

        //Debug.Log($"Loaded: \n{leaderboardName[0]}: {leaderboardTime[0]}\n1st Lap:{firstPlaceLapTime[0]}  2nd Lap:{firstPlaceLapTime[1]}  3rd Lap:{firstPlaceLapTime[2]}  4th Lap:{firstPlaceLapTime[3]}, \n\n {leaderboardTime[1]}: {leaderboardName[1]}, \n {leaderboardTime[2]}: {leaderboardName[2]}");
    }

    public void DisplayCurrentLeaderboard()
    {
        // 1st place
        firstPlaceTimeText.text = string.Format(ConvertFloatToTime(leaderboardTime[0]));
        firstPlaceLapOneTimeText.text = ConvertFloatToTime(firstPlaceLapTime[0]);
        firstPlaceLapTwoTimeText.text = ConvertFloatToTime(firstPlaceLapTime[1]);
        firstPlaceLapThreeTimeText.text = ConvertFloatToTime(firstPlaceLapTime[2]);
        firstPlaceLapFourTimeText.text = ConvertFloatToTime(firstPlaceLapTime[3]);
        firstPlaceNameText.text = leaderboardName[0];

        // 2nd place
        secondPlaceTimeText.text = ConvertFloatToTime(leaderboardTime[1]);
        secondPlaceLapOneTimeText.text = ConvertFloatToTime(secondPlaceLapTime[0]);
        secondPlaceLapTwoTimeText.text = ConvertFloatToTime(secondPlaceLapTime[1]);
        secondPlaceLapThreeTimeText.text = ConvertFloatToTime(secondPlaceLapTime[2]);
        secondPlaceLapFourTimeText.text = ConvertFloatToTime(secondPlaceLapTime[3]);
        secondPlaceNameText.text = leaderboardName[1];

        // 3rd place
        thirdPlaceTimeText.text = ConvertFloatToTime(leaderboardTime[2]);
        thirdPlaceLapOneTimeText.text = ConvertFloatToTime(thirdPlaceLapTime[0]);
        thirdPlaceLapTwoTimeText.text = ConvertFloatToTime(thirdPlaceLapTime[1]);
        thirdPlaceLapThreeTimeText.text = ConvertFloatToTime(thirdPlaceLapTime[2]);
        thirdPlaceLapFourTimeText.text = ConvertFloatToTime(thirdPlaceLapTime[3]);
        thirdPlaceNameText.text = leaderboardName[2];
    }

    private void GetCurrentHighscoreString()
    {
        string message;
        string timeFormatted;

        for (int i = 0; i < leaderboardTime.Length - 1; i++)
        {
            timeFormatted = ConvertFloatToTime(leaderboardTime[i]);
            message = string.Format("{0} : {1:F2}", i + 1, timeFormatted);
            Debug.Log(message);
        }

        Debug.Log("END OF LEADERBOARD");
    }


    public void SetCurrentHighscore()
    {
        PlayerPrefs.SetFloat("FirstPlaceTimer", leaderboardTime[0]);
        PlayerPrefs.SetFloat("1stFirstLapTime", firstPlaceLapTime[0]);
        PlayerPrefs.SetFloat("1stSecondLapTime", firstPlaceLapTime[1]);
        PlayerPrefs.SetFloat("1stThirdLapTime", firstPlaceLapTime[2]);
        PlayerPrefs.SetFloat("1stFourthLapTime", firstPlaceLapTime[3]);

        PlayerPrefs.SetFloat("SecondPlaceTimer", leaderboardTime[1]);
        PlayerPrefs.SetFloat("2ndFirstLapTime", secondPlaceLapTime[0]);

        PlayerPrefs.SetFloat("ThirdPlaceTimer", leaderboardTime[2]);
    }


    public string ConvertFloatToTime(float conversion)
    {

        string output;

        int minutes = Mathf.FloorToInt(conversion / 60);
        int seconds = Mathf.FloorToInt(conversion % 60);

        output = string.Format("{0}:{1:D2}", minutes, seconds);

        return output;
    }


    public float GetFirstPlaceTime()
    {
        LoadLeaderBoard();
        Bubblesort();
        //GetCurrentHighscoreString();

        return leaderboardTime[3];
    }


    private void Bubblesort()
    {
        int i, j;
        float temp;
        bool swapped;
        for (i = 0; i < leaderboardTime.Length - 1; i++)
        {
            swapped = false;
            for (j = 0; j < leaderboardTime.Length - i - 1; j++)
            {
                if (leaderboardTime[j] > leaderboardTime[j + 1])
                {

                    // Swap arr[j] and arr[j+1]
                    temp = leaderboardTime[j];
                    leaderboardTime[j] = leaderboardTime[j + 1];
                    leaderboardTime[j + 1] = temp;
                    swapped = true;
                }
            }

            // If no two elements were
            // swapped by inner loop, then break
            if (swapped == false)
                break;
        }
    }


    private void Bubblesort(float newScoreEntry)
    {
        leaderboardTime[3] = newScoreEntry;

        int i, j;
        float temp;
        bool swapped;
        for (i = 0; i < leaderboardTime.Length - 1; i++)
        {
            swapped = false;
            for (j = 0; j < leaderboardTime.Length - i - 1; j++)
            {
                if (leaderboardTime[j] > leaderboardTime[j + 1])
                {

                    // Swap arr[j] and arr[j+1]
                    temp = leaderboardTime[j];
                    leaderboardTime[j] = leaderboardTime[j + 1];
                    leaderboardTime[j + 1] = temp;
                    swapped = true;
                }
            }

            // If no two elements were
            // swapped by inner loop, then break
            if (swapped == false)
                break;
        }
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
