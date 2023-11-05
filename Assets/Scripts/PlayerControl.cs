using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    private Controls controler;

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

    void Update()
    {
            movement = controler.PC.Movement.ReadValue<Vector2>();            ;
    }

    void FixedUpdate()
    {     
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}



/*[SerializeField]
private Tilemap groundTilemap;
[SerializeField]
private Tilemap collisionTilemap;
private Controls controler;

[SerializeField] private float walkTimer = 0.5f;
public Transform playerSprite;
public float moveSpeed;
public bool isMoving;
private Vector3 origPos;
private Vector3 targetPos;

// Start is called before the first frame update
private void Awake()
{
    controler = new Controls();
}

void Start()
{
    //controler.PC.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());

}

private void Update()
{

    if (controler.PC.Movement.IsInProgress())
    {
        //Debug.Log("Movement button pressed");
        Move(controler.PC.Movement.ReadValue<Vector2>());
    }

}

private void OnEnable()
{
    controler.Enable();
}

private void OnDisable()
{
    controler.Disable();
}

private void Move(Vector2 direction)
{
    if (CanMove(direction))
    {
        //Debug.Log("Can move, trying to do movement");
        transform.position += (Vector3)direction;
    }

}


private bool CanMove(Vector2 direction)
{
    Debug.Log("Checking if can move");
    Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
    if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition))
        return false;
    return true;
}*/