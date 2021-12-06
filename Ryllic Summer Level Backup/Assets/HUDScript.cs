using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HUDScript : MonoBehaviour {

    GameObject player;
    public Image healthBarUI;
    public Sprite[] healthBarSprites;
    public Color[] healthBarColor;
    public Image[] fragmentIcons;
    public Text colorDropText;
    public Text lifeText;

	// Use this for initialization
	void Start () {

        player = GameObject.Find("Player");

		
	}
	
	// Update is called once per frame
	void Update () {

        int i = 0; 
        foreach(int fragment in player.GetComponent<PlayerHealthManager>().powerFragments)
        {
            if (fragment == 0)
            {
                fragmentIcons[i].color = Color.black;
            }

            else
            {
                fragmentIcons[i].color = Color.white;
            }
            i++;
        }

        lifeText.text = "Lives: " + player.GetComponent<PlayerHealthManager>().livesLeft;

        int currentForm = (int)player.GetComponent<Player_Controller>().activeForm;

        healthBarUI.sprite = healthBarSprites[player.GetComponent<PlayerHealthManager>().currentPlayerHealth];

        healthBarUI.color = healthBarColor[currentForm];

        if (player.GetComponent<PlayerHealthManager>().colorDrops < 10)
        {
            colorDropText.text = "x " + "0" + player.GetComponent<PlayerHealthManager>().colorDrops; 
        }

        else
        {
            colorDropText.text = "x " + player.GetComponent<PlayerHealthManager>().colorDrops;
        }

    }
}
