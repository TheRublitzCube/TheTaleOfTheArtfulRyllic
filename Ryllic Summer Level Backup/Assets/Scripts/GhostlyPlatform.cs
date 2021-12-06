using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostlyPlatform : MonoBehaviour {

    GameObject player;


	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");

    }
	
	// Update is called once per frame
	void Update () {



    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            player.GetComponent<Player_Controller>().CanTransform = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            player.GetComponent<Player_Controller>().CanTransform = true;
        }
    }

}
