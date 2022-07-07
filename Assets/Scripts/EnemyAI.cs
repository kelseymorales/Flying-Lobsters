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
    Color _enemyColor;

    void Start()
    {
        _enemyColor = rRend.material.color;
    }

    void Update()
    {
        //agent lets enemies know where player is
        nAgent.SetDestination(GameManager._instance._player.transform.position);

        if (nAgent.remainingDistance <= nAgent.stoppingDistance && bCanShoot)
            StartCoroutine(Shoot());
    }

    public void TakeDamage(int iDamage)
    {
        //when enemy takes damage it flashes a color
        iHP -= iDamage;
        StartCoroutine(FlashColor());

        //if enemy dies then enemy object is destroyed
        if(iHP <= 0)
            Destroy(gameObject);
    }

    IEnumerator FlashColor()
    {
        //flash color when hit
        rRend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        //return back to original color
        rRend.material.color = _enemyColor;
    }

    IEnumerator Shoot()
    {
        //enemy can shoot player
        bCanShoot = false;
        Instantiate(gBullet, transform.position, gBullet.transform.rotation);
        yield return new WaitForSeconds(fShootRate);
        bCanShoot=true;
    }
}
