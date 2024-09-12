using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Grab : MonoBehaviour
{
    
    private GameObject leftHand;
    private Rigidbody2D rb;
    private LeftHand scriptLH;
    private Obj_Grab grab;
    public bool isHeld;
 


    private void Awake()
    {
        grab = GetComponent<Obj_Grab>();
        rb = GetComponent<Rigidbody2D>();
        leftHand = GameObject.Find("LeftHand");
        scriptLH = leftHand.GetComponent<LeftHand>();

    }




    public IEnumerator MoveWithFist() 
    {
  

        while (scriptLH.isGrabbed == true) 
        {
            //Object is moving with player's ledt fist
            transform.position = leftHand.transform.position;
            yield return null;

        }
       
        //Moving with fist has stopped
        IsGrabbed();
        StopAllCoroutines();

    }

    public void IsGrabbed() 
    {
 
        isHeld = !isHeld;
 
    }
}
