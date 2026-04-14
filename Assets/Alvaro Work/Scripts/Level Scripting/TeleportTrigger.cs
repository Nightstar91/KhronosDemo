using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    private BoxCollider triggerCollider;
    [SerializeField] GameObject wayPointToTeleport;
    private Transform spotToTeleport;
    private GameObject player;

    private void Awake()
    {
        //triggerCollider = GetComponent<BoxCollider>();
        spotToTeleport = wayPointToTeleport.transform;
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Validation steps
        if (other.tag != "Player") return;


        player.transform.position = spotToTeleport.position;
        player.transform.rotation = spotToTeleport.rotation;
    }
}
