using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    public KeyCode rightKey;
    public KeyCode leftKey;
    public KeyCode jumpKey;
    protected RaycastHit2D hit;
    protected Vector3 vec;
    protected Rigidbody2D playerRb;
    protected float playerSpeed = 3f;
    protected float playerJump;
    protected float playerXStop = 0f;
    public float distance;
    [SerializeField] protected bool isMove = true;
    [SerializeField] private bool jumpAble;
    [SerializeField] private bool isGravity;
    [SerializeField] public SIZE_TYPE type;
    public enum SIZE_TYPE
    {
        DEFAULT,
        BIG,
    }
    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        IsGravity = this.isGravity;
        switch (type)
        {
            case SIZE_TYPE.DEFAULT:
                distance = 1.5f;
                break;
            case SIZE_TYPE.BIG:
                distance = 6f;
                break;
        }
    }

    public bool IsMove
    {
        get => isMove; 
        set { isMove = value; }
    }
    public float PlayerSpeed
    {
        get => playerSpeed;
        set
        {
            playerSpeed = value;
        }
    }
    public float PlayerJump
    {
        get => playerJump;
        set
        {
            playerJump = value;
        }
    }
    public bool IsGravity
    {
        get => isGravity;
        set
        {

            isGravity = value;
            if (isGravity == true)
            {
                vec = Vector3.down;
                playerJump = 8f;
            }
            if (isGravity == false)
            {
                vec = Vector3.up;
                playerJump = -8f;
            }
        }
    }
    protected virtual void FixedUpdate()
    {
        if (jumpAble && IsGravity)
            transform.position += new Vector3(0, 0.01f, 0);
        if (jumpAble && !IsGravity)
            transform.position -= new Vector3(0, 0.01f, 0);

    }
    protected virtual void Update()
    {
        if (playerRb.velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (playerRb.velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        hit = Physics2D.Raycast(playerRb.position, vec, distance, 1<<6);
        jumpAble = (hit.collider != null);
        if (hit.collider != null) { }
        else Debug.DrawRay(playerRb.position, vec * distance, Color.blue);

        if (isMove)
        {
            if (Input.GetKey(rightKey))
                playerRb.velocity = new Vector2(playerSpeed, playerRb.velocity.y);
            if (Input.GetKey(leftKey))
                playerRb.velocity = new Vector2(-playerSpeed, playerRb.velocity.y);
            if (Input.GetKeyUp(rightKey))
                playerRb.velocity = new Vector2(playerXStop, playerRb.velocity.y);
            if (Input.GetKeyUp(leftKey))
                playerRb.velocity = new Vector2(playerXStop, playerRb.velocity.y);
            if (Input.GetKeyDown(jumpKey))
                if (jumpAble)
                    playerRb.velocity = new Vector2(playerRb.velocity.x, playerJump);
        }
    }
}