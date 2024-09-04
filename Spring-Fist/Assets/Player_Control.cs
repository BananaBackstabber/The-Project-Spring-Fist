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
    private bool isFacingRight = true;



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
    public float ChargeMin;
    public float ChargeMid;
    public float ChargeMax;

    //RIGHT CHARGE
    private float rp_ChargeTime;
    private bool isRightCharging = false;
    private Vector2 hitPoint;
    private float rp_PunchDistance;
    public float rightPunchSpeed;

    //LEFT CHARGE
    private float lp_ChargeTime;
    private bool isLeftCharging = false;
    private float lp_PunchDistance;
    public float leftPunchSpeed;

    //public InputAction playerControls;
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction Aim;
    private InputAction rPunch;
    private InputAction lPunch;
    private InputAction rightCharge;
    private InputAction leftCharge;


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

        //FIND TARGET POINT for right punch
        targetPoint01 = GameObject.Find("TargetPoint01");

        //FIND THE TARGET POINT FOR LEFT GRAB
        targetPoint02 = GameObject.Find("TargetPoint02");

        //FIND RIGHT HAND SCRIPT
        scriptRH = GameObject.Find("RightHand").GetComponent<RightHand>();

        //FIND LEFT HAND SCRIPT
        scriptLH = GameObject.Find("LeftHand").GetComponent<LeftHand>();

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
    }

    private void OnDisable()
    {
        move.Disable();
        Aim.Disable();
        rPunch.Disable();
        lPunch.Disable();

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
        OnRightCharge();
        onLeftCharge();

    }
    void UpdateMovement() 
    {

        //This code reads the values from the left and right joystick for movement and aiming
        moveDirection = move.ReadValue<Vector2>();
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
                rp_PunchDistance = 0f;

            }

            if (rp_ChargeTime >= ChargeMin)
            {
                //Debug.Log("Small Punch GO!!");
                rp_PunchDistance = 0.5f;
                //set trigger animation
            }

            if (rp_ChargeTime >= ChargeMid)
            {
                //Debug.Log("Mid Punch A GOO!!!");
                rp_PunchDistance = 1f;
                rightPunchSpeed = 10f;
                //animator.SetTrigger("CR_L2");
            }


            if (rp_ChargeTime >= ChargeMax)
            {
                //Debug.Log("BIGGER AND MAX PUNCH A GOGO!!!");
                rp_PunchDistance = 2f;
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
            RightPerformAttack(rp_ChargeTime);

        }

        rp_ChargeTime = 0f;

    }
    private void RightPerformAttack(float rp_ChargeTime) 
    {

        Debug.Log(rp_ChargeTime);

        //Checks charge variables once
        if (rp_ChargeTime <= ChargeMin)
        {
            Debug.Log("Small Punch GO!!");
            rightPunchSpeed = 3f;
            //set trigger animation
        }

        if (rp_ChargeTime >= ChargeMid)
        {

            Debug.Log("Mid Punch A GOO!!!");
            rightPunchSpeed = 6f;
            //set trigger animation
        }

        if (rp_ChargeTime >= ChargeMax)
        {
            Debug.Log("BIGGER AND MAX PUNCH A GOGO!!!");
            rightPunchSpeed = 10f;
            animator.SetTrigger("CR_L2");
        }

        scriptRH.StopAllCoroutines();
        scriptRH.StartCoroutine(scriptRH.MoveFist(rp_PunchDistance));

        //If the fist is locked then player can't spam charged punches
        //Making so the punch can't change direction mid flight
        if (scriptRH.isLocationLocked && rp_PunchDistance > 0f)
        {
            scriptRH.StopAllCoroutines();
            scriptRH.StartCoroutine(scriptRH.MoveFist(rp_PunchDistance));

        }
       
        

        //hitPoint = new Vector2(rp_ChargeTime, 0f);

   
       
    
    }

    private void RightPunch(InputAction.CallbackContext context)
    {


        // Debug.Log("PUNCHED");

    }



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
                lp_PunchDistance = 0f;

            }
            if (lp_ChargeTime >= ChargeMin)
            {
                
                lp_PunchDistance = 0.5f;
                //set trigger animation
            }
            if (lp_ChargeTime >= ChargeMid)
            {
               
                lp_PunchDistance = 1f;
                rightPunchSpeed = 10f;
                //animator.SetTrigger("CR_L2");
            }
            if (lp_ChargeTime >= ChargeMax)
            {
                
                lp_PunchDistance = 2f;
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
        LeftPerformAttack(lp_ChargeTime);
    }
    private void LeftPerformAttack(float lp_ChargeTime) 
    {

        if(lp_ChargeTime >= ChargeMin) 
        {
            leftPunchSpeed = 0f;
        
        }
        //Checks charge variables once
        if (lp_ChargeTime <= ChargeMin)
        {

            leftPunchSpeed = 3f;
            //set trigger animation
        }

        if (lp_ChargeTime >= ChargeMid)
        {

            leftPunchSpeed = 6f;
            //set trigger animation
        }

        if (lp_ChargeTime >= ChargeMax)
        {

            leftPunchSpeed = 10f;
            animator.SetTrigger("CR_L2");
        }

        //If the fist is locked then player can't spam charged punches
        //Making so the punch can't change direction mid flight
        if (scriptLH.isLocationLocked && lp_PunchDistance > 0f)
        {
             scriptLH.StopAllCoroutines();
             scriptLH.StartCoroutine(scriptLH.MoveFist(rp_PunchDistance));

        }

        lp_ChargeTime = 0f;


    }
   
    
    private void LeftPunch(InputAction.CallbackContext context) 
    {
        //Debug.Log("GRAB");


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
            targetPoint01.transform.localPosition = downRight * rp_PunchDistance;

        }
        else if (aimDirection.x > -0.25f && aimDirection.y < -0.25f)
        {

            //DIRECTION DOWN    
            targetPoint01.transform.localPosition = down * rp_PunchDistance; // * distance 0.5, 1, 2
        }
        else if (aimDirection.x < -0.25f && aimDirection.y < -0.25f)
        {
            //DIRECTION DOWNLEFT 
            targetPoint01.transform.localPosition = downLeft * rp_PunchDistance;

        }
        else if (aimDirection.x < -0.25f && aimDirection.y < 0.25f)
        {
            //DIRECTION LEFT 
            targetPoint01.transform.localPosition = left * rp_PunchDistance;

        }
        else if (aimDirection.x < -0.25f && aimDirection.y > 0.25f)
        {
            //DIRECTION UPLEFT 
            targetPoint01.transform.localPosition = upLeft * rp_PunchDistance;

        }
        else if (aimDirection.x < 0.25f && aimDirection.y > 0.25f)
        {
            //DIRECTION UP
            targetPoint01.transform.localPosition = up * rp_PunchDistance;

        }
        else if (aimDirection.x > 0.25f && aimDirection.y > 0.25f)
        {
            //DIRECTION UPRIGHT
            targetPoint01.transform.localPosition = upRight * rp_PunchDistance;

        }
        else if (aimDirection.x > 0.25f && aimDirection.y < 0.25f)
        {
            //DIRECTION RIGHT
            targetPoint01.transform.localPosition = right * rp_PunchDistance;

        }
        else
        {
            targetPoint01.transform.localPosition = Vector2.zero;
        }



        //LEFT STICK TARGET POINT || TARGET POINT 2 ROTATION
        if (aimDirection.x > 0.25f && aimDirection.y < -0.25f)
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
