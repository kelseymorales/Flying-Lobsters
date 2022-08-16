using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Image _loadingBar;
    [SerializedField] public int iSceneIndex;

    void Start()
    {
        
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(iSceneIndex);

        while(gameLevel.progress < 1)
        {
            _loadingBar.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
        
    }

    
}
