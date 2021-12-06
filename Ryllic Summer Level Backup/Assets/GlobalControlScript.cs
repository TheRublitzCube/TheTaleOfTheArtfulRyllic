using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalControlScript : MonoBehaviour {

    public static GlobalControlScript Instance;
    

    public int currentPlayerHealth;
    public int colorDrops;
    public int livesLeft;
    public int[] powerFragments;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
