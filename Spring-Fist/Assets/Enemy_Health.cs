using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{

    
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

        if(timeToDie >= 2) 
        {
            Death();
        
        }
        //Color eColor = gameObject.GetComponent<SpriteRenderer>().color;
        //curColour = eColor;

      
    
    }

    public void Death() 
    {

        Instantiate(particleEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
