using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dpDespawn : MonoBehaviour
{

    private float life;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        life += Time.deltaTime;

        if(life >= 1) 
        {
            Destroy(gameObject);
        }
    }
}
