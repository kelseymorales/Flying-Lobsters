using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += DestroyMusic;
    }

    void DestroyMusic(Scene scene, LoadSceneMode mode)
    {
        if(scene == null)
        {
            Destroy(gameObject);
        }
    }
}
