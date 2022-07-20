using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [Header("Player Referance\n------------------------------")]
    public GameObject _player;
    public playerController _playerScript;
    public Spawner[] _spawners;

    [Header("UI\n------------------------------")]
    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public bool gameOver;
    public GameObject _pauseMenu;
    public GameObject _playerDeadMenu;
    public Image _HpBar;

    public GameObject _winGameMenu;
    public GameObject _loseGameMenu;
    public TMP_Text tEnemiesDead;
    public TMP_Text tEnemyTotal;
    public TMP_Text tBombTotal;
    public TMP_Text tBombsDefused;
    public TMP_Text tBombsTimer;
    public TMP_Text tDefuseCountdown;

    [Header("Effects\n------------------------------")]
    public GameObject _playerDamageFlash;

    [Header("Win Condition\n------------------------------")]
    [SerializeField] GameObject _detonation;
    [SerializeField] public int iBombTimer;
    [SerializeField] public int iDefuseCountdownTime;
    [SerializeField] public GameObject _defuseCountdownObject;

    [Header("Text Prompts\n------------------------------")]
    [SerializeField] public GameObject defuseLabel;

    [HideInInspector] public int iEnemyKillGoal;
    [HideInInspector] int iEnemiesKilled;
    [HideInInspector] public int iBombsActive;
    [HideInInspector] public int iBombsDefusedCounter;
    [HideInInspector] public int iBombTotalOrig;

    [HideInInspector] GameObject _menuCurrentlyOpen;

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

        iBombTotalOrig = iBombsActive;
        tBombTotal.text = iBombTotalOrig.ToString("F0");
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
                _menuCurrentlyOpen = _pauseMenu;
                _menuCurrentlyOpen.SetActive(true);
                LockCursorPause();
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
        _menuCurrentlyOpen.SetActive(false);
        _menuCurrentlyOpen = null;
        UnlockCursorUnpause();
    }

    public void PlayerDead()
    {
        gameOver = true;
        _menuCurrentlyOpen = _playerDeadMenu;
        _menuCurrentlyOpen.SetActive(true);
        LockCursorPause();

        _playerScript.loseJingle();
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

    public void CheckEnemyKills()
    {
        iEnemiesKilled++;
        tEnemiesDead.text = iEnemiesKilled.ToString("F0");
    }

    public IEnumerator Detonate()
    {
        // explosion effect
        _detonation.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        // you lose menu
        gameOver = true;
        _menuCurrentlyOpen = _loseGameMenu;
        _menuCurrentlyOpen.SetActive(true);
        LockCursorPause();

        _playerScript.loseJingle();
    }

    public void LockCursorPause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void UnlockCursorUnpause()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart()
    {
        gameOver = false;
        _menuCurrentlyOpen.SetActive(false);
        _menuCurrentlyOpen = null;
        UnlockCursorUnpause();

        _defuseCountdownObject.SetActive(false);
    }

    public void updateEnemyCount()
    {
        iEnemyKillGoal++;
        tEnemyTotal.text = iEnemyKillGoal.ToString("F0");
    }

    public void updateBombCount()
    {
        iBombsActive++;
        tBombTotal.text = iBombsActive.ToString("F0");
    }

    public void WinGame()
    {
        _menuCurrentlyOpen = _winGameMenu;
        _menuCurrentlyOpen.SetActive(true);
        gameOver = true;
        LockCursorPause();

        _playerScript.winJingle();

    }
}
