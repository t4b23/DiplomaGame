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
    public int clientsToSpawn;
    public GameObject[] Clients;
    public GameObject[] NextClientsToDestroy;
    public int maxNumberOfClients;
    public GameObject[] QueuePlaces;




    private void Start()
    {        
        clientsToSpawn = QueuePlaces.Length;
        StartCoroutine(GenerateClientAfterTime());
        Clients = RegroupMassive(Clients, maxNumberOfClients);
        ManageQueue();

    }

    private void Update()
    {
/*        if (clientsToSpawn > 0)
        {
            StartCoroutine(GenerateClientAfterTime());            
        }*/
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
        Clients = RegroupMassive(Clients, maxNumberOfClients);
    }

    IEnumerator GenerateClientAfterTime()
    {
        for (int i = clientsToSpawn; i > 0; i--)
        {
            int time = Random.Range(2, 15);
            Debug.Log("Generating clients after: " + time + " seconds");
            yield return new WaitForSeconds(time);
            GenerateNewClient();
            ManageQueue();
            clientsToSpawn--;
        }
    }

    public void ClientExit(GameObject client)
    {
        client.GetComponent<ClientLogic>().placeInQueue = null;
        client.GetComponent<ClientLogic>().ChangePathToNew(exitPoint);
        addClientToDestroy(client);
        Clients[0] = null;
        Clients = RegroupMassive(Clients, maxNumberOfClients) ;
       //StartCoroutine(RegroupAfterSeconds(1));

    }

    void addClientToDestroy(GameObject client)
    {
        for (int i = 0;i <= NextClientsToDestroy.Length -1;i++)
        {
            if (NextClientsToDestroy[i] == null)
            {
                NextClientsToDestroy[i] = client;                
                break;
            }
            
        }
    }


    public void DestroyClient(GameObject client)
    {
        Debug.Log("Destroy client 2");
        for (int i = 0; i < NextClientsToDestroy.Length; i++)
        {
            if (NextClientsToDestroy[i] == client)
            {
                Debug.Log("Delete Client");
                NextClientsToDestroy[i] = null;
                Destroy(client);
                NextClientsToDestroy = RegroupMassive(NextClientsToDestroy, NextClientsToDestroy.Length);
                clientsToSpawn++;
                StartCoroutine(GenerateClientAfterTime());
                return;
            }
        }
    }

    public GameObject[] RegroupMassive(GameObject[] massive, int number)
    {
        Debug.Log("Regrpoup massive");
        for (int i = 0;i < number;i++)
        {
            if (massive[i] == null && i != number - 1)
            {
                Debug.Log("Number of client: " + i);
                massive[i] = massive[i + 1];
                massive[i + 1] = null;                
            }            
        }
        return massive;
    }

    public void ManageQueue()
    {
        Debug.Log("Managing Queue");
        CleanQueues();
       for (int i = 0; i < QueuePlaces.Length; i++)
        {
            if (Clients[i] != null && QueuePlaces[i].GetComponent<QueueTrigger>().currentClient == null && !Clients[i].GetComponent<ClientLogic>().gotOrder)
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
