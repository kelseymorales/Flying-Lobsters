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
        // if player or enemy is caught in explosion range
        if ((other.CompareTag("Player") || other.CompareTag("Enemy")) && other.tag != tag)
        {
            // apply physics pushback to player character
            GameManager._instance._playerScript.vPushBack =
                (GameManager._instance._player.transform.position - transform.position) * iDamage;

            // if the target is damageable, it takes damage
            Ray ray = new Ray(transform.position, other.transform.position - transform.position);
            RaycastHit hit;
            Debug.DrawRay(transform.position, other.transform.position - transform.position, Color.red, 10);

            if (Physics.Raycast(ray, out hit, GetComponent<SphereCollider>().radius))
            {
                if (hit.collider.CompareTag("wall"))
                {
                    return;
                }
  
                // get target
                IDamageable isDamageable = other.GetComponent<IDamageable>();

                // apply damage
                isDamageable.TakeDamage(iDamage);
            }
        }
    }
}
