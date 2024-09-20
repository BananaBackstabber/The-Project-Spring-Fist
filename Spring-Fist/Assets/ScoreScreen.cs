using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreScreen : MonoBehaviour
{
    //Scripts
    private GlobalVariables global;

    //Text Variables
    public Text scoreText;
    public TextMeshProUGUI scoreTextNumber;
    public TextMeshProUGUI timeText;


    //Float/score variables
    private float pScore = 0;
    private float growthScore = 1;
    public float timeScore;
    private float curTime;

    private float delta; 

    private bool basescoredone;
    private bool isTimeDone = false;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        scoreTextNumber = GameObject.Find("N-Score").GetComponent<TextMeshProUGUI>();
        timeText = GameObject.Find("N-Time").GetComponent<TextMeshProUGUI>();
        global = GameObject.Find("Global_Object").GetComponent<GlobalVariables>();

        timeText.enabled = false;
        timeScore = global.gameTime;


        delta = 2 * Time.deltaTime;
    }
    // Start is called before the first frame update
    void Start()
    {

        scoreTextNumber.text = pScore.ToString();
        curTime = timeScore;
       
        
    }

    // Update is called once per frame
    void Update()
    {
       
        Debug.Log(global.gameScore);


        /*if(timeScore > 35) 
        {
            timeScore -= Time.deltaTime;
        }
        else 
        {
            timeScore = Mathf.RoundToInt(timeScore);

        }*/


        //pScore = growthScore;


        if (!basescoredone) 
        {

            if (pScore < global.gameScore)
            {
                growthScore += growthScore * Time.deltaTime * 2;
            }

            if (pScore > global.gameScore)
            {
                growthScore = global.gameScore;

            }
            if (pScore == global.gameScore)
            {
                timeText.enabled = true;
                basescoredone = true;

            }


        }

        if (basescoredone && !isTimeDone) 
        {
           
           // Debug.Log("TICKING TIME, Cur Time is " + curTime);
            

            if(curTime <= 0.5f) 
            {
                curTime = 0f;
                isTimeDone = true;
            
            }
            else 
            {
                //This code controls the score speed,
                //come back to this code to slow down the speed later 
                curTime -=  (timeScore/ 1000) * 10;
                growthScore +=  (timeScore * 10) / 1000;
            }

 
        }
        
        if (isTimeDone) 
        {
            Debug.Log("GROWTH SCROE IS: " + growthScore);
            //growthScore = growthScore;
        }

        //Display the time left from the level
        timeScore = Mathf.RoundToInt(curTime);
        timeText.text = "Time Left: " + timeScore.ToString();


        //Display the score for the level
        pScore = Mathf.RoundToInt(growthScore);
        scoreTextNumber.text = pScore.ToString();



    }

    private void LateUpdate()
    {

       
        
    }
}
