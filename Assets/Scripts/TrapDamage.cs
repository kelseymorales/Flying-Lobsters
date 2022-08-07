using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [Header("Hazarodous Traps\n------------------------------")]
    //Particle system for the acid and fire traps
    [SerializeField] GameObject _acidParticles;
    [SerializeField] GameObject _fireParticles;

    //Checks if the trap is fire or acid
    [SerializeField] bool isFire;
    [SerializeField] bool isAcid; 
    
    //Controlls timer for damage
    private bool canTakeDamage = true;
    //Amount of damage (per timer assigned)
    [SerializeField] int fHazardousTrapDamage;

    [SerializeField] float fTakeDamageRate; //Time rate of damage
    [SerializeField] float fHazardousTrapDuration; //How long the trap is going to be active (if modified, also modify the particle system duration)

    [Header("Spike Trap\n------------------------------")]
    [SerializeField] GameObject _spikes; 

    [Header("Grenade Trap\n------------------------------")]
    [SerializeField] GameObject _tripWire; //Trip wire object

    [Header("Type Trap\n------------------------------")]
    //Controls what trap is going to be activated
    [SerializeField] bool isHazardousTrap;
    [SerializeField] bool isSpikeTrap;
    [SerializeField] bool isGranadeTrap;

    [Header("Components\n------------------------------")]
    [SerializeField] float fStartTimeDelay; 
    [SerializeField] GameObject _roof; //Roof of the trap
    private bool isTrapActive = true; //Checks if the trap is active or not


    private void Start()
    {
        if (isSpikeTrap)
        {
            _roof.SetActive(false);
            Instantiate(_spikes, transform.position - new Vector3(0, 1.8f, 0), _spikes.transform.rotation);
        }
        if(isGranadeTrap)
        {
            _roof.SetActive(false); 
            Instantiate(_tripWire, transform.position +  new Vector3(0,-1.8f, 0f), _tripWire.transform.rotation);
        }
    }

    private IEnumerator ActivateTrap() //Start the trap based on a timer 
    {

        //Waits for delay
        yield return new WaitForSeconds(fStartTimeDelay); 
        //Checks if acid or fire trap
        if(isAcid)
        {
            Instantiate(_acidParticles, _roof.transform.position, _acidParticles.transform.rotation); //Instantiate acid particle system
            //Acid droping Audio here (loop) 
        }
            
            
        if(isFire)
        {
            for (int i = 0; i < (int)Random.Range(3,5); i++)
                InstantiateFIreRandom(); //Instantiate fire particle system, multiple times because its a small flame
            //Fire burning Audio here (loop)
        }
            
        isTrapActive = true; //Sets trap back to active
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
            //Controls the trap damage or activation type
            if (isHazardousTrap)
            {
                //Checks trap type and checks for IDamageable component
                if (collider.GetComponent<IDamageable>() != null)
                {
                    IDamageable isDamagable = collider.GetComponent<IDamageable>();

                    if(canTakeDamage)
                    {
                        //Calls Ienumerator so the player can receive damage per time assigned 
                        StartCoroutine(ConstantDamageTrap());
                        isDamagable.TakeDamage(fHazardousTrapDamage);

                        if(isFire)
                        {
                            //Audio for player interacting here (fire)
                        }
                        if(isAcid)
                        {
                            //Audio for player interacting here (acid)
                        }

                    }
                        
                }
            }

        }
    }



    private IEnumerator ConstantDamageTrap()
    {
        //Damage per timer 
        canTakeDamage = false;
        yield return new WaitForSeconds(fTakeDamageRate);
        canTakeDamage = true; 
        
    }

    private IEnumerator StopTrapTimer()
    {
        //Stops trap
        yield return new WaitForSeconds(fHazardousTrapDuration);
        isTrapActive = false; //Deactivates trap
    }

    private void InstantiateFIreRandom()
    {
        //Creates a random spawn for the fire particles
        Vector3 ranPos = new Vector3(Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2), -2f, Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2)); //Sets random position 
        Instantiate(_fireParticles, transform.position + ranPos, _fireParticles.transform.rotation); //Using random position to instantiate particle system inside the collider
    }



}
