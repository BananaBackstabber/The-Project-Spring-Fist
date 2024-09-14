using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockBack : MonoBehaviour
{
    private float knockbackDistance;
    private float knockbackForce;

    private Rigidbody2D rb;
    private Animator animator;
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
    private LayerMask hitLayer;

    //Knockback Delay
    private int countKnockBack;
    private float curKnockBackDelay;
    private int newLayer;
    private int curLayer;

    private void Awake()
    {

        playerControls = GetComponent<Player_Control>();
        health = GetComponent<Enemy_Health>();
        grabbed = GetComponent<Obj_Grab>();
        animator = GetComponent<Animator>();
        //hitLayer = LayerMask.GetMask("Player");
    }


    // Start is called before the first frame update
    void Start()
    {
        knockUp = new Vector2(0f, 1f);
        rb = GetComponent<Rigidbody2D>();
    }

    public void PlayerHit()
    {

        
        //GetComponent<Collider2D>().enabled = false;

        //this.gameObject.layer = hitLayer;
        knockbackForce = Random.Range(1, 12);

        //health.TakeDamage(damageNumber);

        isKnockedBacked = true;
        StartCoroutine(PlayerStun());

        direction = new Vector2(-4f, 0f);
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
        knockbackForce = 3f;

        //Debug.Log("HIT BACK FORCE" + knockbackDirection * knockbackForce);


        //Apply the knockback to a direction NOT WORKING
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

      
        //Clamp the max knockback velocity
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

        countKnockBack++;
    }

    private void Update()
    {

        Vector2 knockbackDirection;
        knockbackDirection = direction + knockUp;

    }


    public IEnumerator PlayerStun() 
    {
        SpriteRenderer spriteRenderer;
        spriteRenderer = GetComponent<SpriteRenderer>();

        
        //Triggers animation
        animator.SetTrigger("Stun");
        //Save current layer player is at
        curLayer = this.gameObject.layer;
        //Gets new for player to be set at
        newLayer = LayerMask.NameToLayer("BackGround");
        this.gameObject.layer = newLayer;

        playerControls.enabled = false;

        for (int i = 0; i < 3; i++) //flash 3 times
        {

            //Change To flash colour
            spriteRenderer.enabled = false;

            yield return new WaitForSeconds(0.4f / 2);
            //Revert to the original colour 
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.4f / 2);


        }
  
        animator.SetTrigger("StunNull");
        //Returns these values after stun has finished
        this.gameObject.layer = curLayer;
        isKnockedBacked = false;
        playerControls.enabled = true;
     
        StopAllCoroutines();

        yield return null;
    }
}
