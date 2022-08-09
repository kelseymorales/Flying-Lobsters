using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadeDefuser : bombGoal
{
    [SerializedField] public GameObject _grenade;
    [SerializedField] public EnemyAI _enemyScript;

    public override void Start()
    {
        _grenade = _enemyScript.gBullet;
        
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);        
    }

    // helper function for when player moves out of range of a grenade
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
    public override void Update()
    {

        base.Update();
    }

    public override void Defuse()
    {
        
        GameManager._instance.isDefusingGrenade = true;
        base.Defuse();
    }

    public override void SetDefusedState()
    {
        
        GameManager._instance.isGrenadeDefused = true;
        
        GameManager._instance._playerScript.iGrenadeCount++;
        GameManager._instance.updateGrenadeCount();
    }

}
