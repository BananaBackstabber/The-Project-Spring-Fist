using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalVariables : MonoBehaviour
{

    public float gameScore;
    public float gameTime;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        if (Input.GetKeyDown("1")) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        //Debug.Log("Score is: " + gameScore);
    }
}
