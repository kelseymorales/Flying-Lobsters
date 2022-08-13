using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Image _loadingBar;

    void Start()
    {
        
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        //temp loading showcase level until all levels are done
        //while implement new code for when loading different scenes
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(1);

        while(gameLevel.progress < 1)
        {
            _loadingBar.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
        
    }

    
}
