using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public float fwaitTime;

    public Image _loadingBar;

    void Start()
    {
        Time.timeScale = 0;
        //GameManager._instance.gameOver = true;
        //GameManager._instance.isPaused = true;
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        for (float i = 0; i <= fwaitTime; i += 0.25f)
        {
            yield return new WaitForSecondsRealtime(0.25f);

            _loadingBar.fillAmount = i / fwaitTime;
        }

        SceneManager.LoadScene(1);

        Time.timeScale = 1;
        //GameManager._instance.gameOver = false;
        //GameManager._instance.isPaused = false;
        Destroy(gameObject);
    }
}
