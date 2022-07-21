using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void Resume()
    {
        GameManager._instance.Resume();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Respawn()
    {
        GameManager._instance._playerScript.Respawn();
        GameManager._instance.Resume();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload scene
        GameManager._instance.Restart();
    }
}
