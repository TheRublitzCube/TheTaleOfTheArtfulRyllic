using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public TextMesh doorText;
    public LevelManager levelManager;
    public int levelIndex;
    string doorTextString;
    GameObject player;
    bool entered;
    bool canEnter;

    // Use this for initialization
    void Start () {

        doorTextString = doorText.text;
        doorText.text = "";

        player = GameObject.FindGameObjectWithTag("Player");
        entered = false;
        canEnter = false;

        levelManager = FindObjectOfType<LevelManager>();

    }
	
	// Update is called once per frame
	void Update () {

        if (canEnter && Input.GetButtonDown("Submit") && entered == false)
        {
            entered = true;
            player.GetComponent<Player_Controller>().canInput = false;
            levelManager.FadeToLevelScene(levelIndex);
        }
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            doorText.text = doorTextString;
            canEnter = true;
        }
    }

    //private void OnTriggerStay2D(Collider2D other)
    //{

    //    if (other.gameObject.tag == "Player" && Input.GetKeyDown("z") && entered == false)
    //    {
    //        entered = true;
    //        player.GetComponent<Player_Controller>().canInput = false;
    //        levelManager.FadeToLevelScene(levelIndex);
    //    }
    //}

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            doorText.text = "";
            canEnter = false;
        }
    }
}
