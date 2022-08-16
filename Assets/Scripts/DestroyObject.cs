using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public GameObject obj;

    private void Update()
    {
        DestroyObj();
    }

    public void DestroyObj()
    {
        if (GameManager._instance.iEnemiesKilled == GameManager._instance.iEnemyKillGoal)
        {
            Destroy(obj);
        }
    }
}
