using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    private Animator animator;

    private bool IS_FacingRight = true;


    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;

        //Move left and right without accelaration and decellration involed in movement
        if(Input.GetKey(KeyCode.LeftArrow)) 
        {
            movement = Vector3.left; 
         
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movement = Vector3.right;
        }


        //controls the way the player is facing
        if(movement.x > 0 && !IS_FacingRight)
        {
            Flip();
        }
        else if(movement.x < 0 && IS_FacingRight) 
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
