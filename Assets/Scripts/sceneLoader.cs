using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    [SerializedField] public int iSceneIndex;

    void OnEnable()
    {
        //Load based on sceneName field
        SceneManager.LoadScene(iSceneIndex, LoadSceneMode.Single);

    }

}
