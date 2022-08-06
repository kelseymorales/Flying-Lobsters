using System.Collections;
using UnityEngine;

public class playerController : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] CharacterController _controller;

    [Header("Player Attributes")]
    [Header("--------------------------")]
    [Range(5, 40)][SerializeField] int iPlayerHealth;               // Player Health 
    [Range(3, 6)][SerializeField] float fPlayerSpeed;               // Player speed
    [Range(1.5f, 4.0f)][SerializeField] float fSprintMulti;         // Multiplier for increasing player speed during use of the sprint() function
    [Range(6, 10)][SerializeField] float fJumpHeight;               // Player Jump height
    [Range(15, 30)][SerializeField] float fGravityValue;            // Player gravity scaler
    [Range(1, 4)][SerializeField] int iJumps;                       // Max jumps allowed
    [Range(1, 40)][SerializeField] int iHealthPickupHealNum;        // Value for how much health packs heal player when picked up
    [Range(1, 40)][SerializeField] int iAmmoPickupAmmoNum;          // Value for how much ammo the ammo pickups give player when picked up

    [Header("Player Weapon Stats")]
    [Header("-------------------------")]
    [Range(0.1f, 3.0f)][SerializeField] float fShootRate;           // Player value for how fast they can shoot
    [Range(1, 10)][SerializeField] int iWeaponDamage;               // Player Weapon Damage
    [Range(1, 25)][SerializeField] public int iWeaponAmmo;          // Player weapon ammo clip size
    [SerializeField] public int iTotalWeaponAmmo;                   // Player weapon ammo pool total
    [SerializeField] GameObject gPlayerGrenade;                     // stores prefab for player grenade
    [SerializeField] public int iGrenadeCount;                      // stores ammo count for player grenades

    [Header("Effects")]
    [Header("--------------------------")]
    [SerializeField] GameObject _hitEffectSpark;                    // Effect shown when player bullet hits something
    [SerializeField] GameObject _muzzleFlash;                       // Effect shown at player weapon muzzle when fired

    [Header("Physics")]
    [Header("--------------------------")]
    public Vector3 vPushBack = Vector3.zero;                        // Vector used for calculating pushback physics on player character
    [SerializeField] int iPushBackResolve;                          // Stores the speed at which pushback is resolved

    [Header("Audio")]
    [Header("--------------------------")]
    public AudioSource aud; // Player Audio Source

    // Audio clips and Audio Clips volume
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
    [SerializeField] AudioClip[] aHeal;
    [Range(0.0f, 1.0f)][SerializeField] float aHealVol;
    [SerializeField] AudioClip[] aAmmo;
    [Range(0.0f, 1.0f)][SerializeField] float aAmmoVol;
    [SerializeField] AudioClip[] aGrenade;
    [Range(0.0f, 1.0f)][SerializeField] float aGrenadeVol;
    [SerializeField] AudioClip[] aGrenadeEmpty;
    [Range(0.0f, 1.0f)][SerializeField] float aGrenadeEmptyVol;

    // Bool variables for player character
    bool canShoot = true;       // indicates whether player is allowed to shoot at any given moment
    bool isSprinting = false;   // Indicates whether the player is currently sprinting
    bool bFootstepsPlaying;     // Indicates if the footsteps audio affect is currently playing

    float fPlayerSpeedOrig;     // stores the starting player speed
    int iPlayerHealthOrig;      // stores the starting player health
    [HideInInspector] public int iWeaponAmmoOrig; // stores the default weapon ammo
    int iTimesJumped;           // How many times the player is allowed to jump in a row
    
    // Vector values for managing player physics and movement
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
        iWeaponAmmoOrig = iWeaponAmmo;

        GameManager._instance.updateAmmoCount(); // update ui with ammo count info
        GameManager._instance.defuseLabel.SetActive(false); // small fix to make sure defuse prompt is inactive when player spawns
        GameManager._instance.updateGrenadeCount(); // update UI with grenade count info
    }

    // Called every frame
    void Update()
    {
        if (!GameManager._instance.isPaused) // if paused - disable player actions
        {
            // handles pushback physics
            vPushBack = Vector3.Lerp(vPushBack, Vector3.zero, Time.deltaTime * iPushBackResolve);

            // Various functions and coroutines that run constantly for player
            MovePlayer();
            Sprint();
            StartCoroutine(Shoot());
            StartCoroutine(reload());
            StartCoroutine(playFootsteps());
            ThrowGrenade();
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
            aud.PlayOneShot(aPlayerJump[Random.Range(0, aPlayerJump.Length)], aPlayerJumpVol);

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

    IEnumerator playFootsteps()
    {
        // if player is moving, on solid ground, and footsteps audio not already playing, play footsteps audio
        if (_controller.isGrounded && _move.normalized.magnitude > 0.4f && !bFootstepsPlaying)
        {
            bFootstepsPlaying = true;

            aud.PlayOneShot(aPlayerFootsteps[Random.Range(0, aPlayerFootsteps.Length)], aPlayerFootstepsVol); // call to footsteps audio clip

            // if player sprint function is active, play footstep sound clip faster to emulate players faster footfalls
            if (isSprinting)
            {
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                yield return new WaitForSeconds(0.4f);
            }

            bFootstepsPlaying = false;
        }
    }

    IEnumerator reload()
    {
        // if reload button is pressed, and player has ammo is their total ammo pool, start reload function
        if (Input.GetButtonDown("Reload") && iTotalWeaponAmmo > 0)
        {
            int shotsFired = iWeaponAmmoOrig - iWeaponAmmo; // determine how many shots were fired from the clip

            
            if (iTotalWeaponAmmo >= shotsFired)
            {
                // has more or the same amount of ammo then what the clip can hold
                iTotalWeaponAmmo -= shotsFired; // take ammo needed to refill the clip from the total amount of ammo

                iWeaponAmmo = iWeaponAmmoOrig; // reload clip
            }
            else
            {
                // has less then the max slip allowed
                iWeaponAmmo += iTotalWeaponAmmo; // take remaining ammo from the total
                iTotalWeaponAmmo = 0; // set total to 0 since we just took the reamining ammo to fill the clip
            }

            // Call to reload audio clip
            aud.PlayOneShot(aPlayerReload[Random.Range(0, aPlayerReload.Length)], aPlayerReloadVol);
        }

        yield return new WaitForSeconds(2.0f);
        GameManager._instance.updateAmmoCount();
    }

    IEnumerator Shoot()
    {
        // if shoot button is pressed, but there is no ammo in clip, play sound file
        if (Input.GetButton("Shoot") && iWeaponAmmo <= 0)
        {
            // call to empty clip audio clip
            aud.PlayOneShot(aPlayerEmptyClip[Random.Range(0, aPlayerEmptyClip.Length)], aPlayerEmptyClipVol);
        }

        // get input for shooting
        if (Input.GetButton("Shoot") && canShoot && iWeaponAmmo > 0)
        {
            iWeaponAmmo--;

            // turns shooting off so it cant be immediately executed again
            canShoot = false;

            // play audio clip
            aud.PlayOneShot(aGunShot[Random.Range(0, aGunShot.Length)], aGunShotVol);

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
                    if (hit.collider is SphereCollider) // apply damage for head shot, and play headshot audio clip
                    {
                        isDamageable.TakeDamage(10000);
                        aud.PlayOneShot(aHeadShot[Random.Range(0, aHeadShot.Length)], aHeadShotVol);
                        GameManager._instance.iScore += 15;
                        GameManager._instance.UpdatePlayerScore(); // call to helper function to update score on win/lose screens
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

            GameManager._instance.updateAmmoCount();
        }
    }

    public void ThrowGrenade()
    {
        if (Input.GetButtonDown("Grenade") && (iGrenadeCount <= 0 || !canShoot)) // if input button is pressed, player does not have grenade ammo to use OR canShoot is false
        {
            // play "no grenades left" audio clip
            aud.PlayOneShot(aGrenadeEmpty[Random.Range(0, aGrenadeEmpty.Length)], aGrenadeEmptyVol);
        }
        else if (Input.GetButtonDown("Grenade") && canShoot && iGrenadeCount > 0) // if input button "grenade" is pressed, player has grenade ammo, and canShoot is enabled
        {
            // decrement player grenade ammo
            iGrenadeCount--;

            // disable shooting until grenade throw is complete
            canShoot = false;

            // play grenade pullpin audio clip
            aud.PlayOneShot(aGrenade[Random.Range(0, aGrenade.Length)], aGrenadeVol);

            // instantiate grenade object
            Instantiate(gPlayerGrenade, transform.position, transform.rotation);

            // update grenade ammo UI component in gameManager
            GameManager._instance.updateGrenadeCount();

            // reenable shooting
            canShoot = true;
        }
    }

    public void TakeDamage(int iDmg)
    {
        iPlayerHealth -= iDmg; // apply damage to player health

        // play player damage taken audio clip
        aud.PlayOneShot(aPlayerHurt[Random.Range(0, aPlayerHurt.Length)], aPlayerHurtVol);

        UpdateHealthBar(); 
        StartCoroutine(DamageFlash()); // flash screen red when damage is taken

        // if playern health is 0 or lower, kill player
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

    IEnumerator PickUpFlash() // Helper function that creates flash upon taking damage
    {
        GameManager._instance._AmmoBoxFlash.SetActive(true);
        yield return new WaitForSeconds(30.0f);
        GameManager._instance._AmmoBoxFlash.SetActive(false);
    }

    public void Respawn()
    {
        iPlayerHealth = iPlayerHealthOrig; // reset player health
        _controller.enabled = false;
        transform.position = _playerSpawnPos; // reset player location
        _controller.enabled = true;
        vPushBack = Vector3.zero;

        UpdateHealthBar();

        GameManager._instance.defuseLabel.SetActive(false);
    }

    // helper function for picking up healthpack
    public void HealthPack()
    {
        iPlayerHealth += iHealthPickupHealNum;  // apply health pickup value to player healthbar
        healthPickUp(); //play health pack sound
        UpdateHealthBar(); //update health change in bar
    }

    // helper function for picking up ammo box
    public void AmmoBox()
    {
        iTotalWeaponAmmo += iAmmoPickupAmmoNum; // apply ammo pickup value to player total ammo pool
        GameManager._instance.updateAmmoCount(); //play ammo pickup sound
        AmmoPickUp(); //add ammo to player
    }

    public void UpdateHealthBar() // update the healthbar in the UI to reflect player's current health
    {
        GameManager._instance._HpBar.fillAmount = (float)iPlayerHealth / (float)iPlayerHealthOrig;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("HealthPack") && (iPlayerHealth+iHealthPickupHealNum) < iPlayerHealthOrig) //Checks to see if health amount is less than original health and health pack amount
        {
            HealthPack(); //calls health pack function
            Destroy(other.gameObject); //destroys health pack
        }
        else if (other.CompareTag("HealthPack") && iPlayerHealth<iPlayerHealthOrig && (iPlayerHealth + iHealthPickupHealNum) >= iPlayerHealthOrig) //Checks if health amount is less than original and greater than heal amount
        {
            iPlayerHealth=iPlayerHealthOrig; //calls health pack function
            UpdateHealthBar();
            Destroy(other.gameObject); //destroys health pack
        }
        else if (other.CompareTag("HealthPack") && iPlayerHealth == iPlayerHealthOrig) //if health is full, do nothing
        {

        }

        if(other.CompareTag("AmmoBox") && iWeaponAmmo < iTotalWeaponAmmo) // Calls Ammo Amount
        {
            AmmoBox(); //calls ammo function
            Destroy(other.gameObject); //destroys ammo box
        }
        else if (other.CompareTag("AmmoBox")&& iWeaponAmmo==iTotalWeaponAmmo)
        {

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

    public void healthPickUp()
    {
        // audio clip for health pickup
        aud.PlayOneShot(aHeal[Random.Range(0, aHeal.Length)], aHealVol);
    }

    public void AmmoPickUp()
    {
        // audio clip for ammo pickup
        aud.PlayOneShot(aAmmo[Random.Range(0, aAmmo.Length)], aAmmoVol);
    }

}
