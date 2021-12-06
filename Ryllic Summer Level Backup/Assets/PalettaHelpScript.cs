using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalettaHelpScript : MonoBehaviour {

    public TextMesh popupText;
    public string popupTextString;
    public DialogueTriggerScript dialogueTrigger;
    private DialogueManagerScript dialogueManager;
    GameObject player;
    private bool startedDialogue;
    bool playerEnter;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        dialogueTrigger = gameObject.GetComponent<DialogueTriggerScript>();
        dialogueManager = FindObjectOfType<DialogueManagerScript>();
        popupTextString = popupText.text;
        popupText.text = "";
        startedDialogue = false;
        playerEnter = false;
		
	}
	
	// Update is called once per frame
	void Update () {

        if (startedDialogue == false && Input.GetButtonDown("Submit") && playerEnter == true)
        {
            dialogueTrigger.TriggerDialogue();
            startedDialogue = true;
            player.GetComponent<Player_Controller>().canInput = false;
            FindObjectOfType<PauseGameScript>().canPause = false;
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, player.GetComponent<Rigidbody2D>().velocity.y);
        }

        if (startedDialogue && Input.GetButtonDown("Submit"))
        {
            int i = 0;
            i = dialogueManager.DisplayNextSentence();

            if (i == 1)
            {
                startedDialogue = false;
                player.GetComponent<Player_Controller>().canInput = true;
                FindObjectOfType<PauseGameScript>().canPause = true;
            }
            
        }
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerEnter = true;
            popupText.text = popupTextString;
            player.GetComponent<Player_Controller>().canJump = false;
        }
    }

    /*private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Submit") && startedDialogue == false)
        {
            dialogueTrigger.TriggerDialogue();
            startedDialogue = true;
            player.GetComponent<Player_Controller>().canInput = false;
            FindObjectOfType<PauseGameScript>().canPause = false;
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, player.GetComponent<Rigidbody2D>().velocity.y);
        }
    }*/

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerEnter = false;
            popupText.text = "";
            player.GetComponent<Player_Controller>().canJump = true;
        }
    }
}
