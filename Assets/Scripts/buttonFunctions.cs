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

    public void CloseOptions()
    {
        if (!GameManager._instance.isPaused)
        {
            GameManager._instance._optionsMenu.SetActive(false);
        }

        else
            GameManager._instance.CloseOptionsInGame();
    }

    public void OpenOptionsInGame() 
    {
        GameManager._instance.OpenOptionsInGame();
         

    }
    public void OpenControlWindow()
    {
        GameManager._instance.OpenControlWindow();
    }

    public void CloseControlWindow()
    {
        GameManager._instance.CloseControlWindow();
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
