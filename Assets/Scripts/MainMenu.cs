using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _credits;

    [SerializeField] private OptionsMenu optionsRef;


    private void Start() 
    {
        optionsRef.Start();    
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(5);
    }

    public void PlayTurtorial()
    {
        SceneManager.LoadScene(15);
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
