using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] int iSpikeDamage;
    [SerializeField] float fDamageTime;
    [SerializeField] bool canTakeDamage = true; 

    public void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player" ) //Checks if the player is colliding
        {
            if (collider.GetComponent<IDamageable>() != null) //Checks for Idamageable
            {
                IDamageable isDamagable = collider.GetComponent<IDamageable>();

                if (canTakeDamage)
                {
                    StartCoroutine(DamagePlayer()); //Damage per time assigned 
                    isDamagable.TakeDamage(iSpikeDamage);
                    //Audio for player touching spikes here
                }

            }
        }
    }

    private IEnumerator DamagePlayer()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(fDamageTime);
        canTakeDamage = true; 
    }
}
