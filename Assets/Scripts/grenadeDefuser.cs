using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadeDefuser : bombGoal
{
    [SerializedField] public GameObject _grenade;
    

    public override void Start()
    {
        
    }

    public override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            base.inRange = true;
            GameManager._instance.grenadeDefuseLabel.SetActive(true);
            GameManager._instance.isDefusingGrenade = true;
        }
              
    }

    // helper function for when player moves out of range of a grenade
    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            base.inRange = false;
            GameManager._instance.grenadeDefuseLabel.SetActive(false);
        }
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
        Destroy(gameObject);
    }

}
