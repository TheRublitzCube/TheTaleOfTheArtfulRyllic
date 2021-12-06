using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlScreenUI : MonoBehaviour {

    public Animator animator;
    bool canInput;
    public Text startText;
    int flashTime;
    bool selectedOption;
    bool menuIsUp;


    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();
        startText.enabled = false;
        selectedOption = false;
        canInput = false;
        menuIsUp = false;
        flashTime = 60;

    }
	
	// Update is called once per frame
	void Update () {

        if (canInput == false && menuIsUp == false)
        {
            StartCoroutine("MenuSetUp");
            menuIsUp = true;
        }

        else if (canInput && menuIsUp) {

            if (flashTime > 0)
            {
                flashTime--;
            }

            else
            {
                startText.enabled = !startText.enabled;
                flashTime = 60;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && canInput && selectedOption == false)
        {
            canInput = false;
            selectedOption = true;

            StartCoroutine("StartGame");
        }

    }

    IEnumerator MenuSetUp()
    {
        yield return new WaitForSeconds(3);

        startText.enabled = true;

        canInput = true;
    }

    IEnumerator StartGame()
    {
        animator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(1);


    }


}
