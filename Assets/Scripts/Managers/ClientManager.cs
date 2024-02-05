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

    private void RegroupMassive()
    {
        Debug.Log("Regrpoup massive");
        for (int i = 0;i < maxNumberOfClients;i++)
        {
            if (Clients[i] == null && Clients[i+1] != null && Clients.Length < maxNumberOfClients) 
            {
                Clients[i] = Clients[i+1];
                Clients[i + 1] = null;
            }
        }
    }

    public void ManageQueue()
    {
        Debug.Log("Managing Queue");
       for (int i = 0; i < QueuePlaces.Length; i++)
        {
            if (QueuePlaces[i].GetComponent<QueueTrigger>().currentClient == null)
            {
                for (int j = 0; j < Clients.Length; j++)
                {
                        QueuePlaces[i].GetComponent<QueueTrigger>().currentClient = Clients[j];
                        Clients[j].GetComponent<ClientLogic>().ChangePathToNew(QueuePlaces[i].transform);                    
                }
            }
        }
    }
}
