using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenuActivate : MonoBehaviour
{
    [SerializeField] GameObject gFirstSelected;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gFirstSelected);
    }
}
