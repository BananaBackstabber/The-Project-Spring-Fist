using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class waveScript : MonoBehaviour
{
    [HideInInspector]
    public float deadCount;
    private int waveCount = 1;
    private GameObject player;

    private LeftHand leftHand;
    private RightHand rightHand;

    private GameObject playerSpawn;

    public GameObject Playerspawn;

    public GameObject obj_wave01;
    public GameObject level_wave01;
    public GameObject obj_wave02;
    public GameObject level_wave02;
    public GameObject obj_wave03;
    public GameObject level_wave03;
    public GameObject obj_wave04;
    public GameObject level_wave04;


    private bool isWave01;
    private bool isWave02;
    private bool isWave03;
    private bool isWave04;
    // Start is called before the first frame update

    private void Awake()
    {

        player = GameObject.Find("Player");
        playerSpawn = GameObject.Find("PlayerSpawnPoint");

        leftHand = GameObject.Find("LeftHand").GetComponent<LeftHand>();
        rightHand = GameObject.Find("RightHand").GetComponent<RightHand>();
        obj_wave01 = GameObject.Find("wave01");
        //level_wave01 = GameObject.Find("Platfroms_01");

        obj_wave02 = GameObject.Find("wave02");
        level_wave02 = GameObject.Find("level-01");

        obj_wave03 = GameObject.Find("wave03");
        level_wave03 = GameObject.Find("level-02");

        obj_wave04 = GameObject.Find("wave04");
        level_wave04 = GameObject.Find("level-04");
    }
    void Start()
    {
       
        Debug.Log(obj_wave01.name);
        obj_wave01.gameObject.SetActive(false);
        //level_wave01.gameObject.SetActive(false);

        obj_wave02.gameObject.SetActive(false);
        level_wave02.gameObject.SetActive(false);

        obj_wave03.gameObject.SetActive(false);
        level_wave03.gameObject.SetActive(false);

        obj_wave04.gameObject.SetActive(false);
        level_wave04.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Deathcount is now:" + deadCount);
        if(deadCount== 2f && waveCount == 1) 
        {
            player.transform.position = playerSpawn.transform.position;
            leftHand.isLocationLocked = true;
            rightHand.isLocationLocked = true;
            obj_wave01.gameObject.SetActive(true);
            // level_wave01.gameObject.SetActive(true);
            waveCount += 1;
        }

        if (deadCount == 6f && waveCount == 2)
        {
            player.transform.position = playerSpawn.transform.position;
            leftHand.isLocationLocked = true;
            rightHand.isLocationLocked = true;
            obj_wave02.gameObject.SetActive(true);
            level_wave02.gameObject.SetActive(true);

            waveCount += 1;
        }

        if (deadCount == 12f && waveCount == 3)
        {
            level_wave02.gameObject.SetActive(false);

            player.transform.position = playerSpawn.transform.position;
            leftHand.isLocationLocked = true;
            rightHand.isLocationLocked = true;

            obj_wave03.gameObject.SetActive(true);
            level_wave03.gameObject.SetActive(true);

            waveCount += 1;
        }

        if (deadCount == 16f && waveCount == 4)
        {
            level_wave03.gameObject.SetActive(false);
            player.transform.position = playerSpawn.transform.position;
            leftHand.isLocationLocked = true;
            rightHand.isLocationLocked = true;

            obj_wave04.gameObject.SetActive(true);
            level_wave04.gameObject.SetActive(true);

            waveCount += 1;

        }

        if(deadCount == 20f && waveCount == 5) 
        {
            player.transform.position = playerSpawn.transform.position;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


        }

    }
}
