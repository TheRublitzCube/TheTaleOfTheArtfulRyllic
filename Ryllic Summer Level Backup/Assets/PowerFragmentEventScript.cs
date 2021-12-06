using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerFragmentEventScript : MonoBehaviour {

    Camera_Controller cameraController;
    GameObject player;
    public Transform cameraPoint;
    public GameObject[] invisibleWalls;
    public GameObject[] enemies;
    public GameObject powerFragment;
    int powerFragmentId;
    public GameObject spawnParticle;
    bool deactivatedTrap = false;
    bool triggeredTrap = false;
    bool fragmentAppear = false;

    // Use this for initialization
    void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        powerFragment.SetActive(false);
        cameraController = FindObjectOfType<Camera_Controller>();
        cameraController.trapArea = transform.gameObject;
        invisibleWalls = GameObject.FindGameObjectsWithTag("InvisibleWalls");
        powerFragmentId = player.GetComponent<PlayerHealthManager>().powerFragments[powerFragment.GetComponentInChildren<PowerFragmentScript>().id];


        foreach (GameObject wall in invisibleWalls)
        {

            wall.SetActive(false);
            Debug.Log("Wall is disabled");

        }

        int i = 0;
        foreach (GameObject enemy in enemies)
        {
            
            enemy.SetActive(false);
            i++;

        }

    }
	
	// Update is called once per frame
	void Update () {

        if (triggeredTrap && AreEnemiesDead() && !deactivatedTrap && fragmentAppear == false)
        {
            powerFragment.SetActive(true);
            fragmentAppear = true;
        }

        if (fragmentAppear == true && player.GetComponent<PlayerHealthManager>().powerFragments[powerFragmentId] == 1)
        {
            cameraController.currentState = Camera_Controller.States.Following;

            foreach (GameObject wall in invisibleWalls)
            {
                wall.SetActive(false);
            }

            deactivatedTrap = true;

            
        }



    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && triggeredTrap == false)
        {
            cameraController.currentState = Camera_Controller.States.Trapped;
            StartCoroutine("SpawnEnemies");

        }
    }

    IEnumerator SpawnEnemies() {

        foreach (GameObject wall in invisibleWalls)
        {

            wall.SetActive(true);

        }

        foreach (GameObject enemy in enemies)
        {
            Instantiate(spawnParticle, enemy.transform.position, enemy.transform.rotation);
        }

        yield return new WaitForSeconds(2.5f);

        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }

        triggeredTrap = true;
    }

    public void ResetTrap()
    {
        int i = 0;
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
            i++;
        }

        foreach (GameObject wall in invisibleWalls)
        {
            wall.SetActive(false);
        }

        triggeredTrap = false;
        deactivatedTrap = false;
        fragmentAppear = false;
        powerFragment.SetActive(false);
        powerFragment.GetComponentInChildren<PowerFragmentScript>().collected = false;
    }

    bool AreEnemiesDead()
    {
        int enemyCounter = 0;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf == true)
            {
                enemyCounter++;
            }
        }

        if (enemyCounter == 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
