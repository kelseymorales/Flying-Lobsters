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

    [SerializeField] public AudioClip[] aGunShot;
    [Range(0.0f, 1.0f)][SerializeField] public float aGunShotVol;

    public RuntimeAnimatorController _anim;
}
