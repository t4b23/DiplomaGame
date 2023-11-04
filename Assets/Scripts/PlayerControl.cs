using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
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

    private IEnumerator Move(Vector2 direction)
    {
        isMoving = true;
            float elapsedTime = 0;
        origPos = transform.position;
        if (CanMove(direction))
        {
            targetPos = origPos + direction;
        }

        while(elapsedTime < walkTimer)
        {
            transform.position = Vector3.Lerp(origPos,targetPos,(elapsedTime / walkTimer));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }


    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if(!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition))
            return false;
        return true;
    }
}
