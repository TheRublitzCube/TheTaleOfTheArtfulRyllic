using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManagerScript : MonoBehaviour {

    Queue<string> sentences;

    public Text nameText;
    public Image dialogueBox;
    public Text dialogueText;


    // Use this for initialization
    void Start () {

        sentences = new Queue<string>();

        dialogueBox = GameObject.Find("DialogueBox").GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<Text>();
        nameText = GameObject.Find("DialogueName").GetComponent<Text>();

        dialogueBox.enabled = false;
        dialogueText.text = "";
        nameText.text = "";


    }

    public void StartDialogue (DialogueScript dialogue)
    {


        Debug.Log("Starting conversation with " + dialogue.name);

        dialogueBox.enabled = true;

        Time.timeScale = 0f;

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        { 
            sentences.Enqueue(sentence);
        }

    }

    public int DisplayNextSentence()
    {

        if (sentences.Count == 0)
        {
            EndDialogue();
            return 1;

        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        return 0;

    }

    void EndDialogue()
    {
        Debug.Log("Ending converstaion.");

        Time.timeScale = 1f;

        dialogueBox.enabled = false;
        dialogueText.text = "";
        nameText.text = "";
    }
}
