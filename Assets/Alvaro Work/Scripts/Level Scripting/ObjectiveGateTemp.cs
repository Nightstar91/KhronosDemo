using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using FMODUnity;
using FMOD.Studio;

public class ObjectiveGateTemp : MonoBehaviour
{
    public enum ObjectiveType
    {
        None,
        DialogueLock,
        Coin
    }
    [HideInInspector]
    public TextMeshPro doorText;
    [SerializeField] public string objectiveGateID;

    [Header("This would be the name of the groupID that can be found in the objective object BEWARE OF CAPS")]
    [SerializeField] public string objectiveSearchID;

    [Header("What kind of objective is it?")]
    [SerializeField] public ObjectiveType objectiveType;

    [SerializeField] private StudioEventEmitter dialogueEmitter;

    // DialogueLock related
    [Header("Dialogue related")]
    [SerializeField] public float dialogueTimer;
    public bool hasDialogueStarted;
    private bool hasDialogueCompleted;

    // Coin related
    private int allCoin;
    private int originalAllCoin;
    private GameObject[] coinArray;

    private void Awake()
    {
        doorText = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if(objectiveType == ObjectiveType.Coin)
        {
            coinArray = GameObject.FindGameObjectsWithTag("Coin");
           
            // Searching the entire array of gameobject with coin to search for a specific group assigned by the level designer
            foreach(GameObject coinObject in coinArray)
            {
                if (coinObject.GetComponent<Coin>().groupID == objectiveSearchID)
                {
                    allCoin++;
                }
            }

            originalAllCoin = allCoin;
            doorText.text = "Collect All Coins";
        }
        else if (objectiveType == ObjectiveType.DialogueLock)
        {
            hasDialogueStarted = false;
            hasDialogueCompleted = false;
            doorText.text = "Await Instruction";
        }
        else
        {
            // literally cooking nothing here
            gameObject.SetActive(false);
            return;
        }
    }


    private void Update()
    {
        if (objectiveType == ObjectiveType.Coin)
        {
            if (allCoin == 0)
            {
                gameObject.SetActive(false);
            }
        }
        else if (objectiveType == ObjectiveType.DialogueLock)
        {
            if (hasDialogueStarted)
            {
                DialogueCountdown();
            }

            if (hasDialogueCompleted)
            {
                gameObject.SetActive(false);
            }
        }
    }


    private void Reset()
    {
        if (objectiveType == ObjectiveType.Coin)
        {
            allCoin = originalAllCoin;
        }
        else if(objectiveType == ObjectiveType.DialogueLock)
        {
            hasDialogueCompleted = false;
        }
    }


    public void DecrementCoinCount()
    {
        allCoin--;
    }


    private void DialogueCountdown()
    {
        if(!hasDialogueCompleted && dialogueTimer >= 0)
        {
            dialogueTimer -= Time.deltaTime;
        }
        else
        {
            dialogueTimer = 0;
            hasDialogueCompleted = true;
        }
    }


    public void StartDialogue()
    {
        hasDialogueStarted = true;
    }


    private void DialogueComplete()
    {
        hasDialogueCompleted = true;
    }
}
