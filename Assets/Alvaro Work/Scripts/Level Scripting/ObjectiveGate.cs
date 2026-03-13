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

    private class CoinData
    {
        public Coin bob;
    }


    [SerializeField] public string objectiveGateName;

    [Header("This would be the name of the groupID that can be found in the objective object BEWARE OF CAPS")]
    [SerializeField] public string objectiveSearchID;

    public string[] objects = new string[3];

    [Header("What kind of objective is it?")]
    [SerializeField] public ObjectiveType objectiveType;

    // Dummy related


    // Coin related
    private int allCoin;
    private int originalAllCoin;
    private CoinData data;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(objectiveType == ObjectiveType.Coin)
        {
            
           
        }
        else if (objectiveType == ObjectiveType.Dummy)
        {
            //dummy parameters
        }
        else
        {
            // literally cooking nothing here
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
