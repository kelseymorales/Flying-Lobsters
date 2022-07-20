using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] NavMeshAgent nAgent;
    [SerializeField] Renderer rRend;
    [SerializeField] Animator aAnim;

    [Header("------------------------------")]
    [Header("Enemy Attributes")]
    [SerializeField] int iHP;
    [SerializeField] int iViewAngle; // fov
    [SerializeField] int iPlayerFaceSpeed;
    [SerializeField] int iRoamingRadius;

    [Header("------------------------------")]
    [Header("Weapon Stats")]
    [SerializeField] float fShootRate;
    [SerializeField] GameObject gBullet;
    [SerializeField] GameObject gShootPosition;

    [Header("------------------------------")]
    [Header("Audio")]
    public AudioSource aud;
    [SerializeField] AudioClip[] gunShot;
    [Range(0.0f, 1.0f)][SerializeField] float gunShotVol;

    bool bCanShoot = true;
    bool bPlayerInRange;

    Color _enemyColor;
    Vector3 vStartingPos;
    Vector3 vPlayerDirection;

    float fStoppingDistanceOrig;

    void Start()
    {
        vStartingPos = transform.position;
        fStoppingDistanceOrig = nAgent.stoppingDistance;
        _enemyColor = rRend.material.color;
        GameManager._instance.updateEnemyCount();
    }

    void Update()
    {
        if (nAgent.isActiveAndEnabled)
        {
            aAnim.SetFloat("Speed", Mathf.Lerp(aAnim.GetFloat("Speed"), nAgent.velocity.normalized.magnitude, Time.deltaTime * 5));

            vPlayerDirection = GameManager._instance._player.transform.position - transform.position;

            if (bPlayerInRange)
            {
                //agent lets enemies know where player is
                nAgent.SetDestination(GameManager._instance._player.transform.position);

                CanSeePlayer();
                facePlayer();
            }
            else if (nAgent.remainingDistance < 0.1f)
            {
                roam();
            }
        }
    }

    void roam()
    {
        nAgent.stoppingDistance = 0;

        // get vector inside sphere multiplied by enemy roamingRadius
        Vector3 randomDirection = Random.insideUnitSphere * iRoamingRadius;
        randomDirection += vStartingPos;

        // sample direction to make sure its within bounds
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, iRoamingRadius, 1);
        NavMeshPath path = new NavMeshPath();

        // create new path based on path sampled above
        nAgent.CalculatePath(hit.position, path);
        nAgent.SetPath(path);
    }

    void facePlayer()
    {
        // if enemy has reached it's destination
        if (nAgent.remainingDistance <= nAgent.stoppingDistance)
        {
            vPlayerDirection.y = 0;
            Quaternion rotation = Quaternion.LookRotation(vPlayerDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * iPlayerFaceSpeed);
        }
    }

    void CanSeePlayer()
    {
        // return angle between player and enemy
        float angle = Vector3.Angle(vPlayerDirection, transform.forward);

        RaycastHit hit;

        // determine if something is inbetween enemy and player
        if (Physics.Raycast(transform.position, vPlayerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && bCanShoot && angle <= iViewAngle)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    // Function for checking if player is in range of enemy
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayerInRange = true;
            bCanShoot = true;
            nAgent.stoppingDistance = fStoppingDistanceOrig;
        }
    }

    // Function for checking if player is leaving enemy's attack range
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayerInRange = false;
            nAgent.stoppingDistance = 0;
        }
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
        aAnim.SetTrigger("Shoot");
        Instantiate(gBullet, gShootPosition.transform.position, gBullet.transform.rotation);
        yield return new WaitForSeconds(fShootRate);
        bCanShoot=true;
    }
}
