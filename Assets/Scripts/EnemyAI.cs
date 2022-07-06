using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] NavMeshAgent nAgent;
    [SerializeField] Renderer rRend;

    [Header("------------------------------")]
    [Header("Enemy Attributes")]
    [SerializeField] int iHP;

    [Header("------------------------------")]
    [Header("Weapon Stats")]
    [SerializeField] float fShootRate;
    [SerializeField] GameObject gBullet;

    bool bCanShoot = true;

    void Start()
    {
    }

    void Update()
    {
        nAgent.SetDestination(GameManager._instance._player.transform.position);

        if (nAgent.remainingDistance <= nAgent.stoppingDistance && bCanShoot)
            StartCoroutine(Shoot());
    }

    public void TakeDamage(int iDamage)
    {
        iHP -= iDamage;
        StartCoroutine(FlashColor());

        if(iHP <= 0)
            Destroy(gameObject);
    }

    IEnumerator FlashColor()
    {
        rRend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rRend.material.color = Color.white;
    }

    IEnumerator Shoot()
    {
        bCanShoot = false;
        Instantiate(gBullet, transform.position, gBullet.transform.rotation);
        yield return new WaitForSeconds(fShootRate);
        bCanShoot=true;
    }
}
