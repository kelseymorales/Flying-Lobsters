using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{

    [SerializedField] public bool isSpeedPowerUp;
    [SerializedField] public bool isDamagePowerUp;
    [SerializedField] public bool isShieldPowerUp;
    [SerializedField] public bool isAmmoPowerUp;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // check if player collided with powerup
        {
            if (isSpeedPowerUp)
                GameManager._instance._playerScript.hasSpeedBoost = true;
            if (isDamagePowerUp)
                GameManager._instance._playerScript.hasDamageBoost = true;
            if (isShieldPowerUp)
                GameManager._instance._playerScript.isShielded = true;
            if (isAmmoPowerUp)
                GameManager._instance._playerScript.hasUnlimetedAmmo = true;


            GameManager._instance._playerScript.powerUpPickup();

            Destroy(gameObject); 
        }
    }
}
