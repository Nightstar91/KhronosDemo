using UnityEngine;
using UnityEngine.Events;

public class LevelTrigger : MonoBehaviour
{
    public enum SpecialTriggerType 
    { 
        Start,
        Finish
    }

    [SerializeField] SpecialTriggerType triggerType;
    [SerializeField] public bool triggerOnce = false;
    private BoxCollider triggerCollider;
    
    TempLevelObjective levelObjectiveController;

    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
        levelObjectiveController = GameObject.Find("LevelObjectiveController").GetComponent<TempLevelObjective>();
    }

    [Tooltip("Use this method for reseting trigger")]
    public void ResetTrigger()
    {
        triggerOnce = false;
        //triggerCollider.isTrigger = true; // so trigger can be passable again
    }


    private void OnTriggerExit(Collider other)
    {
        // Validation steps
        if (other.tag != "Player") return;
        if (triggerOnce) return;

        if (!triggerOnce && triggerType == SpecialTriggerType.Start)
        {
            triggerOnce = true;
            levelObjectiveController.TriggerLevelStart();
        }
        if (!triggerOnce && triggerType == SpecialTriggerType.Finish)
        {
            triggerOnce = true;
            levelObjectiveController.TriggerLevelEnd();
        }

        //triggerCollider.isTrigger = false; // Too lock player out like a one way door
    }
}
