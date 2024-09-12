using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{

    
    private int enemyHP;
    private SpriteRenderer spriteRenderer;

    private Color curColour;
    public Color damageColour;
    private float curTime;
    public float flashTime;
    public int flashAmount;



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

    private void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        curColour = spriteRenderer.color;
    }


    public void TakeDamage(int damage) 
    {

        //Debug.Log("DAMAGE");
        enemyHP -= damage;

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

        //Color eColor = gameObject.GetComponent<SpriteRenderer>().color;
        //curColour = eColor;

      
    
    }
}
