using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;


    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OptionsMenu()
    {
        _mainMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMainFromOptions()
    {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
    }
}
