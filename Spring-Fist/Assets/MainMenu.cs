using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGame() 
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } 

    public void ExitGame() 
    {


         
        SceneManager.LoadScene(2);
    
    
    }

    public void Settings() 
    {
       //Load settings screen stuff
    
    }

    public void HowTo() 
    {
       //could be another scene
    
    }
}
