using UnityEngine;
using UnityEngine.Events;

public class LevelTrigger : MonoBehaviour
{
    public enum SpecialTriggerType 
    { 
        None,
        Start,
        Finish
    }


    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;
    [SerializeField] public bool triggerOnce;

    private sbyte triggerCounter;

    LevelObjective levelObjectiveController;

    private void Start()
    {
        levelObjectiveController = GameObject.Find("LevelObjectiveController").GetComponent<LevelObjective>();
    }

    [Tooltip("Use this method for reseting trigger with triggeronce")]
    public void ResetTrigger()
    {
        if(triggerOnce)
        {
            triggerCounter = 0;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        // Validation steps
        if (other.tag != "Player") return;
        if (triggerOnce && triggerCounter != 0) return;

        if (triggerOnce)
        {
            triggerCounter++;
            onTriggerEnter.Invoke();
        }
        else 
        {
            onTriggerEnter.Invoke();
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        // Validation steps
        if (other.tag != "Player") return;
        if (triggerOnce && triggerCounter != 0) return;

        if (triggerOnce)
        {
            triggerCounter++;
            onTriggerEnter.Invoke();
        }
        else
        {
            onTriggerEnter.Invoke();
        }
    }

    
}
