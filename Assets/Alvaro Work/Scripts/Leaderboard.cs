using UnityEngine;
using UnityEngine.Rendering;

public class Leaderboard : MonoBehaviour
{
    // 0 being the fastest
    private float[] leaderboard = new float[4];
    private string[] leaderboardName = new string[3];

    private string playerInput = string.Empty;

    public float[] firstPlaceLapTime = new float[4];
    public static float[] currentLapTime = new float[4];

    [SerializeField] public float tempTimer;

    private void Start()
    {
        LoadLeaderBoard();
    }


    public void LoadLeaderBoard()
    {
        // First place position
        leaderboard[0] = PlayerPrefs.GetFloat("FirstPlaceTimer", 240);
        leaderboardName[0] = PlayerPrefs.GetString("FirstPlaceName", "Dev1");
        firstPlaceLapTime[0] = PlayerPrefs.GetFloat("FirstLapTime", 35);
        firstPlaceLapTime[1] = PlayerPrefs.GetFloat("SecondLapTime", 45);
        firstPlaceLapTime[2] = PlayerPrefs.GetFloat("ThirdLapTime", 65);
        firstPlaceLapTime[3] = PlayerPrefs.GetFloat("FourthLapTime", 95);

        // Second place
        leaderboard[1] = PlayerPrefs.GetFloat("SecondPlaceTimer", 420);
        leaderboardName[1] = PlayerPrefs.GetString("FirstPlaceName", "Dev2");

        // Third place
        leaderboard[2] = PlayerPrefs.GetFloat("ThirdPlaceTimer", 600);
        leaderboardName[2] = PlayerPrefs.GetString("FirstPlaceName", "Dev3");

        leaderboard[3] = 0f; // this is a temp variable to be used for bubble sorting

        Debug.Log($"Loaded: \n{leaderboardName[0]}: {leaderboard[0]}\n1st Lap:{firstPlaceLapTime[0]}  2nd Lap:{firstPlaceLapTime[1]}  3rd Lap:{firstPlaceLapTime[2]}  4th Lap:{firstPlaceLapTime[3]}, \n\n {leaderboard[1]}: {leaderboardName[1]}, \n {leaderboard[2]}: {leaderboardName[2]}");
    }


    private void GetCurrentHighscoreString()
    {
        string message;
        string timeFormatted;

        for (int i = 0; i < leaderboard.Length - 1; i++)
        {
            timeFormatted = ConvertFloatToTime(leaderboard[i]);
            message = string.Format("{0} : {1:F2}", i + 1, timeFormatted);
            Debug.Log(message);
        }

        Debug.Log("END OF LEADERBOARD");
    }


    public void SetCurrentHighscore()
    {
        PlayerPrefs.SetFloat("FirstPlaceTimer", leaderboard[0]);
        PlayerPrefs.SetFloat("SecondPlaceTimer", leaderboard[1]);
        PlayerPrefs.SetFloat("ThirdPlaceTimer", leaderboard[2]);
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
        GetCurrentHighscoreString();

        return leaderboard[3];
    }


    private void Bubblesort()
    {
        int i, j;
        float temp;
        bool swapped;
        for (i = 0; i < leaderboard.Length - 1; i++)
        {
            swapped = false;
            for (j = 0; j < leaderboard.Length - i - 1; j++)
            {
                if (leaderboard[j] > leaderboard[j + 1])
                {

                    // Swap arr[j] and arr[j+1]
                    temp = leaderboard[j];
                    leaderboard[j] = leaderboard[j + 1];
                    leaderboard[j + 1] = temp;
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


    private void Bubblesort(float newScoreEntry)
    {
        leaderboard[3] = newScoreEntry;

        int i, j;
        float temp;
        bool swapped;
        for (i = 0; i < leaderboard.Length - 1; i++)
        {
            swapped = false;
            for (j = 0; j < leaderboard.Length - i - 1; j++)
            {
                if (leaderboard[j] > leaderboard[j + 1])
                {

                    // Swap arr[j] and arr[j+1]
                    temp = leaderboard[j];
                    leaderboard[j] = leaderboard[j + 1];
                    leaderboard[j + 1] = temp;
                    swapped = true;
                }
            }

            // If no two elements were
            // swapped by inner loop, then break
            if (swapped == false)
                break;
        }
    }
}
