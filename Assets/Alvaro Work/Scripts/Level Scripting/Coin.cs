using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("This is to link to the door that will unlock once all the coins have been collected")]
    [SerializeField] string doorToUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        gameObject.SetActive(false);
    }

    private void ResetCoin()
    {
        gameObject.SetActive(true);
    }
}
