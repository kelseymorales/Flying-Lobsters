using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    //only loads temp loading screen
    //change title to correct scene want to load to
    void OnEnable()
    {
        SceneManager.LoadScene("Temp Loading Scene", LoadSceneMode.Single);
    }

}
