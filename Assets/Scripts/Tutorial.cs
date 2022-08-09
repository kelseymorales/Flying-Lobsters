using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializedField] private GameObject moveUi;
    [SerializedField] private GameObject dogeUi;
    [SerializedField] private GameObject AttackUi;
    [SerializedField] private GameObject reloadUi;

    private bool[] checks = new bool[4];


    private void Update() 
    {
        if (moveUi.activeSelf)
        {
            if (Input.GetButtonDown("W"))
            {
                checks[0] = true;
            }
            if (Input.GetButtonDown("S"))
            {
                checks[1] = true;
            }

            if (Input.GetButtonDown("A"))
            {
                checks[2] = true;
            }

            if (Input.GetButtonDown("D"))
            {
                checks[3] = true;
            }

            for (int i = 0; i < checks.Length; i++)
            {
                if (checks[i] == false)
                {
                    return;
                }
            }

            moveUi.SetActive(false);
            dogeUi.SetActive(true);

            checks = new bool[1];
        }

        if (dogeUi.activeSelf)
        {
            if (Input.GetButtonDown("Doge"))
            {
                dogeUi.SetActive(false);
                AttackUi.SetActive(true);

                checks = new bool[2];
            }
        }

        if (AttackUi.activeSelf)
        {
            if (Input.GetButtonDown("Shoot"))
            {
                
            }
        }
    }
}
