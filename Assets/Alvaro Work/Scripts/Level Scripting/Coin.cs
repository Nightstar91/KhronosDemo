using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Group ID is used for objective gate to determine which coin to track")]
    [SerializeField] public string groupID;
    [Header("Gate Search ID is to link the coin to the gate")]
    [SerializeField] public string gateSearchID;
    private GameObject[] gateArray;
    public GameObject realGate;

    private bool hasBeenCollected = false;

    private void Start()
    {
        gateArray = GameObject.FindGameObjectsWithTag("Gate");

        foreach (GameObject gateObject in gateArray)
        {
            if (gateObject.GetComponent<ObjectiveGate>().objectiveGateID == gateSearchID)
            {
                realGate = gateObject;
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        gameObject.SetActive(false);
        hasBeenCollected = true;
        realGate.GetComponent<ObjectiveGate>().DecrementCoinCount();
    }

    private void ResetCoin()
    {
        gameObject.SetActive(true);
        hasBeenCollected = false;
    }
}
