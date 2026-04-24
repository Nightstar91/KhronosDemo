using UnityEngine;

public class TempScore : MonoBehaviour
{
    public static bool cameFromChallenge = false; // keep false unless player goes to leader by main menu
    public static string tempName;
    public static float tempTotalTime;
    public static float[] tempLapTime = { 0, 0, 0, 0 };


    public void SetFromChallengeCheckFalse()
    {
        cameFromChallenge = false;
    }

    public void SetFromChallengeCheckTrue()
    {
        cameFromChallenge = true;
    }
}
