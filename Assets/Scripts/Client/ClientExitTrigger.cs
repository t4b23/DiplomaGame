using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientExitTrigger : MonoBehaviour
{
    public ClientManager clientManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Client")
        {
            Debug.Log("Destroying Client");
            clientManager.DestroyClient(collision.gameObject);
            clientManager.ManageQueue();
        }
    }
}
