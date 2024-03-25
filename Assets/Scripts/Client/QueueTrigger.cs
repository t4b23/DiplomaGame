using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueTrigger : MonoBehaviour
{
    public GameObject currentClient;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Client" && currentClient == null)
        {
            currentClient = collision.gameObject;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Client" && currentClient != null)
        {
            currentClient = null;
            
        }
    }
}
