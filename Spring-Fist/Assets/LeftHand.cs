using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHand : MonoBehaviour
{
    public bool isLocationLocked = true;
    private bool isReturning;


    private GameObject handPoint;
    private Vector2 handPointLocation;

    private GameObject targetPoint;
    private Vector2 targetPointLocation;

    private float count;
    private float reachThreshold = 0.3f;

    public Player_Control playerControls;

    // Start is called before the first frame update
    void Start()
    {
        handPoint = GameObject.Find("L_HandPoint");
        targetPoint = GameObject.Find("TargetPoint02");

        playerControls = GameObject.Find("Player").GetComponent<Player_Control>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if(isLocationLocked == true)
        {

            StopAllCoroutines();
            isReturning = false;
            transform.position = handPoint.transform.position;
            count = 0;
        }

        if(isReturning && count == 0) 
        {
            StopAllCoroutines();
            StartCoroutine(ReturnToPlayer());
            count += 1;
        }

    }

    public IEnumerator MoveFist(float chargeTime)
    {
        isLocationLocked = false;
        targetPointLocation = targetPoint.transform.position;
        //Debug.Log("TARGET LOCATION is" + targetPointLocation);

        while ((Vector3.Distance(transform.position, targetPointLocation) > reachThreshold))
        {

            transform.position = Vector3.MoveTowards(transform.position, targetPointLocation, playerControls.rightPunchSpeed * Time.deltaTime);
            yield return null;
        }
        isReturning = true;

    }

    public IEnumerator ReturnToPlayer()
    {
        handPointLocation = handPoint.transform.position;
        while ((Vector3.Distance(transform.position, handPointLocation) > reachThreshold))
        {
            //Debug.Log("DISTANCE TO = " + Vector3.Distance(transform.position, handPointLocation));

            transform.position = Vector3.MoveTowards(transform.position, handPointLocation, playerControls.rightPunchSpeed * Time.deltaTime);
            yield return null;


        }
        // Debug.Log("RETURN END");
        isLocationLocked = true;

        // Debug.Log("RETURN STARTED");

   

    }
}
