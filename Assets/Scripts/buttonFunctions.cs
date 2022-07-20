using UnityEngine;

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
}
