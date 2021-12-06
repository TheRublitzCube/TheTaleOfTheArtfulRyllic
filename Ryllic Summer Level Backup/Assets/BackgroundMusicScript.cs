using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicScript : MonoBehaviour {

    public AudioSource BGM;
    public static BackgroundMusicScript Instance;

	// Use this for initialization
	void Start () {

        BGM = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Instance.ChangeBGM(BGM.clip);
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeBGM(AudioClip music)
    {

        if (BGM.clip.name == music.name)
        {
            return;
        }
        
        if (music == null)
        {
            BGM.Stop();
            return;
        }
        

        BGM.Stop();
        BGM.clip = music;
        BGM.Play();

    }

    public void StopMusic()
    {

        BGM.Stop();

    }

    public void PlayMusic()
    {

        BGM.Play();
    }

    public void PauseMusic()
    {
        BGM.Pause();

    }


}
