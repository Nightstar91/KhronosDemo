using UnityEngine;
using UnityEngine.Rendering;

public class Leaderboard : MonoBehaviour
{
    private float[] leaderboard = new float[4];

    [SerializeField] public float tempTimer;

    private void Start()
    {
        leaderboard[0] = PlayerPrefs.GetFloat("FirstPlaceTimer", 240);
        leaderboard[1] = PlayerPrefs.GetFloat("SecondPlaceTimer", 420);
        leaderboard[2] = PlayerPrefs.GetFloat("ThirdPlaceTimer", 600);
        leaderboard[3] = tempTimer; // this is a temp variable to be used for bubble sorting

        Debug.Log("START OF LEADERBOARD");

        Bubblesort(tempTimer);
        GetCurrentHighscore();
    }


    private void GetCurrentHighscore()
    {
        
        string message;

        for (int i = 0; i < leaderboard.Length; i++)
        {
            message = string.Format("{0} : {1}", i, leaderboard[i]);
            Debug.Log(message);
        }

        Debug.Log("END OF LEADERBOARD");
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
