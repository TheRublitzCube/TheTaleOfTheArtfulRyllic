using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Controller_MinibossEvent : MonoBehaviour {

    Camera_Controller cameraController;
    GameObject player;
    Color originalColor;
    float originalSpinSpeed;
    GameObject[] invisibleWalls;
    GameObject miniboss;
    public GameObject particle;
    public GameObject deathParticle;
    bool minibossDefeated;
    bool playerHasDied;
    Vector2 originalPosition;
    public string minibossName;
    public AudioClip standardMusic;
    public AudioClip bossMusic;

    public Slider bossHealth;
    public Text bossName;


	// Use this for initialization
	void Start () {


        cameraController = FindObjectOfType<Camera_Controller>();
        miniboss = GameObject.FindGameObjectWithTag("Miniboss");
        originalPosition = miniboss.transform.position;
        cameraController.minibossArea = transform.gameObject;
        invisibleWalls = GameObject.FindGameObjectsWithTag("InvisibleWalls");

        minibossDefeated = false;

        miniboss.SetActive(false);

        playerHasDied = false;
        
        foreach (GameObject wall in invisibleWalls)
        {

            wall.SetActive(false);

        }
        
        bossHealth.gameObject.SetActive(false);
        bossName.text = "";
    }
	
	// Update is called once per frame
	void Update () {

        bossHealth.value = miniboss.GetComponent<FungillScript>().health;


        if (miniboss.activeSelf == false && miniboss.GetComponent<FungillScript>().health <= 0 && !minibossDefeated)
        {

            minibossDefeated = true;

            bossHealth.gameObject.SetActive(false);

            bossName.text = "";

            Instantiate(deathParticle, miniboss.transform.position, deathParticle.transform.rotation);

            cameraController.currentState = Camera_Controller.States.Following;

            foreach (GameObject wall in invisibleWalls)
            {

                wall.SetActive(false);

            }

            BackgroundMusicScript.Instance.ChangeBGM(standardMusic);

        }
		
	}

    IEnumerator Spawn()
    {
        bossHealth.gameObject.SetActive(true);

        bossName.text = minibossName;

        Instantiate(particle, miniboss.transform.position, miniboss.transform.rotation);

        yield return new WaitForSeconds(2);

        miniboss.SetActive(true);


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && cameraController.currentState == Camera_Controller.States.Following && minibossDefeated == false)
        {
            BackgroundMusicScript.Instance.ChangeBGM(bossMusic);

            StartCoroutine("Spawn");

            cameraController.currentState = Camera_Controller.States.Miniboss;
            
            foreach (GameObject wall in invisibleWalls)
            {

                wall.SetActive(true);

            }

        }


    }

    public void ResetBoss()
    {

        bossHealth.gameObject.SetActive(false);
        bossName.text = "";

        miniboss.transform.position = originalPosition;

        miniboss.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        miniboss.transform.localScale = new Vector3(Mathf.Abs(miniboss.transform.localScale.x), Mathf.Abs(miniboss.transform.localScale.y), Mathf.Abs(miniboss.transform.localScale.z));

        foreach (GameObject wall in invisibleWalls)
        {

            wall.SetActive(false);

        }

        miniboss.GetComponent<FungillScript>().health = miniboss.GetComponent<FungillScript>().maxHealth;

        if (miniboss.GetComponent<FungillScript>().currentState != FungillScript.BossState.Idle)
        {
            miniboss.GetComponent<FungillScript>().currentState = FungillScript.BossState.Idle;

        }

        miniboss.SetActive(false);


    }
}
