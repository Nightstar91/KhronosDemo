using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GameObject.Find("FadeInTransition").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
