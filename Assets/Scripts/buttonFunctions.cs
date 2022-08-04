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
        GameManager._instance.levelWin = false;
        GameManager._instance.iScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload scene
        GameManager._instance.Restart();
    }

    #region Menus

    public void OpenInfo() //Activates the Info tab
    {
        GameManager._instance._infoMenu.SetActive(true); 
    }

    public void OpenOptions() //Activates the options tab
    {
        GameManager._instance._optionsMenu.SetActive(true);
    }

    public void StartGame() //to implement.. 
    {

    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    #endregion
}
