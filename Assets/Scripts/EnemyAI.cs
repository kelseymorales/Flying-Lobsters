using System.Collections;
using UnityEngine;
using UnityEngine.AI;



public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] NavMeshAgent nAgent;               // enemy nav mesh
    [SerializeField] Renderer rRend;                    // enemy renderer
    [SerializeField] Animator aAnim;                    // enemy animator

    [Header("------------------------------")]
    [Header("Enemy Attributes")]
    [SerializeField] int iHP;                           // enemy health
    [SerializeField] int iViewAngle;                    // enemy field of view
    [SerializeField] int iPlayerFaceSpeed;              // speed at which enemy rotates to face player while tracking them
    [SerializeField] int iRoamingRadius;                // radius the enemy pathfinding is allowed to roam in

    [Header("------------------------------")]
    [Header("Weapon Stats")]
    [SerializeField] float fShootRate;                  // Rate at which enemy can fire their weapon
    [SerializeField] GameObject gBullet;                // stores enemy bullet object (can be used to store various object that will be used like the bullet is - example, grenades)
    [SerializeField] GameObject gShootPosition;         // stores position at which bullets are instantiated (in the case of guns, should be at the muzzle, for grenades it should be in an empty hand)

    [Header("------------------------------")]
    [Header("Drops")]
    [SerializeField] GameObject gHealthPack;            // slot for drops - healthpack (not currently implemented)
    [SerializeField] GameObject gAmmoBox;               // slot for drops - ammo pack (not currently implemented)

    [Header("------------------------------")]
    [Header("Audio")]
    public AudioSource aud;                             // enemy audio source

    // enemy audio clips and clip volume
    [SerializeField] AudioClip[] aGunShot;
    [Range(0.0f, 1.0f)][SerializeField] float aGunShotVol;

    bool bCanShoot = true;                              // value for whether enemy can currently fire their weapon
    bool bPlayerInRange;                                // value tracking whether the player is in range of enemyAI

    Vector3 vStartingPos;                               // vector storing enemy starting position
    Vector3 vPlayerDirection;                           // vector storing the direction the player is in from the perspective of the enemy

    float fStoppingDistanceOrig;                        // float value for how close enemy can get to other enemies, player, and etc

    // Called at Start
    void Start()
    {
        vStartingPos = transform.position;                  // stores starting position
        fStoppingDistanceOrig = nAgent.stoppingDistance;    // stores stopping distance
        GameManager._instance.updateEnemyCount();           // update UI to reflect enemies placed in scene
    }

    // Called every frame
    void Update()
    {
        if (nAgent.isActiveAndEnabled) // if navmesh is enabled
        {
            // pass information to animator on how fast enemy is moving
            aAnim.SetFloat("Speed", Mathf.Lerp(aAnim.GetFloat("Speed"), nAgent.velocity.normalized.magnitude, Time.deltaTime * 5));

            // gets player direction for tracking player
            vPlayerDirection = GameManager._instance._player.transform.position - transform.position;

            if (bPlayerInRange) // if player is in range
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
        bPlayerInRange = true;

        aAnim.SetTrigger("Damage");
        StartCoroutine(FlashColor());

        //if enemy dies then enemy object is destroyed
        if (iHP <= 0)
        {
            
            GameManager._instance.CheckEnemyKills();

            nAgent.enabled = false;
            bCanShoot = false;
            aAnim.SetBool("Dead", true);

            // disable colliders
            foreach(Collider col in GetComponents<Collider>())
            {
                col.enabled = false;
            }
        }
    }

    IEnumerator FlashColor()
    {
        //flash color when hit
        rRend.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        //return back to original color
        rRend.material.color = Color.white;
    }

    IEnumerator Shoot()
    {
        //enemy can shoot player
        bCanShoot = false;
        aAnim.SetTrigger("Shoot");
        aud.PlayOneShot(aGunShot[Random.Range(0, aGunShot.Length)], aGunShotVol);
        Instantiate(gBullet, gShootPosition.transform.position, gBullet.transform.rotation);
        yield return new WaitForSeconds(fShootRate);
        bCanShoot=true;
    }
   
}
