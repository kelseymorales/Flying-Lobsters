using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [Header("Hazarodous Traps\n------------------------------")]
    [SerializeField] GameObject _acidParticles;
    [SerializeField] GameObject _fireParticles;

    [SerializeField] bool isFire;
    [SerializeField] bool isAcid; 
    
    private bool canTakeDamage = true;

    [SerializeField] int fHazardousTrapDamage;

    [SerializeField] float fTakeDamageRate;
    [SerializeField] float fHazardousTrapDuration;

    [Header("Spike Trap\n------------------------------")]
    [SerializeField] int fSpikeTrapDamage;

    [Header("Grenade Trap\n------------------------------")]
    [SerializeField] GameObject _grenadeToSpawn; 

    [Header("Type Trap\n------------------------------")]
    [SerializeField] bool isHazardousTrap;
    [SerializeField] bool isSpikeTrap;
    [SerializeField] bool isGranadeTrap;

    [Header("Components\n------------------------------")]
    [SerializeField] float fStartTimeDelay;
    [SerializeField] GameObject _roof;
    private bool isTrapActive = true;




    private IEnumerator ActivateTrap()
    {
        yield return new WaitForSeconds(fStartTimeDelay); 
        if(isAcid)
            Instantiate(_acidParticles,_roof.transform.position, _acidParticles.transform.rotation);
        if(isFire)
        {
            for (int i = 0; i < (int)Random.Range(3,5); i++)
                InstantiateFIreRandom();
        }
            
        isTrapActive = true; 
    }


    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            StartCoroutine(StopTrapTimer()); 
            StartCoroutine(ActivateTrap());
        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player" && isTrapActive)
        {

            if (isHazardousTrap)
            {
                if (collider.GetComponent<IDamagable>() != null)
                {
                    IDamagable isDamagable = collider.GetComponent<IDamagable>();

                    if(canTakeDamage)
                    {
                        StartCoroutine(ConstantDamageTrap());
                        isDamagable.takeDamage(fHazardousTrapDamage);
                    }
                        
                }
            }
            else if (isGranadeTrap)
            {
                ThrowGrenades(); 
            }
        }
    }



    private IEnumerator ConstantDamageTrap()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(fTakeDamageRate);
        canTakeDamage = true; 
        
    }

    private IEnumerator StopTrapTimer()
    {
        yield return new WaitForSeconds(fHazardousTrapDuration);
        isTrapActive = false; 
    }

    private void InstantiateFIreRandom()
    {
        Vector3 ranPos = new Vector3(Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2), -1f, Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2));
        Instantiate(_fireParticles, transform.position + ranPos, _fireParticles.transform.rotation); 
    }

    private void ThrowGrenades()
    {
        float throwForce = .05f; 
        Vector3 ranPos = new Vector3(Random.Range((-transform.localScale.x / 2) * throwForce, (transform.localScale.x / 2) * throwForce), 0f, Random.Range((-transform.localScale.z / 2) * throwForce, (transform.localScale.z / 2) * throwForce));
        Instantiate(_grenadeToSpawn, _roof.transform.position + ranPos, _grenadeToSpawn.transform.rotation);
        isTrapActive = false; 
    }


}
