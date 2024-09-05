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

    public void ApplyKnockBack(Vector2 direction)
    {
        Vector2 knockbackDirection;

        knockbackDirection = direction + knockUp;

        Debug.Log("KNOCK KNOCK");
        if(rb == null) 
        {
            return;
        }

        //Resets velocity
        rb.velocity = Vector2.zero;

        knockbackForce = playerControls.rightPunchSpeed * 1.5f;

        //Apply the knockback to a direction
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }
}
