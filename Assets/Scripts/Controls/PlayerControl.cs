using Pathfinding.Util;
using System;
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

    public Animator animator;
    public AnimationClip[] idleClips;
    protected AnimatorOverrideController animatorOverrideController;

    private void Awake()
    {
        controler = new Controls();
    }

    private void Start()
    {
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
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
            movement = controler.PC.Movement.ReadValue<Vector2>();

            // horizontal
            if (movement.x < 0)
            {
                animator.SetFloat("X", -1);
                animatorOverrideController["player_idle"] = idleClips[0];

            }
            else if (movement.x > 0)
            {
                animator.SetFloat("X", 1);
                animatorOverrideController["player_idle"] = idleClips[1];
            }
            else if (movement.x == 0)
            {
                animator.SetFloat("X", 0);
            }

            // vertical
            if (movement.y < 0)
            {
                animator.SetFloat("Y", -1);
                animatorOverrideController["player_idle"] = idleClips[2];
            }
            if (movement.y > 0)
            {
                animator.SetFloat("Y", 1);
                animatorOverrideController["player_idle"] = idleClips[3];
            }
            else if (movement.y == 0)
            {
                animator.SetFloat("Y", 0);
            }


        animator.SetFloat("Speed", movement.sqrMagnitude);



    }

    void FixedUpdate()
    {     
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}
