using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHand : MonoBehaviour
{
    public bool isLocationLocked = true;
    private bool isReturning;
    private float temp;

    private GameObject handPoint;
    private Vector2 handPointLocation;

    private GameObject targetPoint;
    private Vector2 targetPointLocation;

    private float count;
    private float reachThreshold = 0.3f;

    private float holdingCount;
    public float holdingMax = 2f;
    private bool isHolding;

    public Player_Control playerControls;

    //GRABBING VARIABLES
    public bool isGrabbed;
    public Transform leftHandPosition;

    private BoxCollider2D objectCollider;

    // Start is called before the first frame update
    void Start()
    {
        handPoint = GameObject.Find("L_HandPoint");
        targetPoint = GameObject.Find("TargetPoint02");

        playerControls = GameObject.Find("Player").GetComponent<Player_Control>();
        objectCollider = GetComponent<BoxCollider2D>();
        objectCollider.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(isLocationLocked == true)
        {

            StopAllCoroutines();
            isGrabbed = false;
            isHolding = false;
            isReturning = false;
            transform.position = handPoint.transform.position;
            count = 0;
            holdingCount = 0f;
            objectCollider.enabled = false;
        }
        
        if(isHolding && count == 0) 
        {
            StopAllCoroutines();
            StartCoroutine(GrabHold());
            count += 1;
        
        }
        else if(isReturning && count == 0) 
        {
            StopAllCoroutines();
            StartCoroutine(ReturnToPlayer());
            count += 1;
        }

    }



    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        temp += 1;
        Debug.Log("we hit: " + collision.gameObject.name + temp);

        Obj_Grab grabbedObject;
        grabbedObject = collision.gameObject.GetComponent<Obj_Grab>();

        if(grabbedObject == null) 
        {
            Debug.LogError("GRABBED OBJECT = NULL");

            return;
   
         
        }
        if (isReturning == false)
        {

            isGrabbed = true;
            grabbedObject.StartCoroutine(grabbedObject.MoveWithFist());
            Debug.Log("we hit: " + collision.gameObject.name);
            isHolding = true;

        }


       
     /*
      IF the collison object has the grabbed script on it then
      isGrabbed = true
      isreturning = true
      */  



    }


    public IEnumerator MoveFist(float chargeTime)
    {
        //Actives the hit box for the object;
        objectCollider.enabled = true;

        isLocationLocked = false;
        targetPointLocation = targetPoint.transform.position;
        //Debug.Log("TARGET LOCATION is" + targetPointLocation);

        while ((Vector3.Distance(transform.position, targetPointLocation) > reachThreshold))
        {

            transform.position = Vector3.MoveTowards(transform.position, targetPointLocation, playerControls.leftPunchSpeed * Time.deltaTime);
            yield return null;
        }
        isReturning = true;

    }

    public IEnumerator GrabHold() 
    {
        while(holdingCount < holdingMax) 
        {
            holdingCount += 1f * Time.deltaTime;
            yield return null;

        }



        isHolding = false;
        isReturning = true;
        count = 0;
    
    
    }

    public IEnumerator ReturnToPlayer()
    {
        objectCollider.enabled = false;

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
