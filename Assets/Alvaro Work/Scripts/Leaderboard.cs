using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Leaderboard : MonoBehaviour
{
    // 0 being the fastest
    private float[] leaderboardTime = new float[4];
    private string[] leaderboardName = new string[3];

    public float[] firstPlaceLapTime = new float[4];
    public float[] secondPlaceLapTime = new float[4];
    public float[] thirdPlaceLapTime = new float[4];

    // Related to the player score/time
    public float currentTime;
    public float[] currentLapTime = new float[4];
    private string playerInput = string.Empty;


    [SerializeField] TextMeshProUGUI firstPlaceTimeString;
    [SerializeField] TextMeshProUGUI firstPlaceLapOneTime;
    [SerializeField] TextMeshProUGUI firstPlaceLapTwoTime;
    [SerializeField] TextMeshProUGUI firstPlaceLapThirdTime;
    [SerializeField] TextMeshProUGUI firstPlaceLapFourTime;
    [SerializeField] TextMeshProUGUI firstPlaceName;

    [SerializeField] TextMeshProUGUI secondPlaceTimeString;
    [SerializeField] TextMeshProUGUI secondPlaceLapOneTime;
    [SerializeField] TextMeshProUGUI secondPlaceLapTwoTime;
    [SerializeField] TextMeshProUGUI secondPlaceLapThreeTime;
    [SerializeField] TextMeshProUGUI secondPlaceLapFourTime;
    [SerializeField] TextMeshProUGUI secondPlaceName;

    [SerializeField] TextMeshProUGUI thirdPlaceTimeString;
    [SerializeField] TextMeshProUGUI thirdPlaceLapOneTime;
    [SerializeField] TextMeshProUGUI thirdPlaceLapTwoTime;
    [SerializeField] TextMeshProUGUI thirdPlaceLapThreeTime;
    [SerializeField] TextMeshProUGUI thirdPlaceLapFourTime;
    [SerializeField] TextMeshProUGUI fourthPlaceTimeString;
    [SerializeField] TextMeshProUGUI thirdPlaceName;

    private void Start()
    {
        LoadLeaderBoard();
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

        secondPlaceLapTime[0] = PlayerPrefs.GetFloat("2ndFirstLapTime");
        secondPlaceLapTime[1] = PlayerPrefs.GetFloat("2ndSecondLapTime");
        secondPlaceLapTime[2] = PlayerPrefs.GetFloat("2ndThirdLapTime");
        secondPlaceLapTime[3] = PlayerPrefs.GetFloat("2ndFourthLapTime");

        // Third place
        leaderboardTime[2] = PlayerPrefs.GetFloat("ThirdPlaceTimer", 600);
        leaderboardName[2] = PlayerPrefs.GetString("ThirdPlaceName", "Dev3");

        thirdPlaceLapTime[0] = PlayerPrefs.GetFloat("3rdFirstLapTime", 130);
        thirdPlaceLapTime[1] = PlayerPrefs.GetFloat("3rdSecondLapTime", 180);
        thirdPlaceLapTime[2] = PlayerPrefs.GetFloat("3rdThirdLapTime", 290);
        thirdPlaceLapTime[3] = PlayerPrefs.GetFloat("3rdFourthLapTime", 400);

        Debug.Log($"Loaded: \n{leaderboardName[0]}: {leaderboardTime[0]}\n1st Lap:{firstPlaceLapTime[0]}  2nd Lap:{firstPlaceLapTime[1]}  3rd Lap:{firstPlaceLapTime[2]}  4th Lap:{firstPlaceLapTime[3]}, \n\n {leaderboardTime[1]}: {leaderboardName[1]}, \n {leaderboardTime[2]}: {leaderboardName[2]}");
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


    public string ConvertFloatToTime(float Conversion)
    {
        string output;

        int minutes = Mathf.FloorToInt(Conversion / 60);
        int seconds = Mathf.FloorToInt(Conversion % 60);

        output = string.Format("{0}:{1:F2}", minutes, seconds);

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
