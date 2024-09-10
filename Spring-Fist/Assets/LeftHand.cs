using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHand : MonoBehaviour
{
    [HideInInspector]
    public bool isLocationLocked = true;
    public bool isReturning;
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
    private bool isPulled;
    private Vector2 collisionPoint;

    private GameObject player;

    private float dCount;


    private BoxCollider2D objectCollider;
    //public LayerMask;

    // Start is called before the first frame update
    void Start()
    {
        handPoint = GameObject.Find("L_HandPoint");
        targetPoint = GameObject.Find("TargetPoint02");
        player = GameObject.Find("Player");

        playerControls = GameObject.Find("Player").GetComponent<Player_Control>();
        objectCollider = GetComponent<BoxCollider2D>();
        objectCollider.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {


        if (!isLocationLocked) 
        {
            dCount += 1;
            //Rotates the fist to face the target point location while the hand is moving;
            Vector2 Direction = (targetPointLocation - handPointLocation).normalized;
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        }
        else 
        {
            dCount += 1;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }

        

 
        /* if (!isReturning) 
         {
             RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPointLocation, 0.3f);
             if (hit.collider.gameObject.CompareTag("Wall"))
             {
                 StopAllCoroutines();
                 StartCoroutine(pullPlayer());
                 collisionPoint = hit.point;

                 Debug.Log("Point of Contact:" + hit.point);

             }

         }*/


        if (isLocationLocked == true)
        {
            

            StopAllCoroutines();
            isGrabbed = false;
            isHolding = false;
            isReturning = false;
            transform.position = handPoint.transform.position;
            count = 0;
            holdingCount = 0f;
            dCount = 0f;

            playerControls.isMoving = true;
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


    private void LateUpdate()
    {
        if (dCount < 2f)
        {
            //Debug.Log(dCount);
            targetPointLocation = targetPoint.transform.position;//DON'T DELETE IT MESSSES WITH THE AIM

        }
        else 
        {
            Debug.Log("NO COUNT");
        }
    }
    private void OnDrawGizmos()
    {

        if(targetPoint == null) 
        {
            return;
        
        }
        Vector2 origin;
        Vector2 endPoint;
        if (playerControls.isFacingRight) 
        {
              origin = player.transform.localPosition;
              endPoint = origin + new Vector2(targetPoint.transform.localPosition.x, targetPoint.transform.localPosition.y) * 5f;
        }
        else 
        {
           origin = player.transform.localPosition;
           endPoint = origin - new Vector2(targetPoint.transform.localPosition.x,targetPoint.transform.localPosition.y) * 5f;

        }
        

        //Changes the gizmos colour on hit
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, endPoint);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        StopAllCoroutines();

        
        temp += 1;
        //Debug.Log("we hit: " + collision.gameObject.name + temp);

        Obj_Grab grabbedObject;
        grabbedObject = collision.gameObject.GetComponent<Obj_Grab>();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPointLocation, 4f);
        if (hit.collider != null)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                collisionPoint = hit.point;
                Debug.Log("Point of Contact:" + hit.point);

                this.GetComponent<Rigidbody2D>().isKinematic = true;

                StopAllCoroutines();
                StartCoroutine(pullPlayer());

                return;
            }
            
        }

        

        if(grabbedObject == null)//If n object is not Grabable
        {
            Debug.LogError("GRABBED OBJECT = NULL");

            isReturning = true;//stop it from frezze if it hits a non grabable object
            //return;
   
         
        }
        else if (grabbedObject && isReturning == false)
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if (collision.gameObject.CompareTag("Wall"))
        {

            transform.position = collisionPoint;

           

           // StopAllCoroutines();
            //StartCoroutine(PullPlayer());

        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        ContactPoint2D point = collision.GetContact(0);
    }




    public IEnumerator MoveFist(float chargeTime)
    {

        //Actives the hit box for the object;
        objectCollider.enabled = true;


        Debug.Log("TARGET PPoint LOCATION" + targetPoint.transform.localPosition);
        isLocationLocked = false;
        targetPointLocation = targetPoint.transform.position;
        Debug.Log("LOCATION IS NOW " + targetPointLocation);
        //Debug.Log("TARGET LOCATION is" + targetPointLocation);
        //targetPointLocation = targetPoint.transform.position;

        while ((Vector3.Distance(transform.position, targetPointLocation) > reachThreshold))
        {
            

            transform.position = Vector3.MoveTowards(transform.position, targetPointLocation, playerControls.leftPunchSpeed  * Time.deltaTime);
            yield return null;
        }
        isReturning = true;

    }

    public IEnumerator pullPlayer()
    {
        Debug.Log("HIT PULL");

        player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;

        while (Vector3.Distance(player.transform.position, transform.position) > reachThreshold * 2)
        {
            playerControls.isMoving = false;

            player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, playerControls.leftPunchSpeed / 4 * Time.deltaTime);
            yield return null;

        }

        //Debug.Log("PULL HAS STopped");
        //isLocationLocked = true;


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

            transform.position = Vector3.MoveTowards(transform.position, handPointLocation, playerControls.leftPunchSpeed/2 * Time.deltaTime);
            yield return null;


        }
        // Debug.Log("RETURN END");
        isLocationLocked = true;

        // Debug.Log("RETURN STARTED");

   

    }
}
