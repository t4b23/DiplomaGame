using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class OrderManager : MonoBehaviour
{
    public GameObject OrderPoint, orderObjectPrefab, orderObjectsRow;
    public int maxItemsInOrder;
    public Item[] itemsToOrder;
    private void Start()
    {
        //GenerateNewOrder();
    }
    public GameObject GenerateNewOrder()
    {
        GameObject OrderObj = Instantiate(orderObjectPrefab, orderObjectsRow.transform);
        int price = 0;
        int numberOfItems = Random.Range(1,maxItemsInOrder+1);        
        Item[] items = new Item[numberOfItems];
        int currentItemInd = Random.Range(0, itemsToOrder.Length);
        for (int ind = 0; ind < numberOfItems; ind++)
            {
            items[ind] = itemsToOrder[Random.Range(0, itemsToOrder.Length)];
            price += items[ind].Price;
            }
        OrderObj.GetComponent<OrderObjectPrefabScript>().orderedItems = items;
        OrderObj.GetComponent<OrderObjectPrefabScript>().price = price;
        //int ordersLastItem = orders.Length;
        //SetNewOrder(OrderObj)
        return OrderObj;

    }

    public void SetNewOrder(GameObject obj) 
    {
        OrderPoint.GetComponent<OrderPointScript>().currentOrder = obj;
    }   
}
