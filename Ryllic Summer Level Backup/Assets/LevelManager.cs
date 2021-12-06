using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public Animator animator;
    GameObject player;

    public Transform levelSceneSpawnPoint;

    private int levelSceneToLoad;

    public GameObject enemies;

    public GameObject commonPickups;

    public PauseGameScript pauseGameScript;
    

    Camera_Controller cameraController;

    public AudioClip newTrack;



    // Use this for initialization
    void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        levelSceneSpawnPoint.position = player.transform.position;
        animator = gameObject.GetComponent<Animator>();
        pauseGameScript = GameObject.Find("PauseMenu").GetComponent<PauseGameScript>();
        cameraController = FindObjectOfType<Camera_Controller>();

        if (newTrack != null)
        {

            BackgroundMusicScript.Instance.ChangeBGM(newTrack);

        }


    }

	// Update is called once per frame
	void Update () {

    }



    public void FadeToLevelScene(int levelIndex)
    {
        pauseGameScript.canPause = false;
        levelSceneToLoad = levelIndex;
        animator.SetTrigger("FadeOut");

        if (levelIndex != 8 || levelIndex != 7)
        {
            player.GetComponent<PlayerHealthManager>().SavePlayer();
        }

        else {
            player.GetComponent<PlayerHealthManager>().ResetPlayer();
        }
    }

    public void OnFadeOutSceneSwitchComplete()
    {
        SceneManager.LoadScene(levelSceneToLoad);
    }

    public void OnFadeOutSceneResetComplete()
    {
        if (cameraController.currentState != Camera_Controller.States.Following)
        {
            cameraController.currentState = Camera_Controller.States.Following;
        }

        animator.ResetTrigger("FadeOutReset");
    }

    public void OnFadeInSceneResetComplete()
    {

        pauseGameScript.canPause = true;
    }

    public void ReactivateDisabledObjects()
    {
        for (int a = 0; a < enemies.transform.childCount; a++)
        {
            enemies.transform.GetChild(a).gameObject.SetActive(true);
        }

        for (int a = 0; a < commonPickups.transform.childCount; a++)
        {
            commonPickups.transform.GetChild(a).gameObject.SetActive(true);
        }
    }
}
