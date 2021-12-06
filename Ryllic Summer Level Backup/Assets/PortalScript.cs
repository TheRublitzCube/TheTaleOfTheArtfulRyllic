using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour {

    GameObject player;
    PauseGameScript pauseGameScript;
    LevelManager levelManager;
    public int levelToLoad;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        pauseGameScript = FindObjectOfType<PauseGameScript>();
        levelManager = FindObjectOfType<LevelManager>();


    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            StartCoroutine("Warping");
        }
    }

    IEnumerator Warping()
    {
        player.gameObject.GetComponent<Player_Controller>().canInput = false;
        pauseGameScript.canPause = false;
        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForSeconds(1);
        levelManager.FadeToLevelScene(levelToLoad);
    }
}
