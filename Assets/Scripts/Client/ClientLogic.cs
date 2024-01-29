using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLogic : MonoBehaviour
{
    public GameObject clientOrder;
    public bool isReadyToMakeOrder;
    public bool gotOrder;
    public GameObject placeInQueue;
    public GameObject orderPoint;
    public GameObject exitPoint;
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
            placeInQueue = collision.gameObject;
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
        ChoosePath();
    }

    public void ChoosePath()
    {
        if(!gotOrder)
        { MoveToOrderPoint(); 
        }else
        {
            ExitBuilding();
        }
    }

    public void ChangePathToNew(Transform newPath)
    {
        gameObject.GetComponent<AIDestinationSetter>().target = newPath;        
    }

    private void Update()
    {
        if(!isMoving && !isReadyToMakeOrder && !gotOrder)
        {
            //MoveToOrderPoint();
        }
    }

    public void MoveToOrderPoint()
    {
        //проверка свободного места в очереди и передвижение к нему вплоть до точки заказа
        isMoving = true;
        gameObject.GetComponent<AIDestinationSetter>().target = orderPoint.transform;
    }

    public void ExitBuilding()
    {
        //заказ получен, выходим из сдания
        gameObject.GetComponent<AIDestinationSetter>().target = exitPoint.transform;
    }
}
