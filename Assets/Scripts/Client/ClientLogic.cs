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

    public Vector2 movement = Vector2.zero;
    public Animator animator;
    public AnimationClip[] idleClips;

    public int AnimIndex;
    protected AnimatorOverrideController animatorOverrideController;

    private void Start()
    {
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OrderPoint")
        {
            isReadyToMakeOrder = true;
        }
        else if (collision.gameObject.tag == "queuePlace" && placeInQueue == null)
        {
            placeInQueue = collision.transform;
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




    public void ChangePathToNew(Transform newPath)
    {
        //проверка свободного места в очереди и передвижение к нему вплоть до точки заказа
        
        if (placeInQueue != newPath) 
        {
            gameObject.GetComponent<AIDestinationSetter>().target = newPath;
            placeInQueue = newPath;
        }

        
    }

    private void Update()
    {
        movement = gameObject.GetComponent<AIPath>().velocity;
       
        animator.SetInteger("AnimSetIndex", AnimIndex);
        // horizontal
        if (movement.x < 0)
        {
            animator.SetFloat("X", -1);
            animatorOverrideController["client_idle_down"] = idleClips[0];

        }
        else if (movement.x > 0)
        {
            animator.SetFloat("X", 1);
            animatorOverrideController["client_idle_down"] = idleClips[1];
        }
        else if (movement.x == 0)
        {
            animator.SetFloat("X", 0);
        }

        // vertical
        if (movement.y < 0)
        {
            animator.SetFloat("Y", -1);
            animatorOverrideController["client_idle_down"] = idleClips[2];
        }
        if (movement.y > 0)
        {
            animator.SetFloat("Y", 1);
            animatorOverrideController["client_idle_down"] = idleClips[3];
        }
        else if (movement.y == 0)
        {
            animator.SetFloat("Y", 0);
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);





    }

    public void ExitBuilding(Transform newPath)
    {
        //заказ получен, выходим из сдания
        gameObject.GetComponent<AIDestinationSetter>().target = newPath;
    }
}
