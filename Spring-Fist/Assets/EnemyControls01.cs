using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControls01 : MonoBehaviour
{
    /// <summary>
    /// Controls enemeies movement 
    /// the goal of this enemy is to jump like a frog and land 
    /// a certain distance away. If the enemy hits a wall then it flips around 
    /// and jumps in the other direction
    /// 
    /// 
    /// </summary>


    /*
     if (isGrounded) 
        {
            Debug.Log("Jumped");
            rb.velocity = new Vector2(rb.velocity.x, 0f);


            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
           
            jumped = new Vector2(rb.velocity.x, 80f);
            isGrounded = false;
            animator.SetTrigger("Jump");

        }
     */
    public float jumpHeight;
    public float jumpDistance;
    private Vector2 Jumped;
    private bool isGrounded;
    private Rigidbody2D rb;
    private float Temp;

    public Transform groundCheck;
    private LayerMask groundLayer;
    private bool isFacingRight;

    private EnemyKnockBack knockBack;
    private PlayerKnockBack playerKnockBack;
    private Obj_Grab grab;

    private Animator animator;
    private SpriteRenderer spriterenderer;
    private float stayTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriterenderer = GetComponent<SpriteRenderer>();
        playerKnockBack = GameObject.Find("Player").GetComponent<PlayerKnockBack>();
        rb = GetComponent<Rigidbody2D>();
        grab = GetComponent<Obj_Grab>();
        knockBack = GetComponent<EnemyKnockBack>();
        groundLayer = LayerMask.GetMask("Ground");  
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Y", rb.velocityY);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);

        //
        //Debug.Log("IsKnockBack :" + knockBack.isKnockedBacked);

        if (!grab.isHeld)
        {
            //Enemey is not grabbed by the player
            knockBack.isKnockedBacked = false;
        }
        else if (grab.isHeld)
        {
            //Enemy is grabbed by the player
            knockBack.isKnockedBacked = true;
        }

        //IF knockback = true then don't do AI movement
        if ( Temp >= 0.2f && !knockBack.isKnockedBacked) 
         {
            //ENEMY JUMPED
             isGrounded = true;
             Jump();
             Temp = 0f;
            
        }

        if (knockBack.isKnockedBacked) 
        {
            animator.SetBool("isStun", true);
        }
        else 
        {
            animator.SetBool("isStun", false);
        }

        if (isGrounded && !knockBack.isKnockedBacked) 
        {
            Temp += Time.deltaTime;
        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        LayerMask targetLayer;
        targetLayer = LayerMask.NameToLayer("Wall");

        //Debug.Log("HIT SOMETHING");
        if(collision.gameObject.layer == targetLayer) 
        {
           // Debug.Log("Flip");
            Flip();

        }

        //Debug.Log(gameObject.name + " Just Hit" + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Wall") && !knockBack.isKnockedBacked)
        {
            
        }

        if (collision.gameObject.CompareTag("Player")
            && !knockBack.isKnockedBacked
            && rb.velocity.y < 0) 
        {
            
            playerKnockBack.PlayerHit();
        
        }
        //Player Knockback

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        //When object stays colling with a wall
        if (collision.gameObject.CompareTag("Wall") && !knockBack.isKnockedBacked)
        {
            //Add Seconds to time variable
            stayTime += Time.deltaTime;
            if (stayTime >= 4)  //  reach 2 seconds before flipping
            {
                Flip();
                stayTime = 0; // Reset temp after flipping
            }
        }



    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            stayTime = 0;
        }

    }


    void Jump() 
    {

        if (isGrounded) 
        {

            //Clears Velocity
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            if (isFacingRight) 
            {
                //Apply Upwards Force
                rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                rb.AddForce(Vector2.right * jumpDistance, ForceMode2D.Impulse);

            }
            else 
            {

                //Apply Upwards Force
                rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                rb.AddForce(Vector2.left * jumpDistance, ForceMode2D.Impulse);


            }


            // Jumped = new Vector2(jumpDistance, jumpHeight);

            // rb.velocity = Jumped;
            //isGrounded = false;
        
        }
        /*
         if is grounded is true then
         rb velocity = Jump
         
         */
    
    
    }


    void Flip() 
    {
        spriterenderer.flipX = !spriterenderer.flipX;
        
        isFacingRight = !isFacingRight;
    }
}
