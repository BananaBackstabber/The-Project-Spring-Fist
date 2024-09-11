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
    ///
    /// 
    /// 
    /// 
    /// 
    /// 
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
    //


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<EnemyKnockBack>();
        groundLayer = LayerMask.GetMask("Ground");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);

        //Debug.Log("IsKnockBack :" + knockBack.isKnockedBacked);

        if( Temp >= 0.2f) 
         {
             isGrounded = true;
             Debug.Log("ENEMY JUMPED");
             Jump();
             Temp = 0f;

         }




        if (isGrounded && !knockBack.isKnockedBacked) 
        {
            Temp += Time.deltaTime;
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Wall") && !knockBack.isKnockedBacked)
        {
            Flip(); 
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
            isGrounded = false;
        
        }
        /*
         if is grounded is true then
         rb velocity = Jump
         
         */
    
    
    }


    void Flip() 
    {

        isFacingRight = !isFacingRight;
    }
}
