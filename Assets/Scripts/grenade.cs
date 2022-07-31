using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidBody;
    [SerializeField] int iSpeed;
    [SerializeField] int iTimer;

    [SerializeField] GameObject gExplosion;

    // Start is called before the first frame update
    void Start()
    {
        // When spawned grenade will move forward and up, arcing towards player
        _rigidBody.velocity = ((GameManager._instance._player.transform.position - transform.position) + new Vector3(0, 0.5f, 0) * iSpeed);
        StartCoroutine(explosionTime());
    }

    IEnumerator explosionTime()
    {
        yield return new WaitForSeconds(iTimer);
        Instantiate(gExplosion, transform.position, gExplosion.transform.rotation);

        Destroy(gameObject);
    }
}
