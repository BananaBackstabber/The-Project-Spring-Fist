using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockBack : MonoBehaviour
{
    private float knockbackDistance;
    private float knockbackForce;

    private Rigidbody2D rb;
    public bool isKnockedBacked = false;
    private int damageNumber = 1;
    //private float drag = 1f;



    //Scripts;
    private Player_Control playerControls;
    private Obj_Grab grabbed;
    private Enemy_Health health;

    //Direction Variables
    private Vector2 knockUp;
    private Vector2 direction;
    private Vector2 lastVelocity;

    //knockback Variables
    private float curSpeed;
    public float maxSpeed;
    private int curBounce;
    public int numOfBounces = 2;

    //Knockback Delay
    private int countKnockBack;
    private float curKnockBackDelay;

    private void Awake()
    {
        playerControls = GetComponent<Player_Control>();


        health = GetComponent<Enemy_Health>();
        grabbed = GetComponent<Obj_Grab>();


    }
    // Start is called before the first frame update
    void Start()
    {

        knockUp = new Vector2(0f, 1f);

        rb = GetComponent<Rigidbody2D>();

    }

    public void PlayerHit() 
    {
        playerControls.enabled = false;
        //GetComponent<Collider2D>().enabled = false;


        knockbackForce = Random.Range(1, 12);

        //health.TakeDamage(damageNumber);

        isKnockedBacked = true;

        Vector2 knockbackDirection;
        knockbackDirection = direction + knockUp;
        curBounce = 0;

        if (rb == null || countKnockBack > 0)
        {
            return;
        }

        //Resets velocity
        rb.velocity = Vector2.zero;

        //knockback speed equals punch speed X 1.5f
        knockbackForce = playerControls.punchSpeed * 2f;

        //Apply the knockback to a direction
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        //Clamp the max knockback velocity
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        }
        countKnockBack++;


    }
}
