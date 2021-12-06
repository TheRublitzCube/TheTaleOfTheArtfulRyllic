using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismiteScript : MonoBehaviour
{

    Vector2 warpPos;
    GameObject player;
    Camera_Controller mainCamera;
    enum States { Idling, Warping, Shooting };
    States currentState;
    bool isWarping;


    // Use this for initialization
    void Start()
    {
        mainCamera = GameObject.FindObjectOfType<Camera_Controller>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentState = States.Warping;
        isWarping = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (currentState == States.Warping && isWarping == false)
        {
            StartCoroutine("Warp");
            isWarping = true;
        }

    }

    IEnumerator Warp()
    {

        //Color tmp = GetComponent<SpriteRenderer>().color;

        //while (GetComponent<SpriteRenderer>().color.a != 0)
        //{
        //    tmp.a -= 0.01f;
        //    GetComponent<SpriteRenderer>().color = tmp;
        //}
        yield return new WaitForSeconds(3);
        

        float screenX = Random.Range(0.0f, Camera.main.pixelWidth * .88f);
        float screenY = Random.Range(0.0f, Camera.main.pixelHeight * .78f);


        Vector2 randomPoint = Camera.main.ScreenToWorldPoint(new Vector2(screenX, screenY));

        //if (Mathf.Abs(difference.x) <= radius && difference.x >= 0f) //if the x distance from the random point and the player is less than/equal to the radius and x difference is greater than 0
        //{
        //    randomPoint.x += radius;
        //}

        //else if (Mathf.Abs(difference.x) <= radius && difference.x <= 0f) //if the x distance from the random point and the player is less than/equal to the radius and x difference is less than 0
        //{
        //    randomPoint.x -= radius;
        //}

        //if (Mathf.Abs(difference.y) <= radius && difference.x >= 0f) //if the y distance from the random point and the player is less than/equal to the radius and y difference is greater than 0
        //{
        //    randomPoint.y += radius;
        //}

        //else if (Mathf.Abs(difference.y) <= radius && difference.x <= 0f) //if the y distance from the random point and the player is less than/equal to the radius and y difference is greater than 0
        //{
        //    randomPoint.y -= radius;
        //}

        transform.position = randomPoint;

        //while (GetComponent<SpriteRenderer>().color.a != 1)
        //{
        //    tmp.a += 0.01f;
        //    GetComponent<SpriteRenderer>().color = tmp;
        //}

        isWarping = false;

    }
}
