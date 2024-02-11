using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public GameObject clientPrefab;
    public Transform clientSpawnPoint;
    public OrderManager orderManager;
    public Transform orderPoint;
    public Transform exitPoint;    
    public GameObject[] Clients;
    public int maxNumberOfClients;
    public GameObject[] QueuePlaces;
    

    

    private void Start()
    {
        GenerateNewClient();
        GenerateNewClient();
        GenerateNewClient();
        RegroupMassive();
        ManageQueue();
    }

    public void GenerateNewClient()
    {
            Debug.Log("GeneratingNewClient");
            GameObject ClientObject = Instantiate(clientPrefab, transform);
            ClientObject.transform.position = clientSpawnPoint.transform.position;
            ClientObject.GetComponent<ClientLogic>().clientOrder = orderManager.GetComponent<OrderManager>().GenerateNewOrder();
            ClientObject.SetActive(true);
            for (int i = 0; i < maxNumberOfClients; i++)
            {
            Debug.Log("Looping");
            if (Clients[i] == null)
                {
                Debug.Log("Found");
                Clients[i] = ClientObject;
                return;
                }
            }
        RegroupMassive();
    }

    public void ClientExit(GameObject client)
    {
        client.GetComponent<ClientLogic>().placeInQueue = null;
        client.GetComponent<ClientLogic>().ChangePathToNew(exitPoint);
    }

    public void DestroyClient(GameObject client)
    {
        Debug.Log("Destroy client 2");
        for (int i = 0; i < Clients.Length; i++)
        {
            if (Clients[i] == client)
            {
                Clients[i] = null;
                Destroy(client);
                RegroupMassive();
                return;
            }
        }
    }

    public void RegroupMassive()
    {
        Debug.Log("Regrpoup massive");
        for (int i = 0;i < maxNumberOfClients;i++)
        {
            if (Clients[i] == null && i != maxNumberOfClients - 1)
            {
                Debug.Log("Number of client: " + i);
                Clients[i] = Clients[i + 1];
                Clients[i + 1] = null;                
            }            
        }
    }

    public void ManageQueue()
    {
        Debug.Log("Managing Queue");
        CleanQueues();
       for (int i = 0; i < QueuePlaces.Length; i++)
        {
            if (Clients[i] != null && QueuePlaces[i].GetComponent<QueueTrigger>().currentClient == null)
            {
                QueuePlaces[i].GetComponent<QueueTrigger>().currentClient = Clients[i];
                Clients[i].GetComponent<ClientLogic>().ChangePathToNew(QueuePlaces[i].transform);
            }
        }
    }

    private void CleanQueues()
    {
        for (int i = 0; i < QueuePlaces.Length; i++)
        {
            QueuePlaces[i].GetComponent<QueueTrigger>().currentClient = null;
        }
    }
}
