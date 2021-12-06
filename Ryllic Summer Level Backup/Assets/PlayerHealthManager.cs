using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{

    Player_Controller player;

    //Damage
    bool isInvincible;
    float invincibleTime;
    int maxPlayerHealth = 6;
    public int currentPlayerHealth;
    public int colorDrops;
    public int livesLeft;
    public int fragmentsFound;
    public int[] powerFragments;
    AudioSource audioSource;
    public AudioClip commonPickupSound;
    public AudioClip heartSound;
    public AudioClip powerFragmentSound;
    public AudioClip damageSound;
    public AudioClip deathSound;

    //public Transform checkpoint;
    //public float respawnTime;
    public GameObject deathParticle;
    float gravityStore;
    bool isDead;

    LevelManager levelManager;
    PauseGameScript pauseGameScript;




    // Use this for initialization
    void Start()
    {

        player = GetComponent<Player_Controller>();
        pauseGameScript = GameObject.Find("PauseMenu").GetComponent<PauseGameScript>();
        isInvincible = false;
        isDead = false;
        invincibleTime = 0;
        gravityStore = player.gameObject.GetComponent<Rigidbody2D>().gravityScale;
        audioSource = gameObject.GetComponent<Player_Controller>().audioSource;


        //Global variables for player health manager
        currentPlayerHealth = GlobalControlScript.Instance.currentPlayerHealth;
        colorDrops = GlobalControlScript.Instance.colorDrops;
        livesLeft = GlobalControlScript.Instance.livesLeft;
        powerFragments = GlobalControlScript.Instance.powerFragments;

        levelManager = FindObjectOfType<LevelManager>();

    }

    // Update is called once per frame
    void Update()
    {

        if (currentPlayerHealth <= 0 && isDead == false)
        {

            audioSource.PlayOneShot(deathSound, 1f);

            isDead = true;

            if (livesLeft > 0)
            {
                StartCoroutine("Respawn");
            }

            else
            {
                StartCoroutine("EndGame");
            }

        }

    }

    private void FixedUpdate()
    {
        if (invincibleTime > 0)
        {
            invincibleTime -= 1;
        }

        else
        {
            invincibleTime = 0;
            isInvincible = false;
        }

        if (invincibleTime % 5 == 0 && isInvincible || isDead == true)
        {
            player.GetComponent<SpriteRenderer>().enabled = false;
        }

        else
        {
            player.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Heart")
        {
            if (currentPlayerHealth < maxPlayerHealth)
            {
                currentPlayerHealth++;
            }
            audioSource.pitch = 1f;
            audioSource.volume = 0.4f;
            audioSource.PlayOneShot(heartSound);
            other.gameObject.SetActive(false);
        }

        else if (other.gameObject.tag == "GoldenHeart")
        {
            currentPlayerHealth = maxPlayerHealth;
            audioSource.pitch = 1f;
            audioSource.volume = 0.4f;
            audioSource.PlayOneShot(heartSound);
            other.gameObject.SetActive(false);
        }

        else if (other.gameObject.tag == "DeathPit")
        {
            currentPlayerHealth = 0;
        }

        else if (other.gameObject.tag == "RedColorDrop")
        {
            colorDrops += 1;
            other.gameObject.SetActive(false);
            audioSource.pitch = 1f;
            audioSource.volume = 0.4f;
            audioSource.PlayOneShot(commonPickupSound);

            if (colorDrops >= 50)
            {
                colorDrops = 0;
                livesLeft++;
            }
        }

        else if (other.gameObject.tag == "BlueColorDrop")
        {
            colorDrops += 2;
            other.gameObject.SetActive(false);
            audioSource.pitch = 1f;
            audioSource.volume = 0.4f;
            audioSource.PlayOneShot(commonPickupSound);

            if (colorDrops >= 50)
            {
                colorDrops = 0;
                livesLeft++;
            }
        }

        else if (other.gameObject.tag == "OrangeColorDrop")
        {
            colorDrops += 5;
            other.gameObject.SetActive(false);
            audioSource.pitch = 1f;
            audioSource.volume = 0.4f;
            audioSource.PlayOneShot(commonPickupSound);

            if (colorDrops >= 50)
            {
                colorDrops = 0;
                livesLeft++;
            }
        }

        else if (other.gameObject.tag == "PowerFragment")
        {

            powerFragments[other.gameObject.GetComponent<PowerFragmentScript>().id] = 1;
            other.gameObject.GetComponent<PowerFragmentScript>().collected = true;
            audioSource.pitch = 1f;
            audioSource.volume = 0.4f;
            audioSource.PlayOneShot(powerFragmentSound);
            other.gameObject.SetActive(false);

        }


    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 11 && currentPlayerHealth > 0 && !isInvincible)
        {
            if (currentPlayerHealth > 1)
            {
                audioSource.PlayOneShot(damageSound, 1f);
            }
            currentPlayerHealth--;
            isInvincible = true;
            invincibleTime = 180;
        }
    }

    IEnumerator Respawn()
    {
        if (BackgroundMusicScript.Instance != null)
        {
            BackgroundMusicScript.Instance.StopMusic();

            if (FindObjectOfType<Camera_Controller_MinibossEvent>() != null)
            {
                BackgroundMusicScript.Instance.BGM.clip = FindObjectOfType<Camera_Controller_MinibossEvent>().standardMusic;
            }
        }
        Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
        invincibleTime = 0;
        isInvincible = false;
        player.enabled = false;
        player.canInput = false;
        pauseGameScript.canPause = false;
        player.activeForm = Player_Controller.Forms.Artful;
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForSeconds(1);
        levelManager.animator.SetTrigger("FadeOutReset");
        yield return new WaitForSeconds(1);
        if (FindObjectOfType<Camera_Controller_MinibossEvent>() != null)
        {
            FindObjectOfType<Camera_Controller_MinibossEvent>().ResetBoss();
        }
        if (FindObjectOfType<PowerFragmentEventScript>() != null)
        {
            FindObjectOfType<PowerFragmentEventScript>().ResetTrap();
        }
        livesLeft--;
        if (FindObjectOfType<PowerFragmentScript>() != null)
        {
            FindObjectOfType<PowerFragmentScript>().collected = false;
            powerFragments[FindObjectOfType<PowerFragmentScript>().id] = 0;

        }
        currentPlayerHealth = maxPlayerHealth;
        player.transform.position = levelManager.levelSceneSpawnPoint.position;
        player.enabled = true;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.gameObject.GetComponent<Rigidbody2D>().gravityScale = gravityStore;
        player.canInput = true;
        levelManager.ReactivateDisabledObjects();
        yield return new WaitForSeconds(1);
        levelManager.animator.SetTrigger("FadeIn");
        if (BackgroundMusicScript.Instance != null)
        {
            BackgroundMusicScript.Instance.PlayMusic();
        }
        isDead = false;




    }

    IEnumerator EndGame()
    {
        if (BackgroundMusicScript.Instance != null)
        {
            BackgroundMusicScript.Instance.StopMusic();
        }
        Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
        invincibleTime = 0;
        isInvincible = false;
        player.enabled = false;
        player.canInput = false;
        pauseGameScript.canPause = false;
        player.activeForm = Player_Controller.Forms.Artful;
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        ResetPlayer();
        yield return new WaitForSeconds(1);
        levelManager.ReactivateDisabledObjects();
        levelManager.FadeToLevelScene(7);




    }

    public void SavePlayer()
    {
        GlobalControlScript.Instance.currentPlayerHealth = currentPlayerHealth;
        GlobalControlScript.Instance.colorDrops = colorDrops;
        GlobalControlScript.Instance.livesLeft = livesLeft;
        GlobalControlScript.Instance.powerFragments = powerFragments;

    }

    public void ResetPlayer()
    {
        GlobalControlScript.Instance.currentPlayerHealth = 6;
        GlobalControlScript.Instance.colorDrops = 0;
        GlobalControlScript.Instance.livesLeft = 2;

        int i = 0;

        foreach (int fragment in powerFragments)
        {

            GlobalControlScript.Instance.powerFragments[i] = 0;
            i++;


        }


    }
}
