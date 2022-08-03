using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void Resume() //calling resume from game manager for menu UI
    {
        GameManager._instance.Resume();
    }

    public void Quit() //calling quit from game manager for menu UI
    {
        Application.Quit();
    }

    public void Respawn() //calling respawn from playerscript and resume from game manager for menu UI
    {
        GameManager._instance._playerScript.Respawn();
        GameManager._instance.Resume();
    }

    public void Restart() //calling restart from gamemanger and reloading scene for menu UI
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload scene
        GameManager._instance.Restart();
    }
}
