using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidBody;      // stores rigidbody for grenade object
    [SerializeField] int iSpeed;                // stores the speed at which the grenade moves through worldspace
    [SerializeField] int iTimer;                // timer value that counts down to grenade exploding

    [SerializeField] GameObject gExplosion;     // stores explosion effect for grenade detonation

    // Start is called before the first frame update
    void Start()
    {
        // When spawned grenade will move forward and up, arcing towards player
        _rigidBody.velocity = ((GameManager._instance._player.transform.position - transform.position) + new Vector3(0, 0.5f, 0) * iSpeed);

        // start timer countdown for grenade exploding
        StartCoroutine(explosionTime());
    }

    IEnumerator explosionTime()
    {
        // timer countdown
        yield return new WaitForSeconds(iTimer);

        // spawn explosion effect at grenade position
        Instantiate(gExplosion, transform.position, gExplosion.transform.rotation);

        // destroy grenade game object
        Destroy(gameObject);
    }
}
