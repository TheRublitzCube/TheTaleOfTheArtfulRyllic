using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    //Movement Variables
    public int playerSpeed; //player walk speed
    bool facingRight = false; //variable to determine player direction
    public int jumpPower; //player jump height
    float moveX; //direction player is facing
    public bool canInput; //variable to determine if player can press any buttons
    public float topFallSpeed;

    //jumping variables
    public bool grounded = false; //variable to determine player is on the ground
    public bool nearWall = false;
    public bool canJump; //variable to determine if the player can jump
    public bool hasJumped;
    float groundCheckRadius = 0.2f; //radius of ground check area
    public LayerMask groundLayer; //layer the ground is on
    public Transform groundCheck; //point where grounded is checked
    public AudioClip jumpSound;
    public Transform wallCheck;

    //public Transform atCornerCheck;
    //public bool atCorner = false;

    //Transforming variables
    public enum Forms {Artful, Candle, Bulb}; //list of forms player can take
    public Forms activeForm; //current form being used
    public bool CanTransform;
    public AudioClip transformError;
    public AudioSource audioSource;

    //Form Controllers
    Player_ArtfulForm_Controller artfulController;
    Player_BulbForm_Controller bulbController;
    Player_CandleForm_Controller candleController;


    //Animation Controllers
    Animator myAnim;

    // Use this for initialization
    void Start () {

        canJump = true;
        hasJumped = false;
        canInput = true;

        artfulController = FindObjectOfType<Player_ArtfulForm_Controller>();
        bulbController = FindObjectOfType<Player_BulbForm_Controller>();
        candleController = FindObjectOfType<Player_CandleForm_Controller>();
                       
        activeForm = Forms.Artful;
        CanTransform = true;

        myAnim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {

        if (canInput)
        {
            PlayerMove();

            if (Input.GetButtonDown("Fire3"))
            {
                SwitchForms();
            }
        }

        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }


    }

    void FixedUpdate()
    {

        //Checks if player on ground, if not then player is falling
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        nearWall = Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, groundLayer);

        if (grounded == true && hasJumped == true)
        {
            hasJumped = false;
        }



        if (!grounded && GetComponent<Rigidbody2D>().velocity.y < -topFallSpeed)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -topFallSpeed);
        }

        if (nearWall)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }

        //setting animation variable for movement
        float move = GetComponent<Rigidbody2D>().velocity.x;
        myAnim.SetFloat("speed", Mathf.Abs(move));
        

        //setting variables for jumping and falling
        float verticalSpeed = GetComponent<Rigidbody2D>().velocity.y;
        myAnim.SetFloat("vspeed", verticalSpeed);

        bool isGrounded = grounded;
        myAnim.SetBool("isGrounded", isGrounded);

        //bool isAtCorner = atCorner;
        //myAnim.SetBool("isAtCorner", atCorner);

        //animation variable for shoot cooldown
        int cooldown = artfulController.cooldown;

        if (cooldown > 0)
        {
            myAnim.SetLayerWeight(1, 1f);
        }

        else if (cooldown == 0 || artfulController.enabled == false)
        {
            myAnim.SetLayerWeight(1, 0f);
        }

        //variable for tranform animation
        if ( activeForm == Forms.Artful)
        {
            myAnim.SetLayerWeight(0, 1f); //Artful layer
            myAnim.SetLayerWeight(2, 0f); //Candle layer
        }

        else if (activeForm == Forms.Candle)
        {
            myAnim.SetLayerWeight(0, 0f); //Artful layer
            myAnim.SetLayerWeight(2, 1f); //Candle layer
        }
        
    }

    void PlayerMove()
    {
        //CONTROLS
        moveX = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Submit") && grounded && canJump) //if player is on the ground and has not jumped yet
        {
            Jump();
        }

        else if (Input.GetButtonUp("Submit") && gameObject.GetComponent<Rigidbody2D>().velocity.y > 0) //released before apex of the jump
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, gameObject.GetComponent<Rigidbody2D>().velocity.y * .3f);
        }


        //DIRECTION
        if (moveX < 0.0f && !facingRight) { FlipPlayer(); }//flips player left
        else if (moveX > 0.0f && facingRight) { FlipPlayer(); } //flips player right

        //PHYSICS
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);

    }

    void FlipPlayer() //Code to flip player
    {
        facingRight = !facingRight;
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void Jump() //Jump Code
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPower);
        grounded = false;
        audioSource.pitch = 1f;
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(jumpSound);
        hasJumped = true;


        if (activeForm == Forms.Bulb)
        {
            bulbController.canDoubleJump = true;
        }
    }

    void SwitchForms()
    {
        if (CanTransform) //if transforming is allowed
        {
            if (activeForm == Forms.Artful)
            {
                activeForm = Forms.Candle;
                print("Candle form active");

            }

            else if (activeForm == Forms.Bulb)
            {
                activeForm = Forms.Artful;
                bulbController.hovering = false;
                bulbController.inAirCurrent = false;
                print("Artful form active");
            }

            else if (activeForm == Forms.Candle)
            {
                activeForm = Forms.Artful;
                print("Artful form active");
            }
        }

        else //player can't transform
        {
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
            audioSource.PlayOneShot(transformError);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.tag == "MovingPlatform" || other.gameObject.tag == "GhostlyMovingPlatform")
        {
            transform.parent = other.transform;
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {

        if (other.gameObject.tag == "MovingPlatform" || other.gameObject.tag == "GhostlyMovingPlatform")
        {
            transform.parent = null;
        }

    }

    /*void IsActive()
    {
        if (activeForm == Forms.Artful)
        {
            artfulController.enabled = true;
            bulbController.enabled = false;
            candleController.enabled = false;

        }

        else if (activeForm == Forms.Bulb)
        {
            artfulController.enabled = false;
            bulbController.enabled = true;
            candleController.enabled = false;
        }

        else if(activeForm == Forms.Candle)
        {
            artfulController.enabled = false;
            bulbController.enabled = false;
            candleController.enabled = true;
        }
    }
    */
}
