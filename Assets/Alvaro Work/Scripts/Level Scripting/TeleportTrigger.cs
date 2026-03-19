using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    private BoxCollider triggerCollider;
    public Vector3 wayPoint;
    public GameObject player;

    private void Awake()
    {
        //triggerCollider = GetComponent<BoxCollider>();
        wayPoint = gameObject.transform.GetChild(0).transform.position;
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Validation steps
        if (other.tag != "Player") return;


        player.transform.position = wayPoint;
    }
}
