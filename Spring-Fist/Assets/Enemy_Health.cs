using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Enemy_Health : MonoBehaviour
{

    private GlobalVariables global;
    public int eHP;
    private SpriteRenderer spriteRenderer;

    //Damage colour variables
    private Color curColour;
    public Color damageColour;

    //Time Float Variables
    private float curTime;
    public float flashTime;
    public int flashAmount;
    private float timeToDie;
    //Death effect
    public GameObject particleEffect;
    private EnemyKnockBack knockBack;
    private waveScript wavescript;

    private float dieCount = 0f;

    public AudioClip deathSound;
    private AudioSource audiosource;
    /// <summary>
    /// 
    /// when enemy HP is reduced then
    /// the enemy should flash the targeted colour
    /// for a few moments
    /// 
    /// make a corutine called flash and have it
    /// last while count number is below 1 seconds
    /// after the while loop has past reset the loop
    /// and then stop the coroutine
    /// </summary>


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockBack = GetComponent<EnemyKnockBack>();
        curColour = spriteRenderer.color;

        global = GameObject.Find("Global_Object").GetComponent<GlobalVariables>();
        wavescript = GameObject.Find("Level_Settings").GetComponent<waveScript>();
        audiosource = GetComponent<AudioSource>();

    }
    


    public void TakeDamage(int damage) 
    {

        //Debug.Log("DAMAGE");
        eHP -= damage;

        //The enemy start to flash red for a few seconds
        StartCoroutine(DamageFlash());
    }
    public IEnumerator DamageFlash() 
    {

        for(int i = 0; i < flashAmount; i++) //flash 3 times
        {
            
            //Change To flash colour
            spriteRenderer.color = damageColour;

            yield return new WaitForSeconds(flashTime / 2);
            //Revert to the original colour 
            spriteRenderer.color = curColour;
            yield return new WaitForSeconds(flashTime / 2);


        }


        if(eHP <= 0) 
        {
            StartCoroutine(DamageFlash());

            timeToDie += Time.deltaTime;
        }

        if(timeToDie >= 0.25f && dieCount == 0) 
        {
            Death();
            dieCount += 1;
        }
        //Color eColor = gameObject.GetComponent<SpriteRenderer>().color;
        //curColour = eColor;

      
    
    }

    public void Death() 
    {
       
        if(global != null) 
        {
            global.gameScore += 10;
        }

        audiosource.PlayOneShot(deathSound, 0.7f);
        Instantiate(particleEffect, transform.position, transform.rotation);
        wavescript.deadCount += 1;
        Destroy(gameObject);
    }
}
