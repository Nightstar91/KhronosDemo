using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class LevelTransition : MonoBehaviour
{
    public string[] allScenes;
    Scene currentScene;
    int Scene;
    
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        Scene = FindScene();

        if(Scene == -1)
        {
            Debug.Log("SCENE NOT LOADED IN ARRAY!!!!!");
        }
    }

    public int FindScene()
    {
        int thisScene = -1;
        for (int i = 0; i < allScenes.Length; i++)
        {
            if (allScenes[i] == currentScene.name)
            {
                thisScene = i;
            }
        }
        return thisScene;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player" || Scene == -1)
        {
            return;
        }
       
        LoadScene(false);
    }

    public void LoadScene(bool resuming)
    {
        if (resuming)
        {
            StopAllAudio();
            SceneManager.LoadScene(allScenes[PlayerPrefs.GetInt("Scene", 1)]);
            return;
        }

        if (Scene == allScenes.Length - 1)
        {
            StopAllAudio();
            SceneManager.LoadScene(allScenes[0]);
        }
        else
        {
            StopAllAudio();
            SceneManager.LoadScene(allScenes[Scene + 1]);
        }
    }


    //make ienoumator


    public void ReturnToMainMenu()
    {
        PlayerPrefs.SetInt("Scene", Scene);
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToLeaderBoardScene()
    {
        SceneManager.LoadScene("ResultScreen");
    }


    private void StopAllAudio()
    {
        var masterBus = RuntimeManager.GetBus("bus:/");
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
