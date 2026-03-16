using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatformCollision : MonoBehaviour
{
    [SerializeField] Transform platform;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Touched platform");
            collision.gameObject.transform.parent = platform; // moves the player if he stands on a moving platform
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Got off from platform");
            collision.gameObject.transform.parent = null;
        }  
    }

}
