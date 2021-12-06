using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BulbForm_Controller : MonoBehaviour
{

    Player_Controller player;

    public float hoverRate;
    public float rideRate;
    float originalGravityScale;
    public bool hovering;
    public bool inAirCurrent;
    public bool canDoubleJump;
    public bool hasDoubleJumped;
    public int doubleJumpPower;
    public bool isLocked;

    // Use this for initialization
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();
        originalGravityScale = player.gameObject.GetComponent<Rigidbody2D>().gravityScale;
        hovering = false;
        canDoubleJump = false;
        hasDoubleJumped = false;

    }

    /*// Update is called once per frame
    void Update()
    {

        /*if (player.activeForm == Player_Controller.Forms.Bulb)
        {

            player.GetComponent<SpriteRenderer>().color = Color.green;

            if (Input.GetButton("Jump") && !player.grounded && player.GetComponent<Rigidbody2D>().velocity.y < -0.1) //if player is not on the ground and hold the jump button
            {
                Hover();
            }

            /*if (Input.GetButtonUp("Jump") || player.grounded) //if player is not on the ground and hold the jump button
            {
                UndoHover();
            }

        } *

    }*/

    private void FixedUpdate()
    {
        if (player.activeForm == Player_Controller.Forms.Bulb)
        {

            player.GetComponent<SpriteRenderer>().color = Color.green;


            //if player is grounded
            if (player.grounded)
            {
                hasDoubleJumped = false;
                canDoubleJump = false;
            }


            //if player presses the jump button after jumping once but hasn't double jumped and not in an air current
            if (Input.GetButton("Jump") && canDoubleJump && !inAirCurrent && !hasDoubleJumped)
            {

                DoubleJump();

            }

            // if the player holds the jump button after tapping the button twice and is fallng and is not in an air current
            //if (Input.GetButton("Jump") && !player.grounded && player.GetComponent<Rigidbody2D>().velocity.y < -0.1 && !inAirCurrent) 
            //{

                   // Hover();

           // }

            // if the player holds the jump button after tapping the button twice and is fallng and is in an air current
           // else if (Input.GetButton("Jump") && !player.grounded && inAirCurrent)
            //{


               // RideCurrent();

            //}
            
            /* //if player is falling in an air current after pressing button twice
            if (inAirCurrent && player.GetComponent<Rigidbody2D>().velocity.y < -0.1)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -5);
            }*/

        }
    }

    void Hover()
    {
         gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -hoverRate);
    }

    void RideCurrent()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, rideRate);
    }

    void DoubleJump()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * doubleJumpPower);
        canDoubleJump = false;
        hasDoubleJumped = true;
    }

    /*void UndoHover()
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = originalGravityScale;
    }*/

    
}