using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockBack : MonoBehaviour
{
    private float knockbackDistance;
    private float knockbackForce;

    private Rigidbody2D rb;
    public bool isKnockedBacked = false;
    private float drag = 1f;

    private Player_Control playerControls;
    private Vector2 knockUp;

    private Vector2 direction;
    private Vector2 lastVelocity;
    private float curSpeed;

    //knockback Variables
    public float maxSpeed;
    private int curBounce;
    public int numOfBounces = 2;

    private int countKnockBack;
    private float curKnockBackDelay;
    private void Awake()
    {
        playerControls = GameObject.Find("Player").GetComponent<Player_Control>();
    }
    // Start is called before the first frame update
    void Start()
    {

        knockUp = new Vector2(0f, 1f);

        rb = GetComponent<Rigidbody2D>();
        rb.drag = drag;
        
    }

    private void Update()
    {

        if(rb.velocity.y == 0f) 
        {
            isKnockedBacked = false;
        }
        //Delays Hit impact so it can't hppen twice in quick succuion
        if(countKnockBack > 0) 
        {

            curKnockBackDelay += 1f * Time.deltaTime;
        }

        if( curKnockBackDelay > 1f) 
        {

            countKnockBack = 0;
            curKnockBackDelay = 0f;
        
        }

        
    }

    private void LateUpdate()
    {

        if (rb.velocity.magnitude > 0f)
        {
            //Debug.Log("Velocity : ]" + rb.velocity.magnitude);
            //transform.Translate(Vector2.up * Time.deltaTime * knockbackForce);

        }


        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        }

        lastVelocity = rb.velocity;
        
    }
  
    public void ApplyKnockBack(Vector2 direction)
    {

        isKnockedBacked = true;

        Vector2 knockbackDirection;
        knockbackDirection = direction + knockUp;
        curBounce = 0;
        

        if(rb == null || countKnockBack > 0) 
        {
            return;
        }

        //Resets velocity
        rb.velocity = Vector2.zero;

        //knockback speed equals punch speed X 1.5f
        knockbackForce = playerControls.rightPunchSpeed * 2f;

        //Apply the knockback to a direction
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        }
        countKnockBack++;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (curBounce >= numOfBounces)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Wall") && isKnockedBacked)
        {
            //Debug.Log("Colliding with " + collision.gameObject.name);
            ContactPoint2D point = collision.contacts[0];
            //gets its velocity speed based on the last frame
            curSpeed = lastVelocity.magnitude;
            //sets and reflects the objects direction
            direction = Vector2.Reflect(lastVelocity.normalized, point.normal);
            //Sets the velocity
            rb.velocity = direction * Mathf.Max(curSpeed, 0f);

            curBounce++;
        }

    }


   
    void ProcessCollision(GameObject collider)
    {

        //collider.GetComponent<Collision2D>().GetContact[0];

        if (curBounce >= numOfBounces)
        {
            return;
        }

        if (collider.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Colliding with " + collider.gameObject.name);
            //ContactPoint2D point = collider.GetComponent<Collider2D>().GetContact(0);
            //gets its velocity speed based on the last frame
            curSpeed = lastVelocity.magnitude;
            //sets and reflects the objects direction
           // direction = Vector2.Reflect(lastVelocity.normalized, point.normal);
            //Sets the velocity
            rb.velocity = direction * Mathf.Max(curSpeed, 0f);

            curBounce++;
        }

    }




}
