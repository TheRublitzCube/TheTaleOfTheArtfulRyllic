using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    public Text resetLevelText;
    public Text endGameText;

    public Animator animator;

    public enum GameOverOptions {Reset, End};

    public GameOverOptions selected;

    Color notHighlighted;

    bool canInput;

    bool selectedOption;


	// Use this for initialization
	void Start () {

        animator = GetComponent<Animator>();

        selected = GameOverOptions.Reset;

        notHighlighted = endGameText.color;

        resetLevelText.enabled = false;

        endGameText.enabled = false;

        canInput = false;

        selectedOption = false;


	}

    private void Awake()
    {
        Destroy(GameObject.FindObjectOfType<BackgroundMusicScript>());
        ResetPlayer();
    }

    // Update is called once per frame
    void Update () {

        if (canInput == false)
        {
            StartCoroutine("MenuSetUp");
        }


        if (Input.GetButtonDown("Horizontal") && canInput == true)
        {
            if (selected == GameOverOptions.Reset)
            {
                selected = GameOverOptions.End;
            }

            else if (selected == GameOverOptions.End)
            {
                selected = GameOverOptions.Reset;
            }


        }


        if (selected == GameOverOptions.Reset)
        {
            resetLevelText.color = Color.white;
            endGameText.color = notHighlighted;
        }

        else if (selected == GameOverOptions.End)
        {
            resetLevelText.color = notHighlighted;
            endGameText.color = Color.white;
        }



        if (Input.GetKeyDown(KeyCode.Z) && canInput && selectedOption == false)
        {
            canInput = false;
            selectedOption = true;

            if (selected == GameOverOptions.Reset)
            {
                StartCoroutine("ResetGame");
            }

            else if (selected == GameOverOptions.End)
            {
                StartCoroutine("QuitGame");
            }
        }



        



    }

    IEnumerator MenuSetUp()
    {
        yield return new WaitForSeconds(3);

        resetLevelText.enabled = true;

        endGameText.enabled = true;

        canInput = true;

    }

    IEnumerator ResetGame()
    {

        animator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1);

         SceneManager.LoadScene(1);
        


    }

    IEnumerator QuitGame()
    {
        animator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1);

        //SceneManager.LoadScene(9);
        Application.Quit();


    }

    public void ResetPlayer()
    {
        GlobalControlScript.Instance.currentPlayerHealth = 6;
        GlobalControlScript.Instance.colorDrops = 0;
        GlobalControlScript.Instance.livesLeft = 2;

        int i = 0;

        foreach (int fragment in GlobalControlScript.Instance.powerFragments)
        {

            GlobalControlScript.Instance.powerFragments[i] = 0;
            i++;


        }


    }


}
