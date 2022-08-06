using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public float fFireRate;
    public int iDamage;
    public GameObject gGunModel;

    public int iClipSize;
    public float fGunRange;
}
