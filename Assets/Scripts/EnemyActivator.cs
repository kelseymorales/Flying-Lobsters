using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
   [SerializeField] private float range;

   [SerializeField] private GameObject main;
   [SerializeField] private GameObject[] enemyComponents;

   [SerializeField] private bool isActive = false; 

   private void Start() 
   {
      CheckForPlayer();
   }

   private void Update() 
   {
      CheckForPlayer();
   }

   private void CheckForPlayer()
   {
      float distance = Vector3.Distance(transform.position, GameManager._instance._player.transform.position);

      if (distance <= range && isActive == false)
      {
         ActivateEnemy();
      }
      else if(distance > range && isActive)
      {
         DeactivateEnemy();
      }
   }

   private void ActivateEnemy()
   {
      Behaviour[] mainComps = main.GetComponents<Behaviour>();

      foreach (Behaviour comp in mainComps)
      {
         comp.enabled = true;
      }

      foreach (GameObject item in enemyComponents)
      {
         item.SetActive(true);
      }

      isActive = true;

      if (main.GetComponent<EnemyAI>().GetIsDead())
      {
         main.GetComponent<EnemyAI>().DeathState();
      }
   }

   private void DeactivateEnemy()
   {
      Behaviour[] mainComps = main.GetComponents<Behaviour>();

      foreach (Behaviour comp in mainComps)
      {
         comp.enabled = false;
      }

      foreach (GameObject item in enemyComponents)
      {
         item.SetActive(false);
      }

      isActive = false;
   }
}
