using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripWireDefuser : bombGoal
{
    public override void SetDefusedState()
    {
        Destroy(transform.parent.gameObject); 
        GameManager._instance.defuseLabel.SetActive(false);
    }
}