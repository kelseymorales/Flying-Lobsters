using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{
    [SerializedField] public float fPowerUpDuration; 

    //Booleans to check power-up type
    [SerializedField] public bool isSpeedPowerUp;
    [SerializedField] public bool isDamagePowerUp;
    [SerializedField] public bool isShieldPowerUp;
    [SerializedField] public bool isAmmoPowerUp;

    private string currentPowerUp; //String to check power-up type

    [SerializedField] public int speedBoostValue; //Value to increase speed

    public Image _currentPowerUpImage; //Current icon for each power-up

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // check if player collided with powerup
        {
             PickUp();
        }
    }

    private void PickUp()
    {
        //Each if statement takes care of setting a boolean to true representing if the power up is
        //active or not. These booleans are located in the playerController. And also sets the current
        //icon to the specific icon for each power up
        //Sets the current powerUp text 
        if (isSpeedPowerUp)
        {
            GameManager._instance._playerScript.hasSpeedBoost = true;
            _currentPowerUpImage = GameManager._instance._powerUpSpeed;
            GameManager._instance._playerScript.SetSpeedStat(speedBoostValue); //Calls a method to change the speed of the player
            currentPowerUp = "speed";

        }
            
        if (isDamagePowerUp)
        {
            GameManager._instance._playerScript.hasDamageBoost = true;
            _currentPowerUpImage = GameManager._instance._powerUpDamage;
            currentPowerUp = "damage";
        }
            
        if (isShieldPowerUp)
        {
            GameManager._instance._playerScript.isShielded = true;
            _currentPowerUpImage = GameManager._instance._powerUpShield;
            currentPowerUp = "shield"; 

        }
            
        if (isAmmoPowerUp)
        {
            GameManager._instance._playerScript.hasUnlimetedAmmo = true;
            _currentPowerUpImage = GameManager._instance._powerUpAmmo;

            currentPowerUp = "ammo"; 

        }
            


        
        StartCoroutine(PickUpTimer()); //Starts individual timers for each power up

        
    }

    private IEnumerator PickUpTimer()
    {
        
        gameObject.GetComponent<Renderer>().enabled = false; //Disables renderer for the power up
        gameObject.GetComponent<Collider>().enabled = false; //Disables collider so the player doesn't activate it again while invisible 

        _currentPowerUpImage.gameObject.SetActive(true); //Activates the icon so the player knows what powerup is active

        yield return new WaitForSeconds(fPowerUpDuration); //Waits for set powerup duration

        _currentPowerUpImage.gameObject.SetActive(false); //Deactivates the icon so the player knows that the powerups is over

        SetActivesFalse(); 

        Destroy(gameObject); //Destroys powerUp
        Destroy(transform.parent.gameObject); //Destroys powerUp parent

    }



    private void SetActivesFalse()
    {
        //Checks for power-up type and deactivetes bools
        
        if(currentPowerUp == "speed")
        {
            GameManager._instance._playerScript.hasSpeedBoost = false;
            GameManager._instance._playerScript.SetBackSpeedStats(); //Calls for PowerUpDeactive in playerController
        }
        else if(currentPowerUp == "damage")
        {
            GameManager._instance._playerScript.hasDamageBoost = false;
        }
        else if(currentPowerUp == "shield")
        {
            GameManager._instance._playerScript.isShielded = false;
        }
        else if(currentPowerUp == "ammo")
        {
            GameManager._instance._playerScript.hasUnlimetedAmmo = false;
        }
        
    }
}
