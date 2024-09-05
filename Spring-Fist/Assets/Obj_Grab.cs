using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Grab : MonoBehaviour
{
    
    private GameObject leftHand;
    private Rigidbody2D rb;
    private LeftHand scriptLH;



    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        leftHand = GameObject.Find("LeftHand");
        scriptLH = leftHand.GetComponent<LeftHand>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(scriptLH.isGrabbed == false) 
        {
           
            StopAllCoroutines();
        }
        
        
    }

    public IEnumerator MoveWithFist() 
    {
        

        while (scriptLH.isGrabbed == true) 
        {
            transform.position = leftHand.transform.position;
            yield return null;

        }

       
    
    }
}
