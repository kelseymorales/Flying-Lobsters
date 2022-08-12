using System.Collections;
using UnityEngine;

enum TrapType
{
    acid,
    fire,
    spike,
    tripWire
}

public class TrapDamage : MonoBehaviour
{
    [Header("Hazarodous Traps\n------------------------------")]
    //Particle system for the acid and fire traps
    [SerializeField] GameObject _acidParticles;
    [SerializeField] GameObject _fireParticles;

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
    [SerializeField] private TrapType _type;

    [Header("Audio\n------------------------------")]
    public AudioSource aud; //trap audio source
    //clips for traps audio
    [SerializeField] AudioClip[] aFireTrap;
    [Range(0.0f, 1.0f)][SerializeField] float aFireTrapVol;
    [SerializeField] AudioClip[] aAcidTrap;
    [Range(0.0f, 1.0f)][SerializeField] float aAcidTrapVol;

    [Header("Components\n------------------------------")]
    [SerializeField] float fStartTimeDelay;
    [SerializeField] GameObject _roof; //Roof of the trap
    private bool isTrapActive = true; //Checks if the trap is active or not


    private void Start()
    {
        if (_type == TrapType.spike)
        {
            _roof.SetActive(false);
            Instantiate(_spikes, transform.position - new Vector3(0, 1.8f, 0), _spikes.transform.rotation);
        }
        else if (_type == TrapType.tripWire)
        {
            _roof.SetActive(false);
            Instantiate(_tripWire, transform.position + new Vector3(0, -1.8f, 0f), _tripWire.transform.rotation);
        }
    }

    private IEnumerator ActivateTrap() //Start the trap based on a timer 
    {

        //Waits for delay
        yield return new WaitForSeconds(fStartTimeDelay);
        //Checks if acid or fire trap

        if (_type == TrapType.acid)
        {
            Instantiate(_acidParticles, _roof.transform.position, _acidParticles.transform.rotation); //Instantiate acid particle system
                                                                                                      //Acid droping Audio here (loop) 
        }

        if (_type == TrapType.fire)
        {
            for (int i = 0; i < (int)Random.Range(3, 5); i++)
            {
                InstantiateFIreRandom(); //Instantiate fire particle system, multiple times because its a small flame
                                         //Fire burning Audio here (loop)
            }
        }

        isTrapActive = true; //Sets trap back to active
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && (_type == TrapType.acid || _type == TrapType.fire))
        {
            StartCoroutine(StopTrapTimer());
            StartCoroutine(ActivateTrap());
        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player" && isTrapActive && (_type == TrapType.acid || _type == TrapType.fire))
        {
            //Checks trap type and checks for IDamageable component
            if (collider.GetComponent<IDamageable>() != null)
            {
                IDamageable isDamagable = collider.GetComponent<IDamageable>();

                if (canTakeDamage)
                {
                    //Calls Ienumerator so the player can receive damage per time assigned 
                    StartCoroutine(ConstantDamageTrap());
                    isDamagable.TakeDamage(fHazardousTrapDamage);

                    if (_type == TrapType.fire)
                    {
                        aud.PlayOneShot(aFireTrap[Random.Range(0, aFireTrap.Length)], aFireTrapVol);
                    }
                    else if (_type == TrapType.acid)
                    {
                        aud.PlayOneShot(aAcidTrap[Random.Range(0, aAcidTrap.Length)], aAcidTrapVol);
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