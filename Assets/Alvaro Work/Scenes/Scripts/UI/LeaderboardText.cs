using TMPro;
using UnityEngine;

public class LeaderboardText : MonoBehaviour
{
    Leaderboard board;
    public bool canUserSubmit = false;

    // UI related text
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = GetComponent<Leaderboard>();

        DisplayCurrentLeaderboard();
        ManipulateUserPanel();
    }

    public void ManipulateUserPanel()
    {
        // For player coming from the main menu
        if (!TempScore.cameFromChallenge && !canUserSubmit)
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

    public void DisplayCurrentLeaderboard()
    {
        //1st place
        firstPlaceTimeText.text = ConvertFloatToTime(board.leaderboard[0].totalTime);
        firstPlaceLapOneTimeText.text = ConvertFloatToTime(board.leaderboard[0].lapTimes[0]);
        firstPlaceLapTwoTimeText.text = ConvertFloatToTime(board.leaderboard[0].lapTimes[1]);
        firstPlaceLapThreeTimeText.text = ConvertFloatToTime(board.leaderboard[0].lapTimes[2]);
        firstPlaceLapFourTimeText.text = ConvertFloatToTime(board.leaderboard[0].lapTimes[3]);
        firstPlaceNameText.text = board.leaderboard[0].name;

        // 2nd place
        secondPlaceTimeText.text = ConvertFloatToTime(board.leaderboard[1].totalTime);
        secondPlaceLapOneTimeText.text = ConvertFloatToTime(board.leaderboard[1].lapTimes[0]);
        secondPlaceLapTwoTimeText.text = ConvertFloatToTime(board.leaderboard[1].lapTimes[1]);
        secondPlaceLapThreeTimeText.text = ConvertFloatToTime(board.leaderboard[1].lapTimes[2]);
        secondPlaceLapFourTimeText.text = ConvertFloatToTime(board.leaderboard[1].lapTimes[3]);
        secondPlaceNameText.text = board.leaderboard[1].name;

        // 3rd place
        thirdPlaceTimeText.text = ConvertFloatToTime(board.leaderboard[2].totalTime);
        thirdPlaceLapOneTimeText.text = ConvertFloatToTime(board.leaderboard[2].lapTimes[0]);
        thirdPlaceLapTwoTimeText.text = ConvertFloatToTime(board.leaderboard[2].lapTimes[1]);
        thirdPlaceLapThreeTimeText.text = ConvertFloatToTime(board.leaderboard[2].lapTimes[2]);
        thirdPlaceLapFourTimeText.text = ConvertFloatToTime(board.leaderboard[2].lapTimes[3]);
        thirdPlaceNameText.text = board.leaderboard[2].name;
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
