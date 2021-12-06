using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBellScript : MonoBehaviour {

    public Animator animator;
    public bool rungBell;
    public AudioSource bellAudio;
    public AudioClip bellMusic;
    GameObject player;
    bool facingRight = false;
    LevelManager levelManager;
    public int victorySceneIndex;

	// Use this for initialization
	void Start () {

        rungBell = false;
        player = GameObject.Find("Player");
        bellAudio = player.gameObject.GetComponent<Player_Controller>().audioSource;
        levelManager = FindObjectOfType<LevelManager>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player && rungBell == false)
        {

            if(transform.position.x > player.transform.position.x)
            {
                Flip();
            }
            animator.SetTrigger("Rung");
            player.GetComponent<Player_Controller>().canInput = false;
            player.GetComponent<Player_Controller>().activeForm = Player_Controller.Forms.Artful;
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, player.GetComponent<Rigidbody2D>().velocity.y);
            FindObjectOfType<PauseGameScript>().canPause = false;
            BackgroundMusicScript.Instance.StopMusic();
            bellAudio.PlayOneShot(bellMusic, 1f);
            StartCoroutine("Victory");
            
        }
    }

    void Flip() //Code to flip player
    {
        facingRight = !facingRight;
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    IEnumerator Victory()
    {
        yield return new WaitForSeconds(6f);
        levelManager.FadeToLevelScene(victorySceneIndex);
    }
}
