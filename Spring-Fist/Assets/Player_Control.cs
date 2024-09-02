using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Control : MonoBehaviour
{
    //Create a 3 punch combo with 2 buttons  R, L , R
    //A Charge Punch with 3 stages




    private Vector2 moveDirection;
    public Rigidbody2D rb;

    private Vector2 aimDirection;
    private GameObject TargetPoint;
    private RightHand scriptRH;

    //PLAYER VARIABLES

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
    public GameObject rightHand;
    public float rP_ChargeMin;
    public float rP_ChargeMid;
    public float rP_ChargeMax;
    private float rp_ChargeTime;
    private bool isRightCharging = false;
    private Vector2 hitPoint;
    public float punchDistance;
    public float punchSpeed;

    //public InputAction playerControls;
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction Aim;
    private InputAction rPunch;
    private InputAction attackCharge;



    //targetpoint vectors
    private Vector2 right; //right 
    private Vector2 upRight;// up-right
    private Vector2 up; // up
    private Vector2 upLeft;// up-left
    private Vector2 left;// left
    private Vector2 downLeft;//down-left
    private Vector2 down;//down
    private Vector2 downRight;//down-right




    private void Awake()
    {
        //INPUT CONTROLS
        playerControls = new PlayerInputActions();

        //FIND TARGET POINT
        TargetPoint = GameObject.Find("TargetPoint");

        //FIND RHAND SCRIPT
        scriptRH = GameObject.Find("RightHand").GetComponent<RightHand>();

        right = new Vector2(1f, 0f);
        upRight = new Vector2(0.5f, 0.5f);
        up = new Vector2(0f, 1f);
        upLeft = new Vector2(-0.5f, 0.5f);
        left = new Vector2(-1f, 0f);
        downLeft = new Vector2(-0.5f,-0.5f);
        down = new Vector2(0f, -1f);
        downRight = new Vector2(0.5f, -0.5f);

    }
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        Aim = playerControls.Player.Aim;
        Aim.Enable();

        rPunch = playerControls.Player.RightPunch;
        rPunch.Enable();
        rPunch.started += RightPunch;

        attackCharge = playerControls.Player.AttackCharge;
        attackCharge.Enable();
        attackCharge.started += onChargeStart;
        attackCharge.canceled += onChargeCanceled;
    }

    private void OnDisable()
    {
        move.Disable();

        Aim.Disable();

        rPunch.Disable();

        attackCharge.Disable();
        attackCharge.started -= onChargeStart;
        attackCharge.canceled -= onChargeCanceled;
    
    }
    // Update is called once per frame
    void Update()
    {
        //TargetPoint.transform.localPosition = TargetPoint.transform.localPosition * punchDistance;

        // Debug.Log("HitPoint = "+ hitPoint);

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

        if (isRightCharging) 
        {
            if (isRightCharging) //HAPPENS IN ONCHARGE
            {
                Debug.Log("Charge time = " + rp_ChargeTime + ": " + "Max Charge = " + rP_ChargeMax);
                rp_ChargeTime += Time.deltaTime;

                if (rp_ChargeTime <= rP_ChargeMin)
                {

                    punchDistance = 0f;
                    punchSpeed = 3f;

                }

                if (rp_ChargeTime >= rP_ChargeMin)
                {
                    Debug.Log("Small Punch GO!!");
                    punchDistance = 0.5f;
                    punchSpeed = 6f;
                    //set trigger animation
                }

                if (rp_ChargeTime >= rP_ChargeMid)
                {
                    Debug.Log("Mid Punch A GOO!!!");
                    punchDistance = 1f;
                    punchSpeed = 10f;
                    //animator.SetTrigger("CR_L2");
                }


                if (rp_ChargeTime >= rP_ChargeMax)
                {
                    Debug.Log("BIGGER AND MAX PUNCH A GOGO!!!");
                    //set trigger animation
                    punchDistance = 2f;
                    //TargetPoint.transform.localPosition = TargetPoint.transform.localPosition * punchDistance;

                }

                //set position of target point based on charge time
                //TargetPoint.transform.localPosition = TargetPoint.transform.localPosition * punchDistance;


                if (rp_ChargeTime > rP_ChargeMax)// if charge time is greater then max charge time
                {
                    rp_ChargeTime = rP_ChargeMax; //ChargeTime = max charge time

                }

                rp_ChargeTime +=  Time.deltaTime;
        
        }






       // When button is down
       if(Input.GetButtonDown("rightPunch")) 
        {
            isRightCharging = true;//HAPPENS AT PUNCH ACTION
            rp_ChargeTime = 0f;
        
        }

        //While Button is held
        if (Input.GetButton("rightPunch")) 
        {

            
            
            }
        
        }

       if (Input.GetButtonUp("rightPunch")) //HAPPENS IN PERFORM ATTACKS
        {
            if(rp_ChargeTime >= rP_ChargeMin) 
            {
                Debug.Log("Small Punch GO!!");
                //set trigger animation
            }

            if (rp_ChargeTime >= rP_ChargeMid) 
            {
                Debug.Log("Mid Punch A GOO!!!");
                animator.SetTrigger("CR_L2");
            }


            if (rp_ChargeTime == rP_ChargeMax) 
            {
                Debug.Log("BIGGER AND MAX PUNCH A GOGO!!!");
                //set trigger animation
               
            
            }

            isRightCharging = false;
            //animator.ResetTrigger("CR_L2");
        }//////
    
    }
    void UpdateMovement() 
    {

        //This code reads the values from the left and right joystick for movement and aiming
        moveDirection = move.ReadValue<Vector2>();
        aimDirection = Aim.ReadValue<Vector2>();

        //Debug.Log(Aim.ReadValue<Vector2>());

        //controls the way the player is facing
        if (moveDirection.x > 0 && !IS_FacingRight)
        {
            Flip();
        }
        else if (moveDirection.x < 0 && IS_FacingRight)
        {
            Flip();
        }


        //set animator parameter for speed
        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x));


    }


    



    private void onChargeStart(InputAction.CallbackContext context) 
    {
        rp_ChargeTime = 0;
        isRightCharging = true;

    }

    private void onChargeCanceled(InputAction.CallbackContext context) 
    {

        isRightCharging = false;
        performAttack(rp_ChargeTime);


    }

    private void performAttack(float rp_ChargeTime) 
    {

        Debug.Log(rp_ChargeTime);

        
       
        //If the fist is locked then player can't spam charged punches
        //Making so the punch can't change direction mid flight
        if (scriptRH.islocationLock && punchDistance > 0f)
        {
            scriptRH.StopAllCoroutines();
            scriptRH.StartCoroutine(scriptRH.MoveFist(punchDistance));

        }
        

        //scriptRH.islocationLock = false;
        
       
        if(rp_ChargeTime <= 2) 
        {

           
            //Vector3 rHandPositon = rightHand.transform.position;

            //rHandPositon = TargetPoint.transform.position;
        
        }

        rp_ChargeTime = 0f;

        hitPoint = new Vector2(rp_ChargeTime, 0f);

       // rightHand.GetComponent<Rigidbody2D>().velocity = new Vector2(2f,0f);
       
    
    }



    private void RightPunch(InputAction.CallbackContext context) 
    {
       

       // Debug.Log("PUNCHED");
    
    }

    private void FixedUpdate()
    {

        rb.velocity = new Vector2(moveDirection.x * speed, 0f);

        //TargetPoint.transform.localPosition = AimDirection;


        //THE FOLLOWING CODE IS USE TO LINK THE DIRECTION OF 
        // THE RIGHT STICK TO THE TARGET POINT Location
        if (aimDirection.x > 0.25f && aimDirection.y < -0.25f)
        {
            //DIRECTION DOWNRIGHT
            TargetPoint.transform.localPosition = downRight * punchDistance;

        }
        else if (aimDirection.x > -0.25f && aimDirection.y < -0.25f)
        {

            //DIRECTION DOWN    
            TargetPoint.transform.localPosition = down * punchDistance; // * distance 0.5, 1, 2
        }
        else if (aimDirection.x < -0.25f && aimDirection.y < -0.25f)
        {
            //DIRECTION DOWNLEFT 
            TargetPoint.transform.localPosition = downLeft * punchDistance;

        }
        else if (aimDirection.x < -0.25f && aimDirection.y < 0.25f)
        {
            //DIRECTION LEFT 
            TargetPoint.transform.localPosition = left * punchDistance;

        }
        else if (aimDirection.x < -0.25f && aimDirection.y > 0.25f)
        {
            //DIRECTION UPLEFT 
            TargetPoint.transform.localPosition = upLeft * punchDistance;

        }
        else if (aimDirection.x < 0.25f && aimDirection.y > 0.25f)
        {
            //DIRECTION UP
            TargetPoint.transform.localPosition = up * punchDistance;


        }
        else if (aimDirection.x > 0.25f && aimDirection.y > 0.25f)
        {
            //DIRECTION UPRIGHT
            TargetPoint.transform.localPosition = upRight * punchDistance;

        }
        else if (aimDirection.x > 0.25f && aimDirection.y < 0.25f)
        {
            //DIRECTION RIGHT
            TargetPoint.transform.localPosition = right * punchDistance;

        }
        else
        {
            TargetPoint.transform.localPosition = Vector2.zero;
        }
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
