using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformAttach : MonoBehaviour
{
    [SerializeField] string PlayerTag = "Player";
    [SerializeField] Transform platform;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals (PlayerTag))
        {
            other.gameObject.transform.parent = platform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals (PlayerTag))
        {
            other.gameObject.transform.parent = null;
        }
    }




}
