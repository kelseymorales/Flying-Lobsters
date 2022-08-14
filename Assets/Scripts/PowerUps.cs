using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{

    [SerializedField] public bool isSpeedPowerUp;
    [SerializedField] public bool isDamagePowerUp;
    [SerializedField] public bool isShieldPowerUp;
    [SerializedField] public bool isAmmoPowerUp;

    public Image _currentPowerUpImage; 

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // check if player collided with powerup
        {
             PickUp();
        }
    }

    private void PickUp()
    {
        if (isSpeedPowerUp)
        {
            GameManager._instance._playerScript.hasSpeedBoost = true;
            _currentPowerUpImage = GameManager._instance._powerUpSpeed; 
        }
            
        if (isDamagePowerUp)
        {
            GameManager._instance._playerScript.hasDamageBoost = true;
            _currentPowerUpImage = GameManager._instance._powerUpDamage;
        }
            
        if (isShieldPowerUp)
        {
            GameManager._instance._playerScript.isShielded = true;
            _currentPowerUpImage = GameManager._instance._powerUpShield;
        }
            
        if (isAmmoPowerUp)
        {
            GameManager._instance._playerScript.hasUnlimetedAmmo = true;
            _currentPowerUpImage = GameManager._instance._powerUpAmmo;
        }
            


        //GameManager._instance._playerScript.powerUpPickup();
        StartCoroutine(PickUpTimer());

        
    }

    private IEnumerator PickUpTimer()
    {
        
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false; 

        _currentPowerUpImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);

        _currentPowerUpImage.gameObject.SetActive(false);
        
        Destroy(gameObject);

    }
}
