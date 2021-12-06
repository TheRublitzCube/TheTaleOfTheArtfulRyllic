using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropletScript : MonoBehaviour {

    float groundCheckRadius = 0.2f; //radius of ground check area
    public LayerMask groundLayer; //layer the ground is on
    public Transform endOfGroundCheck; //point where ground stops
    public Transform wallCheck;
    public GameObject particle;
    public Vector2 original;

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


    // Use this for initialization
    void Start () {

        damaged = 0;

        original = transform.position;

        audioSource = GameObject.Find("Player").GetComponentInChildren<AudioSource>();

        //selfAudio = GetComponent<AudioSource>();
		
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
    void Update () {

        if (damaged > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            damaged--;
        }

        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        if(health <= 0)
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
            if (health > 1)
            {
                audioSource.PlayOneShot(enemyHit);
            }
            health--;
            Destroy(other.gameObject);
            damaged = 2;
        }
    }

    void PlaySquish()
    {

        selfAudio.pitch = Random.Range(0.8f, 1f);

        selfAudio.PlayOneShot(squishSound);

    }
}
