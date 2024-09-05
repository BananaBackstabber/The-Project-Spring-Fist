using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01 : MonoBehaviour
{
    private float knockbackDistance;
    private float knockbackForce;

    private Rigidbody2D rb;
    private bool isKnockedBacked = false;
    private float drag = 1f;

    private Player_Control playerControls;

    private Vector2 knockUp;

    private Vector2 direction;
    private Vector2 lastVelocity;
    private float curSpeed;

    private int curBounce;
    public int numOfBounces = 2; 
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
        //Debug.Log("Velocity : ]" + rb.velocity);
        //transform.Translate(Vector2.up * Time.deltaTime * knockbackForce);
    }

    private void LateUpdate()
    {

        lastVelocity = rb.velocity;
        
    }
  
    public void ApplyKnockBack(Vector2 direction)
    {
        Vector2 knockbackDirection;
        knockbackDirection = direction + knockUp;

        curBounce = 0;
        

        if(rb == null) 
        {
            return;
        }

        //Resets velocity
        rb.velocity = Vector2.zero;

        //knockback speed equals punch speed X 1.5f
        knockbackForce = playerControls.rightPunchSpeed * 2f;

        //Apply the knockback to a direction
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(curBounce >= numOfBounces) 
        {
            return;
        }

        if (collision.gameObject.CompareTag("Wall")) 
        {
            Debug.Log("Colliding with " + collision.gameObject.name);
            ContactPoint2D point = collision.contacts[0];
            //gets its velocity speed based on the last frame
            curSpeed = lastVelocity.magnitude;
            //sets and reflects the objects direction
            direction = Vector2.Reflect(lastVelocity.normalized, point.normal);
            //Sets thevelocity 
            rb.velocity = direction * Mathf.Max(curSpeed, 0f);

            curBounce++;
        }

        
        
    }


}
