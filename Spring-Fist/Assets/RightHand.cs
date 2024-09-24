using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class RightHand : MonoBehaviour
{

    private AudioSource Hit;

    private LeftHand scriptLH;
    private Collider2D objectColiider;

    public bool isLocationLocked = true;
    //HANDPOINT A
    public GameObject handPoint;
    private Vector2 handPointLocation;
    //TARGETPOINT B 
    public GameObject TargetPoint;
    private Vector2 targetPointLocation;
    private bool isReturning;


    //Charge Punch Movement floats
    private float count;
    private float reachThreshold = 0.3f;
    public Player_Control playerControls;
    public Transform punchOrigin;



    private void Awake()
    {
        scriptLH = GameObject.Find("LeftHand").GetComponent<LeftHand>();
        objectColiider = GetComponent<Collider2D>();
        playerControls = GameObject.Find("Player").GetComponent<Player_Control>();
        handPoint = GameObject.Find("R_HandPoint");
        TargetPoint = GameObject.Find("TargetPoint01");

        handPointLocation = handPoint.transform.position;
        targetPointLocation = TargetPoint.transform.position;

        isLocationLocked = true;
        objectColiider.enabled = false;

    }
  
    private void Update()
    {

        

        if (!isLocationLocked)
        {
            Vector2 Direction = (targetPointLocation - handPointLocation).normalized;
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        }
        else
        {

            //Keeps the hands position on the player
            transform.position = handPoint.transform.position;
            //Resets Roation back to Zero
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

            objectColiider.enabled = false;
            isReturning = false;
            
            count = 0;

            StopAllCoroutines();

        }


        //lock right hand from moving
        //happens after every charged punch
        if (isLocationLocked == true) 
        {

          
        }
        else 
        {
            objectColiider.enabled = true;
        }

        if (isReturning && count == 0) 
        {
            
            //StopAllCoroutines();
            //StartCoroutine(ReturnToPlayer());
            //count += 1;
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
        Debug.Log("Move STARTED");
        isLocationLocked = false;
        targetPointLocation = TargetPoint.transform.position;
       
        while ((Vector3.Distance(transform.position, targetPointLocation) > reachThreshold))
        {

            transform.position = Vector3.MoveTowards(transform.position, targetPointLocation, playerControls.punchSpeed * Time.deltaTime);
            yield return null;
        }

       
        Debug.Log("Move END");
        count = 0;
        StartCoroutine(ReturnToPlayer());
        isReturning = true;
    }

    public IEnumerator ReturnToPlayer() 
    {

        Debug.Log("RETURN STARTED");

        objectColiider.enabled = false;
        handPointLocation = handPoint.transform.position;

        //TargetPoint.transform.localPosition = handPoint.transform.localPosition;
        while ((Vector3.Distance(transform.position, handPointLocation) > reachThreshold)) 
        {
            //Debug.Log("DISTANCE TO = " + Vector3.Distance(transform.position, handPointLocation));

            transform.position = Vector3.MoveTowards(transform.position, handPointLocation, playerControls.punchSpeed * Time.deltaTime);
            yield return null;


        }

        Debug.Log("RETURN END");
        isLocationLocked = true;
       

    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        //punchOrigin Origin = current position of the fist 
        punchOrigin = GameObject.Find("Player").transform;

        //if left hand has something grabbed then disable that grab
        scriptLH.isGrabbed = false;

        // Handle collision logic here, e.g., damage enemies
        if (!isReturning)
        {
            EnemyKnockBack knockback = collision.GetComponent<EnemyKnockBack>();
            if(knockback) 
            {
                // Calculate direction from PunchOrigin to the object being punched
                Vector2 knockbackDirection = (collision.transform.position - punchOrigin.transform.position).normalized;

                //Apply knockback to calculated direction
                knockback.ApplyKnockBack(knockbackDirection);
                //Debug.LogError("No Enemy detected, Hit " + collision.gameObject.name + "Instead");
            }
           

            // Hit something while moving towards the target, immediately return
            StopAllCoroutines();
            StartCoroutine(ReturnToPlayer());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      /*  //punchOrigin Origin = current position of the fist 
        punchOrigin = transform;

        //if left hand has something grabbed then disable that grab
        scriptLH.isGrabbed = false;

        // Optional: Handle collision logic here, e.g., damage enemies
        if (!isReturning)
        {
            Debug.Log(" KNOCK TRIGGER");
            Enemy01 knockback = collision.gameObject.GetComponent<Enemy01>();
            // Calculate direction from PunchOrigin to the object being punched
            Vector2 knockbackDirection = (collision.transform.position - punchOrigin.transform.position).normalized;

            //Apply knockback to calculated direction
            knockback.ApplyKnockBack(knockbackDirection);

            // Hit something while moving towards the target, immediately return
            StopAllCoroutines();
            StartCoroutine(ReturnToPlayer());
        }*/
    }


    private void OnDrawGizmos()
    {

        if (TargetPoint == null)
        {
            return;

        }

       
        Vector2 origin;
        Vector2 endPoint;
        float TargetX;
        float TargetY;
        if (playerControls.isFacingRight)
        {

            origin = GameObject.Find("Player").transform.localPosition;
            endPoint = origin + new Vector2(TargetPoint.transform.localPosition.x, TargetPoint.transform.localPosition.y) * 5f;
        }
        else
        {
            origin = GameObject.Find("Player").transform.localPosition;

            TargetX = origin.x - TargetPoint.transform.position.x;
            TargetY = origin.y - TargetPoint.transform.position.y;

            endPoint =  origin - new Vector2(TargetX, TargetY) * 5f;

        }

       

        //Changes the gizmos colour on hit
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, endPoint);
    }

}
