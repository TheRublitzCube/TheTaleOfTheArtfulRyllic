using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPlayerCode : MonoBehaviour {

    //Movement Variables
    public int playerSpeed; //player walk speed
    bool facingRight = false; //variable to determine player direction
    public int jumpPower; //player jump height
    float moveX; //direction player is facing
    public bool canInput; //variable to determine if player can press any buttons
    public float topFallSpeed; //terminal fall velocity

    //jumping variables
    public bool grounded = false; //variable to determine player is on the ground
    public bool canJump; //variable to determine if the player can jump
    float groundCheckRadius = 0.2f; //radius of ground check area
    public LayerMask groundLayer; //layer the ground is on
    public Transform groundCheck; //point where grounded is checked

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (canInput)
        {
            PlayerMove();
        }

        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }

    }

    private void FixedUpdate()
    {
        //Checks if player on ground, if not then player is falling
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!grounded && GetComponent<Rigidbody2D>().velocity.y < -topFallSpeed)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -topFallSpeed);
        }


    }

    void PlayerMove()
    {
        //CONTROLS
        moveX = Input.GetAxis("Horizontal");


        if (Input.GetButtonDown("Jump") && grounded) //if player is on the ground and has not jumped yet
        {
            grounded = false;

        }

        else if (Input.GetButtonUp("Jump") && gameObject.GetComponent<Rigidbody2D>().velocity.y > 0) //released before apex of the jump
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, gameObject.GetComponent<Rigidbody2D>().velocity.y * .3f);
        }


        //DIRECTION
        //if (moveX < 0.0f && !facingRight) { FlipPlayer(); }//flips player left
        //else if (moveX > 0.0f && facingRight) { FlipPlayer(); } //flips player right

        //PHYSICS
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);

    }
}
