using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gunStat; // stores scriptable object containing relevant stats for individual gun

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // check if player collided with pickup
        {
            // apply gunStats from pickup to player using player's gunPickup function
            GameManager._instance._playerScript.gunPickup(gunStat.fFireRate, gunStat.iDamage, gunStat.gGunModel, gunStat.iClipSize, gunStat.fGunRange, gunStat.aGunShot, gunStat.aGunShotVol, gunStat._anim);

            // update player bools on weapon type
            if (gunStat.isSniper == true)
            {
                GameManager._instance._playerScript.sniperGun = true;
                GameManager._instance._playerScript.ShotgunGun = false;
            }
            else if (gunStat.isShotgun == true)
            {
                GameManager._instance._playerScript.ShotgunGun = true;
                GameManager._instance._playerScript.sniperGun = false;
            }
            else
            {
                GameManager._instance._playerScript.sniperGun = false;
                GameManager._instance._playerScript.ShotgunGun = false;
            }

        }
    }
}
