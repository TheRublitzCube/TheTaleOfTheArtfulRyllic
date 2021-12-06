using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmyScript : MonoBehaviour {

    public GameObject plasmy;
    public float moveSpeed;
    public Transform currentPoint;
    public Transform[] points;
    public int pointSelection;
    bool facingPos = false;

    public Animator myAnim;
    public AudioSource audioSource;
    public AudioClip blockSound;
    

    bool movingUp = false;
    bool movingLeft = false;
    public bool isStill;

    // Use this for initialization
    void Start () {
        
        currentPoint = points[pointSelection];
        audioSource = GameObject.Find("Player").GetComponentInChildren<AudioSource>();

    }

    private void Awake()
    {
        myAnim.SetBool("IsStill", isStill);
    }


    void FixedUpdate()
    {

        if (isStill == false)
        {
            plasmy.transform.position = Vector3.MoveTowards(plasmy.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);


            if (plasmy.transform.position == currentPoint.position)
            {

                pointSelection++;

                if (pointSelection == points.Length)
                {
                    pointSelection = 0;
                }

                currentPoint = points[pointSelection];

            }

            if (currentPoint.transform.position.y > plasmy.transform.position.y)
            {
                movingUp = true;
            }

            else
            {
                movingUp = false;
            }

            myAnim.SetBool("IsMovingUp", movingUp);

        }
    }


    private void OnTriggerEnter2D(Collider2D other)

    {
        if (other.gameObject.tag == "Bullet")
        {
            audioSource.PlayOneShot(blockSound);
            Destroy(other.gameObject);
        }
    }

    void Flip()
    {
        facingPos = !facingPos;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
