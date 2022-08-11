using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripWireDefuser : bombGoal
{
    //Added for the TripWireDefuser class
    [SerializedField] public GameObject trap;

    public override void Start()
    {
        
    }
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    // helper function for when player moves out of range of a bomb
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
        GameManager._instance.isDefusingTrap = true;
        base.Defuse();
    }
    public override void SetDefusedState()
    {
        Debug.Log("Destroy object called"); 
        Destroy(trap); 
    }

    public void OnDestroy()
    {
        GameManager._instance.defuseLabel.SetActive(false);
    }
}
