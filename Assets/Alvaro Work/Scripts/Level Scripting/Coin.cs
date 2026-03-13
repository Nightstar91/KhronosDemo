using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("This coin group ID to will be used for the GateObjective to see which group is counting toward for unlock")]
    [SerializeField] public string GroupID;
    private bool hasBeenCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        gameObject.SetActive(false);
        hasBeenCollected = true;

    }

    private void ResetCoin()
    {
        gameObject.SetActive(true);
        hasBeenCollected = false;
    }
}
