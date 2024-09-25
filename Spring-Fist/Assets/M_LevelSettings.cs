using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class M_LevelSettings : MonoBehaviour
{
    private GlobalVariables global;
    public TextMeshProUGUI UItime;
    private float curTime;
    private float textTime;


    private void Awake()
    {
        UItime = GameObject.Find("The Time").GetComponent<TextMeshProUGUI>();
        global = GameObject.Find("Global_Object").GetComponent<GlobalVariables>();

        curTime = 150; 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (global == null) 
        {
            Debug.LogError("Global == Null");
            return; 
        }
        

        if(curTime > 0) 
        {
            curTime -= Time.deltaTime;

        }
        else 
        {
            curTime = 0f;
        }

        global.gameTime = textTime;
        
        textTime = Mathf.RoundToInt(curTime);
        UItime.text = "Time Left: " + textTime;

    }
}
