using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _credits;


    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayTurtorial()
    {
        SceneManager.LoadScene(12);
    }

    public void OptionsMenu()
    {
        _mainMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }

    public void ShowCredits()
    {
        _mainMenu.SetActive(false);
        _credits.SetActive(true);
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

    public void BackToMainFromCredits()
    {
        _mainMenu.SetActive(true);
        _credits.SetActive(false);
    }
}
