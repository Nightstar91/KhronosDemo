using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    private BoxCollider triggerCollider;
    [SerializeField] GameObject wayPointToTeleport;
    private Vector3 spotToTeleport;
    public GameObject player;

    private void Awake()
    {
        //triggerCollider = GetComponent<BoxCollider>();
        spotToTeleport = new Vector3(wayPointToTeleport.transform.position.x, wayPointToTeleport.transform.position.y, wayPointToTeleport.transform.position.z);
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Validation steps
        if (other.tag != "Player") return;


        player.transform.position = spotToTeleport;
    }
}
