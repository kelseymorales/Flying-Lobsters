using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
   [SerializeField] private GameObject[] enemyComponents; 

   private void Update() 
   {
        enemyComponents[1].GetComponents<Component>();
   }
}
