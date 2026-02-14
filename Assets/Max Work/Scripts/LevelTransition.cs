using UnityEngine;
using UnityEngine.SceneManagement;

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

    private int FindScene()
    {
        int thisScene = -1;
        for (int i = 0; i < allScenes.Length - 1; i++)
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

        if(Scene == allScenes.Length - 1)
        {
            SceneManager.LoadScene(allScenes[0]);
        }
        else
        {
            SceneManager.LoadScene(allScenes[Scene + 1]);
        }
    }

    public void ReturnToMainMenu()
    {
        PlayerPrefs.SetInt("Scene", Scene);
        SceneManager.LoadScene(allScenes[0]);
    }
}
