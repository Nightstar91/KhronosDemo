using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] public GameObject gateObject;
    [SerializeField] public string groupID;

    private bool hasBeenCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        gameObject.SetActive(false);
        hasBeenCollected = true;
        gateObject.GetComponent<ObjectiveGate>().DecrementCoinCount();
    }

    private void ResetCoin()
    {
        gameObject.SetActive(true);
        hasBeenCollected = false;
    }
}
