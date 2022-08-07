using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripWireDefuser : bombGoal
{
    

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
        GameManager._instance.isDefusingTrap = true; 
        base.Update();
    }

    public override void Defuse()
    {
        base.Defuse();
    }
}
