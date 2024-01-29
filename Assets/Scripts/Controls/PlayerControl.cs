using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerControl : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    public Controls controler;

    public Vector2 movement = Vector2.zero;

    private void Awake()
    {
        controler = new Controls();
    }

    private void OnEnable()
    {
        controler.Enable();
    }

    private void OnDisable()
    {
        controler.Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inventoryManager.NextItemToPickup == null && collision.gameObject.tag == "Dropped Item")
        {
            inventoryManager.NextItemToPickup = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (inventoryManager.NextItemToPickup == collision.gameObject)
        {
            inventoryManager.NextItemToPickup = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (inventoryManager.NextItemToPickup == null && collision.gameObject.tag == "Dropped Item")
        {
            inventoryManager.NextItemToPickup = collision.gameObject;
        }
    }

    void Update()
    {
            movement = controler.PC.Movement.ReadValue<Vector2>();            ;
    }

    void FixedUpdate()
    {     
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}
