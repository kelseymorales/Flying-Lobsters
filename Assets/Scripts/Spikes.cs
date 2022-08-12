using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] int iSpikeDamage;
    [SerializeField] float fDamageTime;
    [SerializeField] bool canTakeDamage = true;

    // audio components
    public AudioSource aud;
    [SerializeField] AudioClip[] aSpikeTrap;
    [Range(0.0f, 1.0f)][SerializeField] float aSpikeTrapVol;

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
                    aud.PlayOneShot(aSpikeTrap[Random.Range(0, aSpikeTrap.Length)], aSpikeTrapVol);
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
