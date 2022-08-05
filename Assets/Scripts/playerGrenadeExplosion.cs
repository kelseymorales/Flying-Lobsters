using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGrenadeExplosion : MonoBehaviour
{
    [SerializeField] int iDamage; // stores damage dealt by explosion

    [Header("Audio")]
    [Header("--------------------------")]
    public AudioSource aud;     // explosion audio source

    // explosion audio clip and clip volume
    [SerializeField] AudioClip[] aExplosionSound;
    [Range(0.0f, 1.0f)][SerializeField] float aExplosionSoundVol;

    void Start()
    {
        // when explosion is activated, play explosion audio clip
        aud.PlayOneShot(aExplosionSound[Random.Range(0, aExplosionSound.Length)], aExplosionSoundVol);
    }

    public void OnTriggerEnter(Collider other)
    {
        // if player or enemy is caught in explosion range
        if (other.CompareTag("Enemy"))
        {
            // apply physics pushback to player character
            GameManager._instance._playerScript.vPushBack =
                (GameManager._instance._player.transform.position - transform.position) * iDamage;

            // if the target is damageable, it takes damage
            if (other.GetComponent<IDamageable>() != null)
            {
                // get target
                IDamageable isDamageable = other.GetComponent<IDamageable>();

                // apply damage
                isDamageable.TakeDamage(iDamage);
            }
        }
    }
}