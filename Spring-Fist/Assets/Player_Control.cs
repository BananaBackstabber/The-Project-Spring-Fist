using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    //Create a 3 punch combo with 2 buttons  R, L , R
    //A Charge Punch with 3 stages


    //Is facing right
    public float speed;
    private Animator animator;
    private bool IS_FacingRight = true;



    /*
     private float rPunch_count
    private float lPunch_count
     if(Right button is pressed)
    {
        rpunch_count += 1
        animation.trigger(punch_01)
    
    } 
     
     */
    [Header("COMBO VARIABLES")]
    public int rFistCount = 0;
    public int lFistCount = 0;
    public float comboCooldownTime = 2;
    private float comboTimeCount;


    [Header("CHARGE VARIABLES")]
    public float rP_ChargeMin;
    public float rP_Chargemid;
    public float rP_ChargeMax;
    private float rP_ChargeTime;
    private bool isRightCharging = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateMovement();

        UpdateAttack();

        UpdateChargeAttacks();

    }

    void UpdateAttack() 
    {

        //Combo punch 1 script
        if (Input.GetButtonDown("rightPunch") && rFistCount == 0)
        {
            animator.SetTrigger("CP_01");
            rFistCount += 1;
            comboTimeCount = 0;

        }
        else
        {
            animator.ResetTrigger("CP_01");
        }

        //Debug.Log("Right Count = " + rFistCount + ": " + "left count =" + lFistCount);

        //Combo punch 2 
        if (Input.GetButtonDown("leftPunch") && lFistCount == 0)
        {

            animator.SetTrigger("CP_02");
            lFistCount += 1;
            comboTimeCount = 0;
        }
        else
        {
            animator.ResetTrigger("CP_02");
        }


        //Prevent combo spam, gives the combo a cooldown to it
        if(rFistCount > 0 || lFistCount > 0)
        {
            comboTimeCount += 1 * Time.deltaTime;
        
        }


        //If cooldown time is reached reset combo values so
        //player can punch again
        if(comboTimeCount >= comboCooldownTime) 
        {
            rFistCount = 0;
            lFistCount = 0;

            comboTimeCount = 0;
        
        }


    }


    void UpdateChargeAttacks() 
    {

       // When button is down
       if(Input.GetButtonDown("rightPunch")) 
        {
            isRightCharging = true;
            rP_ChargeTime = 0f;
        
        }

        //While Button is held
        if (Input.GetButton("rightPunch")) 
        {

            if (isRightCharging) 
            {
                Debug.Log("Charge time = " + rP_ChargeTime + ": " + "Max Charge = " + rP_ChargeMax);
                rP_ChargeTime += Time.deltaTime;

                if(rP_ChargeTime > rP_ChargeMax)// if charge time is greater then max charge time
                {
                    rP_ChargeTime = rP_ChargeMax; //ChargeTime = max charge time
                
                }
            
            }
        
        }
       //When Button is up
       if (Input.GetButtonUp("rightPunch")) 
        {
            if(rP_ChargeTime >= rP_ChargeMin) 
            {
                Debug.Log("Small Punch GO!!");
                //set trigger animation
            }

            if (rP_ChargeTime >= rP_Chargemid) 
            {
                Debug.Log("Mid Punch A GOO!!!");
                animator.SetTrigger("CR_L2");
            }


            if (rP_ChargeTime == rP_ChargeMax) 
            {
                Debug.Log("BIGGER AND MAX PUNCH A GOGO!!!");
                //set trigger animation
               
            
            }

            isRightCharging = false;
            //animator.ResetTrigger("CR_L2");
        }
    
    
    
    }
    void UpdateMovement() 
    {
        Vector3 movement = Vector3.zero;

        //Move left and right without accelaration and decellration involed in movement
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement = Vector3.left;

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movement = Vector3.right;
        }


        //controls the way the player is facing
        if (movement.x > 0 && !IS_FacingRight)
        {
            Flip();
        }
        else if (movement.x < 0 && IS_FacingRight)
        {
            Flip();
        }

        transform.position += movement * speed * Time.deltaTime;

        //set animator parameter for speed
        animator.SetFloat("Speed", Mathf.Abs(movement.x));


    }

    void Flip()
    {
        //Switch player direction EG. IF player is facing right then player with now face left
        IS_FacingRight = !IS_FacingRight;


        //Mutiple the players scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }
}
