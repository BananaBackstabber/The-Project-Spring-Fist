using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHand : MonoBehaviour
{

    public bool islocationLock;
    
    public GameObject handPoint;
    public GameObject TargetPoint;

    private Vector2 handPointLocation;
    private Vector2 targetPointLocation;

    private void Start()
    {

        handPoint = GameObject.Find("R_HandPoint");
        TargetPoint = GameObject.Find("TargetPoint");

        handPointLocation = handPoint.transform.position;
        targetPointLocation = TargetPoint.transform.position;

    }

    private void Update()
    {



        if (islocationLock) 
        {
            transform.position = handPoint.transform.position;
        }
        else
        {
            //transform.position = targetPoint.transform.position;

        }

        if (Input.GetKeyDown("t")) 
        {

            islocationLock = !islocationLock;
            Debug.Log("Locationlock is" + islocationLock);

        }

    }




}
