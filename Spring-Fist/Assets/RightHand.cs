using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHand : MonoBehaviour
{

    public bool isLocationLocked = true;
    
    //HANDPOINT A
    public GameObject handPoint;
    private Vector2 handPointLocation;
    //TARGET POINT B 
    public GameObject TargetPoint;
    private Vector2 targetPointLocation;
    private bool isReturning;

    //Charge Punch Movement floats
    private float count;
    private float reachThreshold = 0.3f;

    public Player_Control playerControls;
    public Transform punchOrigin;
  

    private void Start()
    {

        playerControls = GameObject.Find("Player").GetComponent<Player_Control>();

        handPoint = GameObject.Find("R_HandPoint");
        TargetPoint = GameObject.Find("TargetPoint01");

        handPointLocation = handPoint.transform.position;
        targetPointLocation = TargetPoint.transform.position;

        isLocationLocked = true;

    }
    private void Update()
    {

        //lock right hand from moving
        //happens after every charged punch
        if (isLocationLocked == true) 
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

            isLocationLocked = !isLocationLocked;
           // Debug.Log("Locationlock is" + islocationLock);

        }

    }
    public IEnumerator MoveFist(float chargeTime)
    {
        //TargetPoint.transform.localPosition = TargetPoint.transform.localPosition * playerControls.punchDistance;
       // Debug.Log("Move STARTED");
        isLocationLocked = false;
        targetPointLocation = TargetPoint.transform.position;
        //Debug.Log("TARGET LOCATION is" + targetPointLocation);

        while ((Vector3.Distance(transform.position, targetPointLocation) > reachThreshold))
        {

            transform.position = Vector3.MoveTowards(transform.position, targetPointLocation, playerControls.rightPunchSpeed * Time.deltaTime);
            yield return null;
        }

        //Debug.Log("Move END");
        isReturning = true;

      

    }

    public IEnumerator ReturnToPlayer() 
    {

       // Debug.Log("RETURN STARTED");
        
        handPointLocation = handPoint.transform.position;
        while ((Vector3.Distance(transform.position, handPointLocation) > reachThreshold)) 
        {
            //Debug.Log("DISTANCE TO = " + Vector3.Distance(transform.position, handPointLocation));

            transform.position = Vector3.MoveTowards(transform.position, handPointLocation, playerControls.rightPunchSpeed * Time.deltaTime);
            yield return null;


        }
       //Debug.Log("RETURN END");
        isLocationLocked = true;


    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(" HIT TRIGGER");

        //punchOrigin Origin = current position of the fist 
        punchOrigin = transform;

        // Optional: Handle collision logic here, e.g., damage enemies
        if (!isReturning)
        {
            Debug.Log(" KNOCK TRIGGER");
            Enemy01 knockback = collision.GetComponent<Enemy01>();
            // Calculate direction from PunchOrigin to the object being punched
            Vector2 knockbackDirection = (collision.transform.position - punchOrigin.transform.position).normalized;

            //Apply knockback to calculated direction
            knockback.ApplyKnockBack(knockbackDirection);

            // Hit something while moving towards the target, immediately return
            StopAllCoroutines();
            StartCoroutine(ReturnToPlayer());
        }
    }




}
