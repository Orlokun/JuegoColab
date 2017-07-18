﻿using CnControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    protected Rigidbody2D rb2d;
    public float jumpSpeed;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;
    public Animator myAnim;
    public Vector3 respawnPosition;
    private LevelManager theLevelManager;

    public bool leftPressed;
    public bool rightPressed;
    public bool jumpPressed;
    public bool localPlayer;
    protected int direction;  //1 = derecha, -1 = izquierda
    protected Vector3 previous_transform;
    protected Transform transform;
    public int characterId;
    public float speed; //For animation nonlocal purposes


    public bool remoteRight; //Used to synchronize data from the server
    public bool remoteLeft; 
    public bool remoteJumping;

    // Use this for initialization
    protected virtual void Start()
    {
        remoteRight = false;
        remoteLeft = false;
        remoteJumping = false;
        transform = GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        respawnPosition = transform.position;
        theLevelManager = FindObjectOfType<LevelManager>();
        localPlayer = false;
        direction = 1;
    }

    public void Activate(int charId)
    {
        localPlayer = true;
        this.characterId = charId;
    }

    protected bool isGoingRight()
    {
        if (localPlayer)
        {
            float axis = CnInputManager.GetAxisRaw("Horizontal");
            if (!remoteRight && axis>0)
            {
                remoteRight = true;
                remoteLeft = false;
                SendObjectDataToServer();
            }
            else if(remoteRight && axis<=0)
            {
                remoteRight = false;
                SendObjectDataToServer();
            }
            return CnInputManager.GetAxisRaw("Horizontal") > 0f;
        }
            return remoteRight;
    }

    protected bool isGoingLeft()
    {
        if (localPlayer)
        {
            float axis = CnInputManager.GetAxisRaw("Horizontal");
            if (!remoteLeft && axis < 0)
            {
                remoteLeft = true;
                remoteRight = false;
                SendObjectDataToServer();
            }
            else if(remoteLeft && axis >= 0)
            {
                remoteLeft = false;
                SendObjectDataToServer();
            }
            return axis < 0f;
        }
        return remoteLeft;
    }

    protected bool isItGrounded()
    {
        bool ground = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        return ground;
    }

    protected bool isJumping(bool isGrounded)
    {
        if (localPlayer)
        {
            bool pressedJump = CnInputManager.GetButtonDown("Jump Button");
            bool saltando = pressedJump && isGrounded;
            if(saltando && !remoteJumping)
            {
                remoteJumping = true;
                SendObjectDataToServer();
            }
            else if(!saltando && remoteJumping)
            {
                remoteJumping = false;
                SendObjectDataToServer();
            }
            return saltando;
        }
        return remoteJumping;
    }

    protected bool CheckIfSomethingChanged()
    {
        Vector3 newPosition = transform.position;
        if(previous_transform.x != newPosition.x)
        {
            return true;
        }
        if(previous_transform.y != newPosition.y)
        {
            return true;
        }
        return false;
    }

    protected void SynchronizeNonLocalPlayer()
    {
        if (!localPlayer)
        {
            if(direction == 1)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            if(direction == -1)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            myAnim.SetFloat("Speed", speed);
            myAnim.SetBool("IsGrounded", isGrounded);

        }
    }

    protected int updateFrames = 0;

    protected virtual void Update()
    {
        if (isGoingRight())
        {
            rb2d.velocity = new Vector3(moveSpeed, rb2d.velocity.y, 0f);
            transform.localScale = new Vector3(1f, 1f, 1f);
            direction = 1;
        }
        else if (isGoingLeft())
        {
            rb2d.velocity = new Vector3(-moveSpeed, rb2d.velocity.y, 0f);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            direction = -1;
        }
        else // it's not moving
        {
            rb2d.velocity = new Vector3(0f, rb2d.velocity.y, 0f);
        }
        isGrounded = isItGrounded();
        if (isJumping(isGrounded))
        {
            rb2d.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
        previous_transform = transform.position;
        myAnim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        myAnim.SetBool("IsGrounded", isGrounded);  
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "KillPlane")
        {
            //gameObject.SetActive(false); 
            //transform.position = respawnPosition;
            theLevelManager.Respawn();
        }

        if (other.tag == "Checkpoint")
        {
            respawnPosition = other.transform.position;
        }        
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }
    }

    protected void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }

    public void SetVariablesFromServer(float positionX, float positionY, bool isGrounded, float speed, int direction, bool remoteRight, bool remoteLeft, bool remoteJumping)
    {
        if (localPlayer)
        {
            return;
        }
        transform.position = new Vector3(positionX, positionY);
        this.isGrounded = isGrounded;
        this.speed = speed;
        this.direction = direction;
        this.remoteRight = remoteRight;
        this.remoteLeft = remoteLeft;
        this.remoteJumping = remoteJumping;
        SynchronizeNonLocalPlayer();
    }

    protected void SendObjectDataToServer()
    {
        float position_x = transform.position.x;
        float position_y = transform.position.y;
        bool grounded = isGrounded;
        float speed = Mathf.Abs(rb2d.velocity.x);
        string message = "ChangePosition/" + characterId + "/" + position_x + "/" + position_y + "/" + isGrounded + "/" + speed + "/" + direction + "/" + remoteJumping + "/" + remoteLeft + "/" + remoteRight;
        Client.instance.SendMessageToServer(message);
    }
}