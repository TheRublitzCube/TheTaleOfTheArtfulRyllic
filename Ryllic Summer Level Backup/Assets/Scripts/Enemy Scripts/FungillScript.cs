using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungillScript : MonoBehaviour {
    
    bool facingRight = false;
    public int health;
    public int maxHealth;
    int damaged;
    public Animator animator;
    bool isInvincible;
    public Transform spinPointLeft;
    public Transform spinPointRight;
    float spinSpeed;
    public float fasterSpinSpeed;
    public float originalSpinSpeed;
    bool attacking;
    bool spinningFromRight; //true = attack starts from right 
    bool jumping;
    int passes;
    bool halfDead;
    public Color original;
    Color current;
    bool bossHasExploded = false;

    Vector3 playerCurrentPos;

    public AudioSource audioSource;
    public AudioClip enemyHit;
    public AudioClip deathSound;
    public AudioClip blockSound;


    public enum BossState
    {
        Idle,
        SpinAttack,
        JumpAttack,
        Death


    }

    private void OnEnable()
    {
        spinSpeed = originalSpinSpeed;
        current = original;
        halfDead = false;
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x = Mathf.Abs(localScale.x);
        transform.localScale = localScale;
        bossHasExploded = false;
    }

    public BossState currentState = BossState.Idle;

    // Use this for initialization
    void Start () {

        damaged = 0;
        currentState = BossState.Idle;
        animator = gameObject.GetComponent<Animator>();
        isInvincible = false;
        attacking = false;
        halfDead = false;
        passes = 0;
        original = GetComponent<SpriteRenderer>().color;
        current = original;

        audioSource = GameObject.Find("Player").GetComponentInChildren<AudioSource>();



        jumping = false;


    }

    // Update is called once per frame
    void Update () {

        if ((float)health / maxHealth <= .5f && halfDead == false)
        {

            spinSpeed = fasterSpinSpeed;
            current = Color.magenta;
            halfDead = true;

        }

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
            currentState = BossState.Death;
        }

        if (attacking == true)
        {
            SpinAttack();
        }


        switch (currentState)
        {
            case BossState.Idle:
                StartCoroutine("HandleIdleState");
                break;

            case BossState.Death:
                DeathState();
                break;
            
            case BossState.SpinAttack:
                if (attacking == false)
                {
                    StartCoroutine("HandleSpinAttackState");
                }
                break;

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            if (!isInvincible)
            {
                if (health > 1)
                {
                    audioSource.PlayOneShot(enemyHit);
                }
                health--;
                damaged = 2;
            }

            else
            {
                audioSource.PlayOneShot(blockSound);
            }
            Destroy(other.gameObject);
        }
    }

    IEnumerator HandleIdleState()
    {
        Debug.Log("Boss is idling");

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        GetComponent<SpriteRenderer>().color = current;


        isInvincible = false;

        attacking = false;

        yield return new WaitForSeconds(3);
        currentState = BossState.SpinAttack;
    }

    IEnumerator HandleSpinAttackState()
    {

        isInvincible = true;

        float distanceFromleft = Mathf.Abs(transform.position.x - spinPointLeft.position.x);
        float distanceFromRight = Mathf.Abs(transform.position.x - spinPointRight.position.x);


        if (distanceFromleft < distanceFromRight) //if closer to the left side
        {

            spinningFromRight = false;
        }

        else if (distanceFromleft > distanceFromRight) //if closer to the right side
        {
            spinningFromRight = true;
        }



        Debug.Log("Boss is spin attacking");

        if (!halfDead)
        {
            passes = 2;
        }

        else
        {
            passes = 3;
        }

        //GetComponent<SpriteRenderer>().color = Color.blue;

        animator.SetTrigger("Reel");

        yield return new WaitForSeconds(1);

        attacking = true;
        animator.SetTrigger("Spin");
        animator.ResetTrigger("Reel");

    }

    void DeathState()
    {
        Debug.Log("Boss has died");
        gameObject.SetActive(false);

        if (bossHasExploded == false)
        {
            audioSource.PlayOneShot(deathSound);
            bossHasExploded = true;
        }

    }

    


    void SpinAttack() {

        if (passes != 0)
        {

            if (spinningFromRight == false) //if closer to the left side
            {
                
                if (transform.position.x >= spinPointRight.position.x)
                {
                    spinningFromRight = true;
                    Flip();
                    passes--;
                }

                else
                {
                    Debug.Log("Spining from the left for " + passes + " more passes.");
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(spinSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                }
            }

            else //if closer to the right side
            {

                if (transform.position.x <= spinPointLeft.position.x)
                {
                    spinningFromRight = false;
                    Flip();
                    passes--;
                }

                else
                {
                    Debug.Log("Spining from the right for " + passes + " more passes.");
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-spinSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                }

            }

        }

        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            attacking = false;
            GetComponent<SpriteRenderer>().color = current;
            currentState = BossState.Idle;
            animator.SetTrigger("Idle");
            animator.ResetTrigger("Spin");
        }

        
        


    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


}



