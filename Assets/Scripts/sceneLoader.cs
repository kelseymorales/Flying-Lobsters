using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    [SerializedField] public int iSceneIndex;
    void OnEnable()
    {
        SceneManager.LoadScene(iSceneIndex, LoadSceneMode.Single);
    }

}
