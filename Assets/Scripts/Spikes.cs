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
        if (collider.tag == "Player" )
        {
            if (collider.GetComponent<IDamageable>() != null)
            {
                IDamageable isDamagable = collider.GetComponent<IDamageable>();

                if (canTakeDamage)
                {
                    StartCoroutine(DamagePlayer());
                    isDamagable.TakeDamage(iSpikeDamage);
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
