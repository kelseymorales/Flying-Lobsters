using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject moveUi;
    [SerializeField] private GameObject dogeUi;
    [SerializeField] private GameObject sprintUi;
    [SerializeField] private GameObject jumpUi;
    [SerializeField] private GameObject AttackUi;
    [SerializeField] private GameObject reloadUi;
    [SerializeField] private GameObject goalUi;
    [SerializeField] private GameObject gunUi;

    private bool[] checks = new bool[4];


    private void Update() 
    {
        if (moveUi.activeSelf)
        {
            if (Input.GetButtonDown("W"))
            {
                checks[0] = true;
            }
            else if (Input.GetButtonDown("S"))
            {
                checks[1] = true;
            }
            else if (Input.GetButtonDown("A"))
            {
                checks[2] = true;
            }
            else if (Input.GetButtonDown("D"))
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
        }
        
        if (dogeUi.activeSelf)
        {
            if (Input.GetButtonDown("Dodge"))
            {
                dogeUi.SetActive(false);
                sprintUi.SetActive(true);

                checks = new bool[2];
            }
        }

        if (sprintUi.activeSelf)
        {
            if (Input.GetButtonDown("Sprint"))
            {
                sprintUi.SetActive(false);
                jumpUi.SetActive(true);
            }
        }

        if (jumpUi.activeSelf)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumpUi.SetActive(false);
                AttackUi.SetActive(true);

                checks = new bool[2];
            }
        }

        if (AttackUi.activeSelf)
        {
            if (Input.GetButtonDown("Shoot"))
            {
                checks[0] = true;
            }

            if (Input.GetButtonDown("Grenade"))
            {
                checks[1] = true;
            }

            for (int i = 0; i < checks.Length; i++)
            {
                if (checks[i] == false)
                {
                    return;
                }
            }

            AttackUi.SetActive(false);
            reloadUi.SetActive(true);
        }

        if (reloadUi.activeSelf)
        {
            if (Input.GetButtonDown("Reload"))
            {
                reloadUi.SetActive(false);
                StartCoroutine(GoalGame());
            }
        }
    }

    IEnumerator GoalGame()
    {
        goalUi.SetActive(true);

        yield return new WaitForSeconds(8f);

        goalUi.SetActive(false);

        StartCoroutine(Guns());
    }

    IEnumerator Guns()
    {
        gunUi.SetActive(true);

        yield return new WaitForSeconds(5f);

        gunUi.SetActive(false);
    }
}
