using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{


    public GameObject platform;
    public float moveSpeed;
    public Transform currentPoint;
    public Transform[] points;
    public int pointSelection;
    bool switching = false;
    public float waitTime;




    // Use this for initialization
    void Start()
    {

        currentPoint = points[pointSelection];

    }

    // Update is called once per frame
    void FixedUpdate()
    {


        platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);

        if (platform.transform.position == currentPoint.position && !switching)
        {

            StartCoroutine("Switch");
            switching = true;

        }

    }

    IEnumerator Switch()
    {


        yield return new WaitForSeconds(waitTime);

        pointSelection++;

        if (pointSelection == points.Length)
        {
            pointSelection = 0;
        }

        currentPoint = points[pointSelection];

        switching = false;
    }
}

