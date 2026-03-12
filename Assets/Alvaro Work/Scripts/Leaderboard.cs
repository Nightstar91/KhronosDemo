using UnityEngine;
using UnityEngine.Rendering;

public class Leaderboard : MonoBehaviour
{
    // 0 being the fastest
    private float[] leaderboard = new float[4];

    [SerializeField] public float tempTimer;

    private void Start()
    {
        leaderboard[0] = PlayerPrefs.GetFloat("FirstPlaceTimer", 240);
        leaderboard[1] = PlayerPrefs.GetFloat("SecondPlaceTimer", 420);
        leaderboard[2] = PlayerPrefs.GetFloat("ThirdPlaceTimer", 600);
        leaderboard[3] = 0f; // this is a temp variable to be used for bubble sorting

        //Debug.Log("START OF LEADERBOARD");

    }


    private void GetCurrentHighscore()
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

        output = string.Format("{0}:{1}", minutes, seconds);

        return output;
    }


    public float GetFirstPlaceTime()
    {
        Bubblesort();

        return leaderboard[0];
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
