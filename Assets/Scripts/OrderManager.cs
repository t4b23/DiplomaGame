using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class OrderManager : MonoBehaviour
{
    public GameObject OrderPointPoint;
    public OrderObject orderObject;
    public OrderObject[] orders;
    public Item[] itemsToOrder;
    public int numberOfOrders;
    private void Start()
    {
        orderObject.orderedItems = null;
        orderObject.price = 0;
        GenerateNewOrder();
    }
    public void GenerateNewOrder()
    {       
        
        int price = Random.Range(10, 50);
        int numberOfItems = Random.Range(2,3);        
        Item[] items = new Item[numberOfItems];
        int currentItemInd = Random.Range(0, itemsToOrder.Length);
        for (int ind = 0; ind < numberOfItems; ind++)
            {
            items[ind] = itemsToOrder[Random.Range(0, itemsToOrder.Length)];
            }
        orderObject.orderedItems = items;
        orderObject.price = price;
        //int ordersLastItem = orders.Length;
        orders[0] = orderObject;
        SetNewOrder(orderObject);

    }

    public void SetNewOrder(OrderObject newOrder) 
    {
        OrderPointPoint.GetComponent<OrderPointScript>().order = newOrder;
    }   
}
