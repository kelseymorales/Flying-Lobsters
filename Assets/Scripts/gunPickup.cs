using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gunStat; // stores scriptable object containing relevant stats for individual gun

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager._instance._playerScript.gunPickup(gunStat.fFireRate, gunStat.iDamage, gunStat.gGunModel, gunStat.iClipSize, gunStat.fGunRange, gunStat.aGunShot, gunStat.aGunShotVol, gunStat._anim);
        }
    }
}
