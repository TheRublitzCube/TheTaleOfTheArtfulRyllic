using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredDropletScript : MonoBehaviour {

    float groundCheckRadius = 0.2f; //radius of ground check area
    public LayerMask groundLayer; //layer the ground is on
    public Transform endOfGroundCheck; //point where ground stops
    public Transform wallCheck;
    public GameObject particle;
    Vector2 original;

    public float moveSpeed;
    bool facingRight = false;
    bool groundInFront;
    bool wallInFront;
    public int moveX;
    public int health;
    int damaged;

    public AudioSource audioSource;
    public AudioSource selfAudio;
    public AudioClip enemyHit;
    public AudioClip deathSound;
    public AudioClip squishSound;
    public AudioClip blockSound;

    // Use this for initialization
    void Start()
    {
        damaged = 0;
        original = transform.position;

        audioSource = GameObject.Find("Player").GetComponentInChildren<AudioSource>();
    }

    private void FixedUpdate()
    {
        groundInFront = Physics2D.OverlapCircle(endOfGroundCheck.position, groundCheckRadius, groundLayer);

        wallInFront = Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, groundLayer);

        if (!groundInFront || wallInFront)
        {
            moveX *= -1;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            damaged--;
        }

        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (health <= 0)
        {
            Instantiate(particle, transform.position, particle.transform.rotation);
            audioSource.PlayOneShot(deathSound, 1f);
            gameObject.SetActive(false);
            health = 3;
            transform.position = original;
        }

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * moveSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);

        if (moveX < 0.0f && !facingRight) { Flip(); }//flips Droplet left
        else if (moveX > 0.0f && facingRight) { Flip(); } //flips Droplet right

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {

            Vector2 difference = other.gameObject.transform.position - gameObject.transform.position;

            //if the AD is facing right and bullet hits from the left-------- OR the AD is facing left and bullet hits from the right 
            if (facingRight && difference.x > 0f){

                damaged = 2;
                health--;
                if (health > 0)
                {
                    audioSource.PlayOneShot(enemyHit);
                }

            }

            else if (!facingRight && difference.x < 0f)
            {

                damaged = 2;
                health--;
                if (health > 0)
                {
                    audioSource.PlayOneShot(enemyHit);
                }

            }

            else
            {
                audioSource.PlayOneShot(blockSound);
            }

            Destroy(other.gameObject);
        }

        else if (other.gameObject.tag == "MovingPlatform" || other.gameObject.tag == "GhostlyMovingPlatform")
        {
            transform.parent = other.transform;
        }
    }

    void PlaySquish()
    {

        selfAudio.pitch = Random.Range(0.8f, 1f);

        selfAudio.PlayOneShot(squishSound);

    }

}
