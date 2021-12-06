using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ArtfulForm_Controller : MonoBehaviour {

    Player_Controller player;

    //Spitballing variables
    public Rigidbody2D spitball;
    public Transform spitballPoint;
    public int cooldown = 0;
    AudioSource audioSource;
    public AudioClip shootSound;

    // Use this for initialization
    void Start () {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();
        audioSource = GetComponent<Player_Controller>().audioSource;

    }

    // Update is called once per frame
    private void Update()
    {

        if (player.activeForm == Player_Controller.Forms.Artful)
        {
            if (Input.GetButtonDown("Fire1") && player.canInput)
            {
                Spitball();
                //audioSource.pitch = Random.Range(0.8f, 1f);
                audioSource.PlayOneShot(shootSound, 1f);

            }
        }


    }

    void FixedUpdate () {

        if ( player.activeForm == Player_Controller.Forms.Artful)
        {

            if (cooldown > 0)
            {
                cooldown--;
            }

        }

        else
        {
            cooldown = 0;
        }

    }

    void Spitball()
    {
        if (cooldown == 0)
        {
            cooldown = 8;

            Rigidbody2D spitballInstance = Instantiate(spitball, spitballPoint.position, Quaternion.Euler(new Vector3(0, 0, 1))) as Rigidbody2D;
            spitballInstance.constraints = RigidbodyConstraints2D.FreezePositionY;
            Physics2D.IgnoreCollision(spitballInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            
            if (player.transform.localScale.x > 1) { spitballInstance.velocity = new Vector2(20, 0); }
            else { spitballInstance.velocity = new Vector2(-20, 0); }


        }

    }
}
