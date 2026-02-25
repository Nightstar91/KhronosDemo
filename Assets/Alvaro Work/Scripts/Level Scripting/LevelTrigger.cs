using UnityEngine;
using UnityEngine.Events;

public class LevelTrigger : MonoBehaviour
{
    public enum SpecialTriggerType 
    { 
        Start,
        Finish
    }


    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;
    [SerializeField] SpecialTriggerType triggerType;
    [SerializeField] public bool triggerOnce = false;


    LevelObjective levelObjectiveController;

    private void Start()
    {
        levelObjectiveController = GameObject.Find("LevelObjectiveController").GetComponent<LevelObjective>();
    }

    [Tooltip("Use this method for reseting trigger")]
    public void ResetTrigger()
    {
        triggerOnce = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        // Validation steps
        if (other.tag != "Player") return;
        if (triggerOnce) return;

        if (!triggerOnce && triggerType == SpecialTriggerType.Start)
        {
            triggerOnce = true;
            // Event to start the timer
        }
        if (!triggerOnce && triggerType == SpecialTriggerType.Finish)
        {
            triggerOnce = true;
            // Event to end the timer
        }
    }

    
}
