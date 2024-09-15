using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECon_02_Floater : MonoBehaviour
{
    private Transform self;
    private Transform Player;

    private float temp;
    public float speed;
    public float rayDistance;
    private float currentAngle;


    private Vector2 targetposition;
    private Vector2 origin;
    private Vector2 randomDirection;

    private bool isMoving;
    private bool isClockwise;
    




    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {

        temp = 34;
        
    }

    // Update is called once per frame
    void Update()
    {
        temp += Time.deltaTime;

        if(temp > 36f)
        {

            StartCoroutine(CastRandomRay());
            temp = 0f;
        }
        
    }

    IEnumerator CastRandomRay() 
    {

        origin = transform.position; // Origin of the ray/ current position of object
        randomDirection = Random.insideUnitCircle.normalized; // Random direction
        isMoving = true;

        //Reset angle after full rotation 
        if(currentAngle >= 360f) 
        {
            currentAngle = 0f;
            isClockwise = !isClockwise;
            Debug.Log("FUll Rotation complete, Flip direction");
        }

        while(currentAngle <= 360f) 
        {

            //Converts angle to direction vector
            Vector2 direction = AngleToVector2(currentAngle);


            //Cast ray in current direction
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance);

            //draw ray
            Debug.DrawRay(origin, direction * rayDistance, Color.red, 1f);


            // Check if the ray hit something
            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name + " at angle: " + currentAngle + " degrees.");
               // rayHit = true; // hit something, so we will try the next angle
            }
            else
            {
                // If no object is hit, move the object towards the end of the ray
                Vector2 targetPosition = origin + direction * rayDistance;
                Debug.Log("No hit, moving towards position: " + targetPosition);
                //currentAngle = 30f; 
                //isClockwise = !isClockwise;
                yield return StartCoroutine(FloaterMove(transform, targetPosition)); // Move towards the empty direction


                // After moving, check if there's an obstacle between the moved object and the player
                if (isPathToPlayerClear(this.transform.position))
                {
                    Debug.Log("PATH TO PLAYER IS CLEAR");
                    break; //Break the loop if path is found

                }
                else
                {
                    Debug.Log("SOMETHING IS IN THE WAY MY FRIEND");
                    //yield return null;
                }
            }


            currentAngle += isClockwise ? 30f : -30f;

            // Reset angle if it goes below 0 degrees (for counter-clockwise rotation)
            if (currentAngle < 0f)
            {
                Debug.Log("All direction hit something or path is block, reset angle");
                currentAngle = 360f;

            }
            yield return null; //Yield control back to unity;

        }

       

        isMoving = false;//Things can move again
        


       




    }



    Vector2 AngleToVector2(float angle) 
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }


    bool isPathToPlayerClear(Vector2 curPos) 
    {
        self = transform;
        Player = GameObject.Find("Player").transform;

        Vector2 directionToPlayer = (Player.position - self.position).normalized;
        float distanceToPlayer = Vector2.Distance(self.position, Player.position);
        Debug.DrawRay(curPos, directionToPlayer * distanceToPlayer, Color.green, 1f); // Visualize the ray

        RaycastHit2D hit = Physics2D.Raycast(curPos, directionToPlayer, distanceToPlayer);


        if(hit.collider != null) 
        {

            if(hit.collider.gameObject == Player.gameObject)
            {

                return true; //path is clear

            }
            else 
            {
                // The ray hit something other than the player, obstacle detected
                Debug.Log("Obstacle detected: " + hit.collider.gameObject.name);
                return false; // Obstacle in the path
            }
    
        }
        return false; //Something is blocking the way
    
    }
    IEnumerator FloaterMove(Transform self, Vector2 targetPosition) 
    {
        while(Vector2.Distance(self.position, targetPosition) > 0.1f) 
        {

            self.position = Vector2.MoveTowards(self.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        
        }

        //StopAllCoroutines();
    
    }
}
