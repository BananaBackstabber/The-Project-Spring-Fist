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
    private GameObject targetPoint01;
    private GameObject targetPoint02;
    private RightHand scriptRH;
    private LeftHand scriptLH;

    //PLAYER VARIABLES
    public float speed;
    private Animator animator;
    [HideInInspector] public bool isFacingRight = true;
    public bool isMoving = true;
    public float nGravity;


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
    public int fistComboCount = 0;
    public float comboCooldownTime = 2;
    private float comboTimeCount;

    [Header("CHARGE TIME")]
    public GameObject rightHand;
    public float ChargeMin;
    public float ChargeMid;
    public float ChargeMax;

    [Header("CHARGE DISTANCE")]
    public float chargeDistanceMin;
    public float chargeDistanceMid;
    public float chargeDistanceMax;

    [Header("CHARGE SPEED")]
    public float punchSpeedMin;
    public float punchSpeedMid;
    public float punchSpeedMax;

    //RIGHT CHARGE
    private float rp_ChargeTime;
    private bool isRightCharging = false;
    private Vector2 hitPoint;
    private float rp_PunchDistance;
    [HideInInspector] public float punchSpeed;
    private int returnCount;
    //LEFT CHARGE
    private float lp_ChargeTime;
    private bool isLeftCharging = false;
    private float lp_PunchDistance;
    [HideInInspector] public float leftPunchSpeed;

    //public InputAction playerControls;
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction Aim;
    private InputAction rPunch;
    private InputAction lPunch;
    private InputAction rightCharge;
    private InputAction leftCharge;
    private InputAction jump;


    //Jumping 
    private Vector2 jumped;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Transform groundcheck;
    public float checkRadius;
    private bool isGrounded;
    private bool isWallJump = false;
    private Vector2 animDirection;

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


        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Wall");

        //INPUT CONTROLS
        playerControls = new PlayerInputActions();

        //FIND TARGET POINT for right punch
        targetPoint01 = GameObject.Find("TargetPoint01");

        //FIND THE TARGET POINT FOR LEFT GRAB
        targetPoint02 = GameObject.Find("TargetPoint02");

        //FIND RIGHT HAND SCRIPT
        scriptRH = GameObject.Find("RightHand").GetComponent<RightHand>();

        //FIND LEFT HAND SCRIPT
        scriptLH = GameObject.Find("LeftHand").GetComponent<LeftHand>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        right = new Vector2(1f, 0f);
        upRight = new Vector2(0.75f, 0.75f);
        up = new Vector2(0f, 1f);
        upLeft = new Vector2(-0.75f, 0.75f);
        left = new Vector2(-1f, 0f);
        downLeft = new Vector2(-0.75f, -0.75f);
        down = new Vector2(0f, -1f);
        downRight = new Vector2(0.75f, -0.75f);

        

    }
    void Start()
    {
        
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        Aim = playerControls.Player.Aim;
        Aim.Enable();

        lPunch = playerControls.Player.LeftGrab;
        lPunch.Enable();
        lPunch.started += LeftPunch; 

        rightCharge = playerControls.Player.RightCharge;
        rightCharge.Enable();
        rightCharge.started += onRightChargeStart;
        rightCharge.canceled += onRightChargeCanceled;

        leftCharge = playerControls.Player.LeftCharge;
        leftCharge.Enable();
        leftCharge.started += onLeftChargeStart;
        leftCharge.canceled += onLeftChargeCanceled;

        rPunch = playerControls.Player.RightPunch;
        rPunch.Enable();
        rPunch.started += RightPunch;

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.started += Jump;
    }

    private void OnDisable()
    {
        move.Disable();
        Aim.Disable();
        rPunch.Disable();
        lPunch.Disable();
        jump.Disable();

        rightCharge.Disable();
        rightCharge.started -= onRightChargeStart;
        rightCharge.canceled -= onRightChargeCanceled;

        leftCharge.Disable();
        leftCharge.started -= onLeftChargeStart;
        leftCharge.canceled -= onLeftChargeCanceled;
    
    }
    // Update is called once per frame
    void Update()
    {

        animator.SetFloat("v.y", rb.velocity.y);
        //TargetPoint.transform.localPosition = TargetPoint.transform.localPosition * punchDistance;

        // Debug.Log("HitPoint = "+ hitPoint);

        UpdateMovement();

        UpdateAttack();
        
        UpdateChargeAttacks();

    }
    private void FixedUpdate()
    {

        //Translate movement of the player
        if (isMoving) 
        {
            rb.gravityScale = nGravity;
            rb.velocity = new Vector2(moveDirection.x * speed, jumped.y);

        }
        

        //Resets jumped
        jumped.y = 0f;


        //TargetPoint.transform.localPosition = AimDirection;
    }
    private void LateUpdate()
    {
       

        if (isFacingRight)
        {
            //THE FOLLOWING CODE IS USE TO LINK THE DIRECTION OF 
            // THE RIGHT STICK TO THE TARGET POINT Location

            if (aimDirection.x == 0f && aimDirection.y == 0f)
            {
                //DIRECTION RIGHT
                targetPoint01.transform.localPosition = right * rp_PunchDistance;
                animDirection = right;


            }
            else if (aimDirection.x > 0.25f && aimDirection.y < -0.25f)
            {
                //DIRECTION DOWNRIGHT
                targetPoint01.transform.localPosition = downRight * rp_PunchDistance;
                animDirection = downRight;
            }
            else if (aimDirection.x > -0.25f && aimDirection.y < -0.25f)
            {
                //DIRECTION DOWN    
                targetPoint01.transform.localPosition = down * rp_PunchDistance; // * distance 0.5, 1, 2
                animDirection = down;
            }
            else if (aimDirection.x < -0.25f && aimDirection.y < -0.25f)
            {
                //DIRECTION DOWNLEFT 
                targetPoint01.transform.localPosition = downLeft * rp_PunchDistance;
                animDirection = downLeft;

            }
            else if (aimDirection.x < -0.25f && aimDirection.y < 0.25f)
            {
                //DIRECTION LEFT 
                targetPoint01.transform.localPosition = left * rp_PunchDistance;
                animDirection = left;

            }
            else if (aimDirection.x < -0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UPLEFT 
                targetPoint01.transform.localPosition = upLeft * rp_PunchDistance;
                animDirection = upLeft;

            }
            else if (aimDirection.x < 0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UP
                targetPoint01.transform.localPosition = up * rp_PunchDistance;
                animDirection = up;
            }
            else if (aimDirection.x > 0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UPRIGHT
                targetPoint01.transform.localPosition = upRight * rp_PunchDistance;
                animDirection = upRight;
            }
            else if (aimDirection.x > 0.25f && aimDirection.y < 0.25f)
            {
                //DIRECTION RIGHT
                targetPoint01.transform.localPosition = right * rp_PunchDistance;
                animDirection = right;
            }
            else
            {
                targetPoint01.transform.localPosition = Vector2.zero;
            }



            //LEFT STICK TARGET POINT || TARGET POINT 2 ROTATION

            if(aimDirection.x == 0f && aimDirection.y == 0f) 
            {
                targetPoint02.transform.localPosition = right * lp_PunchDistance;
            
            }
            else if (aimDirection.x > 0.25f && aimDirection.y < -0.25f)
            {
                //DIRECTION DOWNRIGHT
                targetPoint02.transform.localPosition = downRight * lp_PunchDistance;

            }
            else if (aimDirection.x > -0.25f && aimDirection.y < -0.25f)
            {

                //DIRECTION DOWN    
                targetPoint02.transform.localPosition = down * lp_PunchDistance; // * distance 0.5, 1, 2
            }
            else if (aimDirection.x < -0.25f && aimDirection.y < -0.25f)
            {
                //DIRECTION DOWNLEFT 
                targetPoint02.transform.localPosition = downLeft * lp_PunchDistance;

            }
            else if (aimDirection.x < -0.25f && aimDirection.y < 0.25f)
            {
                //DIRECTION LEFT 
                targetPoint02.transform.localPosition = left * lp_PunchDistance;

            }
            else if (aimDirection.x < -0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UPLEFT 
                targetPoint02.transform.localPosition = upLeft * lp_PunchDistance;

            }
            else if (aimDirection.x < 0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UP
                targetPoint02.transform.localPosition = up * lp_PunchDistance;

            }
            else if (aimDirection.x > 0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UPRIGHT
                targetPoint02.transform.localPosition = upRight * lp_PunchDistance;

            }
            else if (aimDirection.x > 0.25f && aimDirection.y < 0.25f)
            {
                //DIRECTION RIGHT
                targetPoint02.transform.localPosition = right * lp_PunchDistance;

            }
            else
            {
                targetPoint02.transform.localPosition = Vector2.zero;
            }

        }
        else
        {


            //THE FOLLOWING CODE IS USE TO LINK THE DIRECTION OF 
            // THE RIGHT STICK TO THE TARGET POINT Location
            if (aimDirection.x == 0f && aimDirection.y == 0f)
            {
                //DIRECTION Foward
                targetPoint02.transform.localPosition = right *  rp_PunchDistance;
                animDirection = right;
            }
            else if (aimDirection.x > 0.25f && aimDirection.y < -0.25f)
            {
                //DIRECTION DOWNRIGHT
                targetPoint01.transform.localPosition = downLeft * rp_PunchDistance;
                animDirection = downLeft;

            }
            else if (aimDirection.x > -0.25f && aimDirection.y < -0.25f)
            {

                //DIRECTION DOWN    
                targetPoint01.transform.localPosition = down * rp_PunchDistance; // * distance 0.5, 1, 2
                animDirection = down;
            }
            else if (aimDirection.x < -0.25f && aimDirection.y < -0.25f)
            {
                //DIRECTION DOWNLEFT
                targetPoint01.transform.localPosition = downRight * rp_PunchDistance;
                animDirection = right;

            }
            else if (aimDirection.x < -0.25f && aimDirection.y < 0.25f)
            {
                //DIRECTION Left
                targetPoint01.transform.localPosition = right * rp_PunchDistance;
                animDirection = right;
               

            }
            else if (aimDirection.x < -0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UPLEFT
                targetPoint01.transform.localPosition = upRight * rp_PunchDistance;
                animDirection = upRight;

            }
            else if (aimDirection.x < 0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UP
                targetPoint01.transform.localPosition = up * rp_PunchDistance;
                animDirection = up;
            }
            else if (aimDirection.x > 0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UPRIGHT
                targetPoint01.transform.localPosition = upLeft * rp_PunchDistance;
                animDirection = left;
            }
            else if (aimDirection.x > 0.25f && aimDirection.y < 0.25f)
            {
                //DIRECTION RIGHT
                targetPoint01.transform.localPosition = left * rp_PunchDistance;
                animDirection = left;
            }
            else
            {
                targetPoint01.transform.localPosition = Vector2.zero;
            }



            //LEFT STICK TARGET POINT || TARGET POINT 2 ROTATION
            if (aimDirection.x == 0f && aimDirection.y == 0f)
            {
                //DIRECTION Foward If no input from player then shoot in the foward facing direction 
                targetPoint02.transform.localPosition = right * lp_PunchDistance;

            }
            else if (aimDirection.x > 0.25f && aimDirection.y < -0.25f)
            {
                //DIRECTION DOWNleft
                targetPoint02.transform.localPosition = downLeft * lp_PunchDistance;

            }
            else if (aimDirection.x > -0.25f && aimDirection.y < -0.25f)
            {

                //DIRECTION DOWN    
                targetPoint02.transform.localPosition = down * lp_PunchDistance; // * distance 0.5, 1, 2
            }
            else if (aimDirection.x < -0.25f && aimDirection.y < -0.25f)
            {
                //DIRECTION DOWNRIGHT
                targetPoint02.transform.localPosition = downRight * lp_PunchDistance;

            }
            else if (aimDirection.x < -0.25f && aimDirection.y < 0.25f)
            {
                //DIRECTION RIGHT
                targetPoint02.transform.localPosition = right * lp_PunchDistance;

            }
            else if (aimDirection.x < -0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UPRIGHT
                targetPoint02.transform.localPosition = upRight * lp_PunchDistance;

            }
            else if (aimDirection.x < 0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UP
                targetPoint02.transform.localPosition = up * lp_PunchDistance;

            }
            else if (aimDirection.x > 0.25f && aimDirection.y > 0.25f)
            {
                //DIRECTION UPLEFT
                targetPoint02.transform.localPosition = upLeft * lp_PunchDistance;

            }
            else if (aimDirection.x > 0.25f && aimDirection.y < 0.25f)
            {
                //DIRECTION LEFT
                targetPoint02.transform.localPosition = left * lp_PunchDistance;

            }
            else
            {
                targetPoint02.transform.localPosition = Vector2.zero;
            }

            
        }
    }
    void UpdateAttack() 
    {

        //Prevent combo spam, gives the combo a cooldown to it
        if(fistComboCount > 0)
        {
            comboTimeCount += 1 * Time.deltaTime;
        
        }


        //If cooldown time is reached reset combo values so
        //player can punch again
        if(comboTimeCount >= comboCooldownTime) 
        {
            fistComboCount = 0;
            comboTimeCount = 0;
        }

    }


    void UpdateChargeAttacks() 
    {
        //Debug.Log(rp_ChargeTime);
        OnRightCharge();
        onLeftCharge();

    }
    void UpdateMovement() 
    {
        //is grounded Check
        isGrounded = Physics2D.OverlapCircle(groundcheck.position, checkRadius, groundLayer);
       // Debug.Log("Ground = " + isGrounded);

        isWallJump = Physics2D.OverlapCircle(groundcheck.position, checkRadius, wallLayer);
        //Debug.Log("WALL JUMP = " + isWallJump);


        if (isMoving)
        {
            //This code reads the values from the left and right joystick for movement and aiming
            moveDirection = move.ReadValue<Vector2>();
        }
        else
        { 
           moveDirection = Vector2.zero;
        }
        aimDirection = Aim.ReadValue<Vector2>();

        //Debug.Log(Aim.ReadValue<Vector2>());

        //controls the way the player is facing
        if (moveDirection.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveDirection.x < 0 && isFacingRight)
        {
            Flip();
        }
        //set animator parameter for speed
        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x));
    }


    /// <summary>
    /// Right CHARGE 
    /// All need to for the right trigger
    /// and holding it down to charge a punch 
    /// </summary>
    void OnRightCharge() 
    {
        if (isRightCharging) //HAPPENS IN ONCHARGE
        {
            
            rp_ChargeTime += Time.deltaTime;//Increases the charge time when right trigger is held

            //set position of target point based on charge time
            if (rp_ChargeTime <= ChargeMin)
            {
                //scriptRH.isLocationLocked = true;

            }

            if (rp_ChargeTime >= ChargeMin)
            {
                //Debug.Log("Small Punch GO!!");
                rp_PunchDistance = chargeDistanceMin;
                //set trigger animation
            }

            if (rp_ChargeTime >= ChargeMid)
            {
                //Debug.Log("Mid Punch A GOO!!!");
                rp_PunchDistance = chargeDistanceMid;
                //rightPunchSpeed = 10f;
                //animator.SetTrigger("CR_L2");
            }


            if (rp_ChargeTime >= ChargeMax)
            {
                //Debug.Log("BIGGER AND MAX PUNCH A GOGO!!!");
                rp_PunchDistance = chargeDistanceMax;
            }

            if (rp_ChargeTime > ChargeMax)// if charge time is greater then max charge time
            {
                rp_ChargeTime = ChargeMax; //ChargeTime = max charge time

            }
        }


    }

   


    private void onRightChargeStart(InputAction.CallbackContext context) 
    {
        
        rp_ChargeTime = 0;
        isRightCharging = true;

    }

    private void onRightChargeCanceled(InputAction.CallbackContext context) 
    {

        isRightCharging = false;
        if(scriptRH.isLocationLocked && rp_PunchDistance > 0f) 
        {
            Debug.Log("RIGHT PUNCH A GO");
            RightPerformAttack(rp_ChargeTime);
        }

        //rp_ChargeTime = 0f;
        //Debug.Log(rp_ChargeTime);

    }
    private void RightPerformAttack(float rp_ChargeTime) 
    {

        //Debug.Log(rp_ChargeTime);

        //IF player is stunned the stop and reset charge punch
        if (GetComponent<PlayerKnockBack>().isKnockedBacked) 
        {
            punchSpeed = 0f;
            return;
        
        }


        /*if(rp_ChargeTime > ChargeMin && scriptRH.is) 
        {
            scriptLH.isLocationLocked = true;
        }*/
        //Checks charge variables once
        if (rp_ChargeTime <= ChargeMin)
        {
            //Debug.Log("Small Punch GO!!");
            punchSpeed = punchSpeedMin;
            //set trigger animation
        }

        if (rp_ChargeTime >= ChargeMid)
        {

            //Debug.Log("Mid Punch A GOO!!!");
            punchSpeed = punchSpeedMid;
            //set trigger animation
        }

        if (rp_ChargeTime >= ChargeMax)
        {
            //Debug.Log("BIGGER AND MAX PUNCH A GOGO!!!");
            punchSpeed = punchSpeedMax;
        }

        scriptRH.StopAllCoroutines();
        scriptRH.StartCoroutine(scriptRH.MoveFist(rp_PunchDistance));

        //If the fist is locked then player can't spam charged punches
        //Making so the punch can't change direction mid flight
        /*if (scriptRH.isLocationLocked && rp_PunchDistance > 0f)
        {
            scriptRH.StopAllCoroutines();
            scriptRH.StartCoroutine(scriptRH.MoveFist(rp_PunchDistance));

        }*/


        //Sets animation Direction
        if(animDirection == right 
         || animDirection == downRight 
         || animDirection == upRight) 
        {
            //RIGHT
            animator.SetTrigger("R2_CG");
        }
        else if(animDirection == left 
              || animDirection == downLeft
              || animDirection == upLeft) 
        {
            //LEFT
            animator.SetTrigger("R2_CG");
            Flip();
        }
        else if(animDirection == down) 
        {
            //DOWN
            animator.SetTrigger("R2_CD");
        }
        else if(animDirection == up) 
        {
            //UP
            animator.SetTrigger("R2_CU");
        }
        
        //Resets charge time to 0
        this.gameObject.GetComponent<Player_Control>().rp_ChargeTime = 0f;

        //Resets the punch distance and targetpoint location
        rp_PunchDistance = 0f;
        //hitPoint = new Vector2(rp_ChargeTime, 0f);
    }

    private void RightPunch(InputAction.CallbackContext context)
    {  
        if (!scriptRH.isLocationLocked)
        {
            //How many times has the right trigger been pressed
            //while location lock is not true for right hand
            returnCount += 1;
            if (returnCount >= 3) 
            {
                Debug.Log("Return EARLY: " + returnCount);
                //Resets return input count
                returnCount = 0;
                //Stops the fist from moving and then starts its return
                scriptRH.StopAllCoroutines();
                scriptRH.StartCoroutine(scriptRH.ReturnToPlayer());

            }
            
            
        }
        else{ returnCount = 0; }

    }

    ///<summary>
    /// Holding combo script
        /*if (scriptRH.isLocationLocked) 
        {

            //Combo punch 2 
            if (fistComboCount == 1)
            {

                animator.SetTrigger("CP_02");
                fistComboCount += 1;
                comboTimeCount = 0;
            }
            else
            {
                animator.ResetTrigger("CP_02");
            }

            //Combo punch 1 script
            if (fistComboCount == 0)
            {
                animator.SetTrigger("CP_01");
                fistComboCount += 1;
                comboTimeCount = 0;

            }
            else
            {
                animator.ResetTrigger("CP_01");
            }

        }*/
    /// <summary>
 
    /// LEFT CHARGE 
    /// All need to for the left trigger and the charging grab mechanic
    /// </summary>
    void onLeftCharge()
    {

        if (isLeftCharging) //HAPPENS IN ONCHARGE
        {

            lp_ChargeTime += Time.deltaTime;//Increases the charge time when right trigger is held

            //set position of target point based on charge time
            if (lp_ChargeTime <= ChargeMin)
            {
                //

            }
            if (lp_ChargeTime >= ChargeMin)
            {
                
                lp_PunchDistance = chargeDistanceMin;
                //set trigger animation
            }
            if (lp_ChargeTime >= ChargeMid)
            {
               
                lp_PunchDistance = chargeDistanceMid;
                //animator.SetTrigger("CR_L2");
            }
            if (lp_ChargeTime >= ChargeMax)
            {
                
                lp_PunchDistance = chargeDistanceMax;
            }
            if (lp_ChargeTime > ChargeMax)// if charge time is greater then max charge time
            {
                lp_ChargeTime = ChargeMax; //ChargeTime = max charge time

            }

        }



    }
    private void onLeftChargeStart(InputAction.CallbackContext context)
    {
        lp_ChargeTime = 0;
        isLeftCharging = true;
    }

    private void onLeftChargeCanceled(InputAction.CallbackContext context)
    {
        isLeftCharging = false;
        if (scriptLH.isLocationLocked) 
        {

            LeftPerformAttack(lp_ChargeTime);
        }
        
    }
    private void LeftPerformAttack(float lp_ChargeTime) 
    {

        //IF player is stunned the stop and reset charge punch
        if (GetComponent<PlayerKnockBack>().isKnockedBacked)
        {
            punchSpeed = 0f;
            return;

        }

        

        if (lp_ChargeTime >= ChargeMin) 
        {
            //scriptLH.isLocationLocked = true;
            //leftPunchSpeed = 0f;

        }
        //Checks charge variables once
        if (lp_ChargeTime <= ChargeMin )
        {

            leftPunchSpeed = punchSpeedMin;
            //set trigger animation
        }

        if (lp_ChargeTime >= ChargeMid)
        {

            leftPunchSpeed = punchSpeedMid;
            //set trigger animation
        }

        if (lp_ChargeTime >= ChargeMax)
        {

            leftPunchSpeed = punchSpeedMax;
            
        }

        //Sets animation Direction
        if (animDirection == right
         || animDirection == downRight
         || animDirection == upRight)
        {
           // Debug.Log("Right");
            animator.SetTrigger("L2_CG");
        }
        else if (animDirection == left
              || animDirection == downLeft
              || animDirection == upLeft)
        {
           // Debug.Log("Left");
            animator.SetTrigger("L2_CG");
            Flip();
        }
        else if (animDirection == down)
        {
            //Debug.Log("Down");
            animator.SetTrigger("L2_CD");
        }
        else if (animDirection == up)
        {
           // Debug.Log("Up");
            animator.SetTrigger("L2_CU");
        }
        //If the fist is locked then player can't spam charged punches
        //Making so the punch can't change direction mid flight
        if (scriptLH.isLocationLocked && lp_PunchDistance > 0f)
        {
             scriptLH.StopAllCoroutines();
             scriptLH.StartCoroutine(scriptLH.MoveFist(rp_PunchDistance));

        }

        //Resets the punch distance and target points location
        lp_PunchDistance = 0f;
        lp_ChargeTime = 0f;

    }

   
    
    private void LeftPunch(InputAction.CallbackContext context) 
    {
        //If left trigger is tapped after left fist has been launch
        // then return the left fist to the player
        if(!scriptLH.isLocationLocked && !scriptLH.isReturning) 
        {
            scriptLH.StopAllCoroutines();
            scriptLH.StartCoroutine(scriptLH.ReturnToPlayer());
        }
        //Debug.Log("GRAB");

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("You just hit" + collision.gameObject.name);
        if (collision.gameObject.layer == 7) 
        {
            isGrounded = true;
        }
        else 
        {
            isGrounded = false;
        }


        if (collision.gameObject.layer ==  11) 
        {

            isWallJump = true;
        
        }
    }
    private void Jump(InputAction.CallbackContext context) 
    {
    
        if (isGrounded) 
        {
            Debug.Log("Jumped");
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
           
            jumped = new Vector2(rb.velocity.x, 80f);
            isGrounded = false;
            animator.SetTrigger("Jump");

        }
        if (isWallJump && !scriptLH.isReturning) 
        {
            Debug.Log("WALL JUMP");
            scriptLH.isReturning = true;
            Vector2 direction = this.transform.position - scriptLH.gameObject.transform.position;
            Debug.Log(direction);
            rb.velocity = new Vector2(direction.x * 50f, 0f);

            rb.AddForce(Vector2.up * 8f, ForceMode2D.Impulse);
            //isWallJump = false;
        }
    }
 
    void Flip()
    {
        //Switch player direction EG. IF player is facing right then player with now face left
        isFacingRight = !isFacingRight;


        //Mutiple the players scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }
}
