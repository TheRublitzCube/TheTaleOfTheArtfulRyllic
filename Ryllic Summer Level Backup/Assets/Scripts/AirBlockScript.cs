using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBlockScript : MonoBehaviour {

    Player_Controller player; //player controller
    Player_BulbForm_Controller bulbControls;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();
        bulbControls = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_BulbForm_Controller>();

    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bulbControls.inAirCurrent = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bulbControls.inAirCurrent = false;
            print("Out of air current");
        }
    }
}
