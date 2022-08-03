using UnityEngine;

public class explosion : MonoBehaviour
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
        // apply physics pushback to player character, if player character is in range of explosion
        if (other.CompareTag("Player"))
        {
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
