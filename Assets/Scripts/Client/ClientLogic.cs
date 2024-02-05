using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLogic : MonoBehaviour
{
    public GameObject clientOrder;
    public bool isReadyToMakeOrder;
    public bool gotOrder;
    public Transform placeInQueue;

    public bool isMoving;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OrderPoint")
        {
            isReadyToMakeOrder = true;
            isMoving = false;
        }
        else if (collision.gameObject.tag == "queuePlace" && placeInQueue == null)
        {
            placeInQueue = collision.transform;
            isMoving = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OrderPoint")
        {
            isReadyToMakeOrder = false;
        }
        else if (collision.gameObject.tag == "queuePlace" && placeInQueue != null)
        {
            placeInQueue = null;
        }
    }

    private void Start()
    {
        //ChoosePath();
    }

/*    public void ChoosePath()
    {
        if(!gotOrder)
        { MoveToOrderPoint(); 
        }else
        {
            ExitBuilding();
        }
    }*/

    public void ChangePathToNew(Transform newPath)
    {
        //проверка свободного места в очереди и передвижение к нему вплоть до точки заказа
        isMoving = true;
        placeInQueue = newPath;
        gameObject.GetComponent<AIDestinationSetter>().target = newPath;        
    }

    private void Update()
    {
    }

    public void ExitBuilding(Transform newPath)
    {
        //заказ получен, выходим из сдания
        isMoving = true;
        gameObject.GetComponent<AIDestinationSetter>().target = newPath;
    }
}
