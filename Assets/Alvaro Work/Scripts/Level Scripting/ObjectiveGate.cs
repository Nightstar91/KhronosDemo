using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class ObjectiveGate : MonoBehaviour
{
    public enum ObjectiveType
    {
        None,
        Coin,
        Dummy
    }

    [SerializeField] public string objectiveGateID;

    [Header("This would be the name of the groupID that can be found in the objective object BEWARE OF CAPS")]
    [SerializeField] public string objectiveSearchID;

    [Header("What kind of objective is it?")]
    [SerializeField] public ObjectiveType objectiveType;

    // Dummy related
    bool hasDummyBeenRescue;

    // Coin related
    private int allCoin;
    private int originalAllCoin;
    private GameObject[] coinArray;

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
        }
        else if (objectiveType == ObjectiveType.Dummy)
        {
            //dummy parameters
            hasDummyBeenRescue = false;
        }
        else
        {
            // literally cooking nothing here
            return;
        }
    }


    private void Update()
    {
        if (allCoin == 0)
        {
            gameObject.SetActive(false);
        }
    }


    private void Reset()
    {
        if (objectiveType == ObjectiveType.Coin)
        {
            allCoin = originalAllCoin;
        }
    }


    public void DecrementCoinCount()
    {
        allCoin--;
    }
}
