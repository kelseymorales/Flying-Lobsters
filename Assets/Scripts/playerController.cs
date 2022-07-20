using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class playerController : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] CharacterController _controller;

    [Header("Player Attributes")]
    [Header("--------------------------")]
    [Range(5, 20)][SerializeField] int iPlayerHealth;
    [Range(3, 6)][SerializeField] float fPlayerSpeed;
    [Range(1.5f, 4.0f)][SerializeField] float fSprintMulti; // multiplier for increasing player speed during use of the sprint() function
    [Range(6, 10)][SerializeField] float fJumpHeight;
    [Range(15, 30)][SerializeField] float fGravityValue;
    [Range(1, 4)][SerializeField] int iJumps; // Max jumps allowed

    [Header("Player Weapon Stats")]
    [Header("-------------------------")]
    [Range(0.1f, 3.0f)][SerializeField] float fShootRate;
    [Range(1, 10)][SerializeField] int iWeaponDamage;

    [Header("Effects")]
    [Header("--------------------------")]
    [SerializeField] GameObject _hitEffectSpark;
    [SerializeField] GameObject _muzzleFlash;

    [Header("Physics")]
    [Header("--------------------------")]
    public Vector3 vPushBack = Vector3.zero;
    [SerializeField] int iPushBackResolve;

    [Header("Audio")]
    [Header("--------------------------")]
    public AudioSource aud;
    [SerializeField] AudioClip[] aGunShot;
    [Range(0.0f, 1.0f)][SerializeField] float aGunShotVol;
    [SerializeField] AudioClip[] aPlayerHurt;
    [Range(0.0f, 1.0f)][SerializeField] float aPlayerHurtVol;
    [SerializeField] AudioClip[] aPlayerReload;
    [Range(0.0f, 1.0f)][SerializeField] float aPlayerReloadVol;
    [SerializeField] AudioClip[] aPlayerJump;
    [Range(0.0f, 1.0f)][SerializeField] float aPlayerJumpVol;
    [SerializeField] AudioClip[] aPlayerFootsteps;
    [Range(0.0f, 1.0f)][SerializeField] float aPlayerFootstepsVol;
    [SerializeField] AudioClip[] aPlayerEmptyClip;
    [Range(0.0f, 1.0f)][SerializeField] float aPlayerEmptyClipVol;
    [SerializeField] AudioClip[] aHeadShot;
    [Range(0.0f, 1.0f)][SerializeField] float aHeadShotVol;
    [SerializeField] AudioClip[] aYouWin;
    [Range(0.0f, 1.0f)][SerializeField] float aYouWinVol;
    [SerializeField] AudioClip[] aYouLose;
    [Range(0.0f, 1.0f)][SerializeField] float aYouLoseVol;
    [SerializeField] AudioClip[] aDefuseNoise;
    [Range(0.0f, 1.0f)][SerializeField] float aDefuseNoiseVol;

    bool canShoot = true;
    bool isSprinting = false;

    float fPlayerSpeedOrig; // stores the starting player speed
    int iPlayerHealthOrig; // stores the starting player health
    int iTimesJumped;
    
    Vector3 _playerVelocity;
    Vector3 _move;
    Vector3 _playerSpawnPos;

    // Called at Start
    void Start()
    {
        // Store starting values for important variables
        iPlayerHealthOrig = iPlayerHealth;
        fPlayerSpeedOrig = fPlayerSpeed;
        _playerSpawnPos = transform.position;
    }

    // Called every frame
    void Update()
    {
        if (!GameManager._instance.isPaused) // if paused - disable player actions
        {
            MovePlayer();
            Sprint();
            StartCoroutine(Shoot());
        }
    }

    private void MovePlayer()
    {
        if ((_controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            _playerVelocity.y -= 2;
        }

        // if we land - reset player velocity and reset jump counter
        if (_controller.isGrounded && _playerVelocity.y < 0)
        {
            iTimesJumped = 0;
            _playerVelocity.y = 0f;
        }

        // receive input from Unity input manager
        _move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));

        // Add our move vector into the character controller move
        _controller.Move(_move * Time.deltaTime * fPlayerSpeed);

        // Make player jump and increment jump counter
        if (Input.GetButtonDown("Jump") && iTimesJumped < iJumps)
        {
            iTimesJumped++;
            _playerVelocity.y = fJumpHeight;
        }

        // add gravity
        _playerVelocity.y -= fGravityValue * Time.deltaTime;

        // add gravity back into the character controller move
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    void Sprint()
    {
        // on down press of 'Sprint' key, increase player speed
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            fPlayerSpeed = fPlayerSpeed * fSprintMulti;
        }
        // on release of 'sprint' key, return player speed to normal
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            fPlayerSpeed = fPlayerSpeedOrig;
        }
    }

    IEnumerator Shoot()
    {
        // get input for shooting
        if (Input.GetButton("Shoot") && canShoot)
        {
            // turns shooting off so it cant be immediately executed again
            canShoot = false;

            RaycastHit hit;

            // Casts a ray from the player camera and performs an action where the ray hits
            if (Physics.Raycast(UnityEngine.Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit))
            {
                // play spark effect where ray hits
                Instantiate(_hitEffectSpark, hit.point, _hitEffectSpark.transform.rotation);

                // if the target is damageable, it takes damage
                if (hit.collider.GetComponent<IDamageable>() != null)
                {
                    // get target
                    IDamageable isDamageable = hit.collider.GetComponent<IDamageable>();

                    // check for body shot or head shot
                    if (hit.collider is SphereCollider) // apply damage for head shot
                    {
                        isDamageable.TakeDamage(10000);
                    }
                    else
                    {
                        isDamageable.TakeDamage(iWeaponDamage); // apply damage for body shot
                    }
                }
            }
            // muzzle flash effect
            _muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360)); // rotate effect along Z-axis randomly for every shot
            _muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            _muzzleFlash.SetActive(false);

            // Timer runs, based off of player shootRate, and then re-enables shooting
            yield return new WaitForSeconds(fShootRate);
            canShoot = true;
        }
    }

    public void TakeDamage(int iDmg)
    {
        iPlayerHealth -= iDmg; 
        UpdateHealthBar(); 
        StartCoroutine(DamageFlash()); // flash screen red when damage is taken

        if (iPlayerHealth <= 0) 
        {
            GameManager._instance.PlayerDead(); 
        }
    }
    IEnumerator DamageFlash() // Helper function that creates flash upon taking damage
    {
        GameManager._instance._playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager._instance._playerDamageFlash.SetActive(false);
    }
    
    public void Respawn()
    {
        iPlayerHealth = iPlayerHealthOrig; // reset player health
        _controller.enabled = false;
        transform.position = _playerSpawnPos; // reset player location
        _controller.enabled = true;
        vPushBack = Vector3.zero;

        UpdateHealthBar();
    }

    public void HealthPack()
    {
        if (iPlayerHealth < iPlayerHealthOrig)
        {
            iPlayerHealth += 1;
            UpdateHealthBar();
        }
    }

    public void UpdateHealthBar() // update the healthbar in the UI to reflect player's current health
    {
        GameManager._instance._HpBar.fillAmount = (float)iPlayerHealth / (float)iPlayerHealthOrig;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("HealthPack"))
        {
            HealthPack();
            Destroy(other.gameObject);
        }
        
    }

    public void LockInPlace() // stop player movement for scripted events
    {
        fPlayerSpeed = 0;
    }

    public void UnlockInPlace() // allow player movement after scripted event ends
    {
        fPlayerSpeed = fPlayerSpeedOrig;
    }

    public void loseJingle() // jingle for lose and death screen
    {
        aud.PlayOneShot(aYouLose[Random.Range(0, aYouLose.Length)], aYouLoseVol);
    }

    public void winJingle() // jingle for win screen
    {
        aud.PlayOneShot(aYouWin[Random.Range(0, aYouWin.Length)], aYouWinVol);
    }

    public void defuseJingle() // jingle for succesful defusing of a bomb
    {
        aud.PlayOneShot(aDefuseNoise[Random.Range(0, aDefuseNoise.Length)], aDefuseNoiseVol);
    }
}
