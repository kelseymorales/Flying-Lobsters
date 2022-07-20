using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [Header("Player Referance\n------------------------------")]
    public GameObject _player;
    public playerController _playerScript;
    public Spawner[] _spawners;

    [Header("UI\n------------------------------")]
    public bool isPaused = false;
    public GameObject _pauseMenu;
    public GameObject _playerDeadMenu;
    public Image _HpBar;

    [Header("Effects\n------------------------------")]
    public GameObject _playerDamageFlash;

    void Awake()
    {
        _instance = this;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerScript = _player.GetComponent<playerController>();

        GameObject[] s = GameObject.FindGameObjectsWithTag("Spawner");

        if (s != null)
        {
            _spawners = new Spawner[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                _spawners[i] = s[i].GetComponent<Spawner>();    
            }
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !_playerDeadMenu.activeSelf)
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauseMenu.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        isPaused = false;
        _pauseMenu.SetActive(false);
        _playerDeadMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PlayerDead()
    {
        isPaused = true;
        _playerDeadMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void StopSpawners()
    {
        if (_spawners != null)
        {
            for (int i = 0; i < _spawners.Length; i++)
            {
                _spawners[i].StopSpawning();
            }
        }
    }
}
