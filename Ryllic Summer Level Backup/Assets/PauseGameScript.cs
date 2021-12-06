using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameScript : MonoBehaviour {

    public GameObject pauseMenu;
    public bool isPaused;
    public bool canPause;
    GameObject player;


	// Use this for initialization
	void Start () {

        isPaused = false;
        canPause = false;

        player = GameObject.FindGameObjectWithTag("Player");

	}
	
	// Update is called once per frame
	void Update () {

        if (isPaused)
        {
            pauseMenu.SetActive(true);
        }

        else
        {
            pauseMenu.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Return) && canPause)
        {
            isPaused = !isPaused;
            player.GetComponent<Player_Controller>().canInput = !player.GetComponent<Player_Controller>().canInput;
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0f;
                BackgroundMusicScript.Instance.PauseMusic();
            }
            else
            {
                Time.timeScale = 1f;
                BackgroundMusicScript.Instance.PlayMusic();
            }
        }

    }
}
