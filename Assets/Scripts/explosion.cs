using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] int iDamage;

    [Header("Audio")]
    [Header("--------------------------")]
    public AudioSource aud;
    [SerializeField] AudioClip[] aExplosionSound;
    [Range(0.0f, 1.0f)][SerializeField] float aExplosionSoundVol;

    void Start()
    {
        aud.PlayOneShot(aExplosionSound[Random.Range(0, aExplosionSound.Length)], aExplosionSoundVol);
    }

    public void OnTriggerEnter(Collider other)
    {
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
