using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public GameObject clientPrefab;
    public Transform clientSpawnPoint;
    public OrderManager orderManager;

    public GameObject[] Clients;
    public GameObject[] QueuePlaces;
    public GameObject orderPoint;
    public GameObject exitPoint;

    public int maxNumberOfClients;

    public void GenerateNewClient()
    {
        if (Clients.Length < maxNumberOfClients)
        {
            GameObject ClientObject = Instantiate(clientPrefab, clientSpawnPoint);
            ClientObject.GetComponent<ClientLogic>().clientOrder = orderManager.GetComponent<OrderManager>().GenerateNewOrder();
        }
        
    }

    public void ClientExit(GameObject client)
    {

    }

    public void DeleteClient(GameObject client)
    {

    }


}
