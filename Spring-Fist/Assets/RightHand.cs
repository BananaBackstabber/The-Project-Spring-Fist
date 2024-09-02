using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHand : MonoBehaviour
{

    public bool islocationLock = true;
    
    public GameObject handPoint;
    public GameObject TargetPoint;

    private Vector2 handPointLocation;
    private Vector2 targetPointLocation;
    private bool isReturning;

    private float count;
    private float reachThreshold = 0.3f;

    public Player_Control playerControls;
  

    private void Start()
    {

        playerControls = GameObject.Find("Player").GetComponent<Player_Control>();

        handPoint = GameObject.Find("R_HandPoint");
        TargetPoint = GameObject.Find("TargetPoint");

        handPointLocation = handPoint.transform.position;
        targetPointLocation = TargetPoint.transform.position;

        islocationLock = true;

    }
    private void Update()
    {

        //lock right hand from moving
        //happens after every charged punch
        if (islocationLock == true) 
        {

            StopAllCoroutines();
            isReturning = false;

            transform.position = handPoint.transform.position;
            count = 0;
        }

        if (isReturning && count == 0) 
        {
            
            StopAllCoroutines();
            StartCoroutine(ReturnToPlayer());
            count += 1;
        }

        if (Input.GetKeyDown("t")) 
        {

            islocationLock = !islocationLock;
           // Debug.Log("Locationlock is" + islocationLock);

        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Optional: Handle collision logic here, e.g., damage enemies
        if (!isReturning)
        {
            // Hit something while moving towards the target, immediately return
            StopAllCoroutines();
            StartCoroutine(ReturnToPlayer());
        }
    }


    public IEnumerator MoveFist(float chargeTime)
    {
        //TargetPoint.transform.localPosition = TargetPoint.transform.localPosition * playerControls.punchDistance;
        //Debug.Log("Move STARTED");
        islocationLock = false;
        targetPointLocation = TargetPoint.transform.position;
        Debug.Log("TARGET LOCATION is" + targetPointLocation);

        while ((Vector3.Distance(transform.position, targetPointLocation) > reachThreshold))
        {

            transform.position = Vector3.MoveTowards(transform.position, targetPointLocation, playerControls.punchSpeed * Time.deltaTime);
            yield return null;
        }

       // Debug.Log("Move END");
        isReturning = true;

        /*while ((targetPointLocation - handPointLocation).sqrMagnitude > 0.1f)
        {

            transform.position = Vector3.MoveTowards(transform.position, targetPointLocation, 5f * Time.deltaTime);
            yield return null;

        }
        Debug.Log("ACTIVE3");
        isReturning = false;
        islocationLock = true;*/

    }

    public IEnumerator ReturnToPlayer() 
    {

       // Debug.Log("RETURN STARTED");
        
        handPointLocation = handPoint.transform.position;
        while ((Vector3.Distance(transform.position, handPointLocation) > reachThreshold)) 
        {
            //Debug.Log("DISTANCE TO = " + Vector3.Distance(transform.position, handPointLocation));

            transform.position = Vector3.MoveTowards(transform.position, handPointLocation, playerControls.punchSpeed * Time.deltaTime);
            yield return null;


        }
       // Debug.Log("RETURN END");
        islocationLock = true;


    }




}
