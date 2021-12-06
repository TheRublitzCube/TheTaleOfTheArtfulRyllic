using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CandleForm_Controller : MonoBehaviour {

    Player_Controller player;
    GameObject[] ghostlyMovingPlatforms;
    GameObject ghostlyPlatforms;
    public GameObject candleGlow;
    public GameObject candleGlowMask;

	// Use this for initialization
	void Start () {

        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();
        ghostlyPlatforms = GameObject.FindGameObjectWithTag("GhostlyPlatform");
        ghostlyMovingPlatforms = GameObject.FindGameObjectsWithTag("GhostlyMovingPlatform");




    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (player.activeForm != Player_Controller.Forms.Candle)
        {

            candleGlow.SetActive(false);
            candleGlowMask.SetActive(false);

            if (ghostlyPlatforms != null)
            {

                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ghostlyPlatforms.GetComponent<CompositeCollider2D>(), true);
                ghostlyPlatforms.layer = 10;
            }

            if (ghostlyMovingPlatforms.Length != 0)
            {

                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GetClosestGhost(ghostlyMovingPlatforms).GetComponent<Collider2D>(), true);
                foreach (GameObject platform in ghostlyMovingPlatforms)
                {
                    platform.layer = 10;
                }
            }

        }

        else
        {

            candleGlow.SetActive(true);
            candleGlowMask.SetActive(true);

            if (ghostlyPlatforms != null)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ghostlyPlatforms.GetComponent<CompositeCollider2D>(), false);
                ghostlyPlatforms.layer = 8;
            }

            if (ghostlyMovingPlatforms.Length != 0)
            {

                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GetClosestGhost(ghostlyMovingPlatforms).GetComponent<Collider2D>(), false);
                foreach (GameObject platform in ghostlyMovingPlatforms)
                {
                    platform.layer = 8;
                }
            }

        }


    }



    GameObject GetClosestGhost(GameObject[] ghosts)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in ghosts)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
}
