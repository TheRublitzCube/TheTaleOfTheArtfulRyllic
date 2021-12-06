using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    //Camera following variables
    public Transform target; //what the camera is following
    Transform playerTarget;
    public Transform highXPosition;
    public Transform highYPosition;
    public Transform lowestYPosition;
    public Transform lowestXPosition;
    public float smoothing; //dampening effect
    public float smoothingLock; //dampening effect
    Vector3 offset;
    float lowX;
    float lowY;
    float highX;
    float highY;

    public enum States {Following, Miniboss, Trapped};
    public States currentState;
    public GameObject minibossArea;
    public GameObject trapArea;
    bool minibossChecked;
    bool trapChecked = false;

    // Use this for initialization
    void Start()
    {
        playerTarget = target;

        offset = transform.position - target.position; //difference in position of camera and player
        

        if (lowestXPosition != null)
        {
            lowX = lowestXPosition.position.x; //lowest x position
        }

        else
        {
            lowX = transform.position.x;
        }

        if (lowestYPosition != null)
        {
            lowY = lowestYPosition.position.y; //lowest y position
        }

        else
        {
            lowY = transform.position.y;
        }

        if (highXPosition != null)
        {
            highX = highXPosition.position.x; //highest x position
        }

        else
        {
            highX = transform.position.x;
        }

        if (highYPosition != null)
        {
            highY = highYPosition.position.y; //highest y position
        }

        else
        {
            highY = transform.position.y;
        }

        currentState = States.Following;


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (minibossChecked == false )
        {
            if (minibossArea == null)
            {
                Destroy(minibossArea);
            }

            minibossChecked = true;
        }


        if (trapArea == null && trapChecked == false)
        {
            Destroy(trapArea);
            trapChecked = true;
        }

        if (currentState == States.Following)
        {

            Vector3 targetCamPos = target.position + offset;

            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

            if (transform.position.x < lowX)
            {
                transform.position = new Vector3(lowX, transform.position.y, transform.position.z);
            }

            if (transform.position.x > highX)
            {
                transform.position = new Vector3(highX, transform.position.y, transform.position.z);
            }

            if (transform.position.y > highY)
            {
                transform.position = new Vector3(transform.position.x, highY, transform.position.z);
            }

            if (transform.position.y < lowY)
            {
                transform.position = new Vector3(transform.position.x, lowY, transform.position.z);
            }
        }

        else if (currentState == States.Miniboss)
        {

            transform.position = Vector3.Lerp(transform.position, minibossArea.transform.position, smoothingLock * Time.deltaTime);

        }

        else if (currentState == States.Trapped)
        {

            transform.position = Vector3.Lerp(transform.position, trapArea.GetComponent<PowerFragmentEventScript>().cameraPoint.transform.position, smoothingLock * Time.deltaTime);

        }

    }
}
