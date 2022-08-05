using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;                    // public variable referencing this (gameManager), used to communicate to gameManager from other scripts

    [Header("Player Referance\n------------------------------")]
    public GameObject _player;                              // stores reference to the player game object
    public playerController _playerScript;                  // stores reference to the playerController script

    public Spawner[] _spawners;                             // stores references to all spawners in current scene

    [Header("UI\n------------------------------")]
    [HideInInspector] public bool isPaused = false;         // value storing whether game is paused or not
    [HideInInspector] public bool gameOver;                 // value storing whether a gameOver condition has been reached
    [HideInInspector] public bool levelWin = false;         // Stores whether the current level has been won - primarily for determining if player is allowed through level transition into next level

    //Main menus
    public GameObject _optionsMenu;
    public GameObject _infoMenu;

    // game menus
    public GameObject _pauseMenu;
    public GameObject _playerDeadMenu;
    public GameObject _winGameMenu;
    public GameObject _loseGameMenu;

    // UI - healthbar
    public Image _HpBar;

    // UI - enemies dead and enemies spawned
    public TMP_Text tEnemiesDead;
    public TMP_Text tEnemyTotal;

    // UI - bombs defused, bombs spawned total, bomb countdown timer, bomb defuse countdown
    public TMP_Text tBombTotal;
    public TMP_Text tBombsDefused;
    public TMP_Text tBombsTimer;
    public TMP_Text tDefuseCountdown;

    // UI - player ammo total, ammo clip, and current shots left in clip
    public TMP_Text ammoTotal;
    public TMP_Text clipSize;
    public TMP_Text shotsInClip;

    [Header("Effects\n------------------------------")]
    public GameObject _playerDamageFlash;                       // screenspace effect for player taking damage
    public GameObject _AmmoBoxFlash;                            // effect for activating an ammo pickup

    [Header("Win Condition\n------------------------------")]
    [SerializeField] GameObject _detonation;                    // stores a reference to the detonation object placed, disabled, in the scene
    [SerializeField] public int iBombTimer;                     // value for bomb timer countdown
    [SerializeField] public int iDefuseCountdownTime;           // value for defuse countdown timer
    [SerializeField] public GameObject _defuseCountdownObject;  // stores reference to UI - bomb defuse countdown
    [SerializeField] public GameObject _defuseSlider;           // stores reference to UI - bomb defuse countdown slider
    public Image _defuseSliderImage;                            // stores reference to UI - bomb defuse countdown slider fill
    [HideInInspector] public int iScore;                        // stores player score - updated in checkEnemiesKilled, PlayerController shoot, and bombGoal defuse
    [SerializeField] TMP_Text displayWinScore;                  // UI component that displays score on win screen
    [SerializeField] TMP_Text displayLoseScore;                  // UI component that displays score on lose screen

    [Header("Text Prompts\n------------------------------")]
    [SerializeField] public GameObject defuseLabel;             // reference to prompt shown to defuse bombs

    // stores variables keeping track of enemies killed, enemies spawned, bombs defused, bombs active, and original bomb total
    [HideInInspector] public int iEnemyKillGoal;
    [HideInInspector] int iEnemiesKilled;
    [HideInInspector] public int iBombsActive;
    [HideInInspector] public int iBombsDefusedCounter;
    [HideInInspector] public int iBombTotalOrig;

    [HideInInspector] GameObject _menuCurrentlyOpen; 
    
    Coroutine _defuseFunction;          // stores reference to any game menu currently open/active

    void Awake()
    {
        _instance = this;                                               // stores reference this for use in other scripts
        _player = GameObject.FindGameObjectWithTag("Player");           // stores reference to player game object
        _playerScript = _player.GetComponent<playerController>();       // stores reference to player script

        iBombTotalOrig = iBombsActive;                                  // stores total bombs placed in the scene at start of scene
        tBombTotal.text = iBombTotalOrig.ToString("F0");          // stores bomb total as text for use in UI

        iScore = 0;                                                     // sets iScore value to zero at start of game

        GameObject[] s = GameObject.FindGameObjectsWithTag("Spawner");  // finds and references all spawners placed in scene

        if (s != null)
        {
            _spawners = new Spawner[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                _spawners[i] = s[i].GetComponent<Spawner>();
            }
        }

        StartCoroutine(bombTick());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !gameOver) // if actively playing the game (no game over), and Cancel button is pushed, pause game and show pause menu
        {
            if (!isPaused && !_menuCurrentlyOpen)
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

    public void Resume()    // unpause game and hide pause menu
    {
        isPaused = false;
        gameOver = false;
        _menuCurrentlyOpen.SetActive(false);
        _menuCurrentlyOpen = null;
        UnlockCursorUnpause();

    }

    public void PlayerDead()
    {
        gameOver = true;
        isPaused = true;
        _menuCurrentlyOpen = _playerDeadMenu;
        _menuCurrentlyOpen.SetActive(true);
        LockCursorPause();

        _playerScript.loseJingle();
        StopSpawners();
        StopDefuseing();
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
        iScore += iEnemiesKilled * 10; // score tracking for enemy kills - also bonus points are allocated for headshot kills in playerController shoot function
        UpdatePlayerScore(); // call to helper function to update score on win/lose screens
    }

    public void CallDefuse(GameObject bomb)
    {
        _defuseFunction = StartCoroutine(Defuse(bomb));
    }

    IEnumerator Defuse(GameObject bomb)
    {
        tDefuseCountdown.text = ""; // clear previous defuse text if needed
        _defuseSliderImage.fillAmount = GameManager._instance.iDefuseCountdownTime; // make sure slider bar is full before putting it onscreen

        _playerScript.LockInPlace(); // lock player position, but allow camera control and shooting

        // activate UI elements showing defusing in process
        _defuseCountdownObject.SetActive(true);
        _defuseSlider.SetActive(true);

        // defusal countdown
        for (int i = iDefuseCountdownTime; i >= 0; i--)
        {
            // UI updates for defuse countdown
            _defuseSliderImage.fillAmount = (float)i / (float)GameManager._instance.iDefuseCountdownTime;
            tDefuseCountdown.text = i.ToString("F0");
         
            yield return new WaitForSeconds(1);
        }

        // update game goals 
        iBombsActive--;
        iBombsDefusedCounter++;
        iScore += 50;
        UpdatePlayerScore(); // call to helper function to update score on win/lose screens

        //deactivate UI elements showing defusing in process
        _defuseCountdownObject.SetActive(false);
        _defuseSlider.SetActive(false);

        _playerScript.UnlockInPlace(); // unlock player position

        defuseLabel.SetActive(false); // make sure the prompt to defuse bombs deactivates now that bomb is defused

        tBombsDefused.text = iBombsDefusedCounter.ToString("F0"); // update bombs defused UI element

        _playerScript.defuseJingle(); // play defuse audio jingle

        Destroy(bomb); // destroy bomb object (may be better to add a particle effect or something instead, rather than bomb just disapearing)

        StopSpawners();

        // Level Win Condition
        if (iBombsActive == 0)
        {
            levelWin = true;
            WinGame(); // will need to be removed later, as the win game should be called at the end of level 3, and all we need here is the level win trigger for transitioning levels
        }
    }

    private void StopDefuseing()
    {
        if (_defuseFunction == null)
        {
            return;
        }

        StopCoroutine(_defuseFunction);

        // activate UI elements showing defusing in process
        _defuseCountdownObject.SetActive(false);
        _defuseSlider.SetActive(false);
    }

    IEnumerator bombTick()
    {
        // countdown bomb timer, and update it on UI
        for (int i = iBombTimer; i >= 0; i--)
        {
            tBombsTimer.text = i.ToString("F0");
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(.5f);

        // once countdown reaches 0, exit forloop and detonate bomb
        StartCoroutine(Detonate());
    }

    // function used to detonate bombs after bomb countdown reaches 0, this is a lose game scenario
    public IEnumerator Detonate()
    {
        // explosion effect
        _detonation.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        for (int i = 0; i < iBombsActive; i++)
        {
            // play 'You Lose' audio clip
            _playerScript.loseJingle();
        }

        // you lose menu
        gameOver = true;
        _menuCurrentlyOpen = _loseGameMenu;
        _menuCurrentlyOpen.SetActive(true);
        LockCursorPause();

    }

    public void LockCursorPause()   // helper function for menu navigation - locks cursor 
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void UnlockCursorUnpause()   // helper function for menu navigation - unlocks cursor
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void updateEnemyCount()  // helper function for updating UI with current enemy count (killed and total)
    {
        iEnemyKillGoal++;
        tEnemyTotal.text = iEnemyKillGoal.ToString("F0");
    }

    public void updateBombCount()   // helper function for updating UI with current bomb count
    {
        iBombsActive++;
        tBombTotal.text = iBombsActive.ToString("F0");
    }

    public void updateAmmoCount()   // helper function for updating UI with current ammo count
    {
        ammoTotal.text = _playerScript.iTotalWeaponAmmo.ToString("F0"); // ammo pool total
        shotsInClip.text = _playerScript.iWeaponAmmo.ToString("F0"); // current Clip
        clipSize.text = _playerScript.iWeaponAmmoOrig.ToString("F0");
    }

    public void WinGame()   // helper function for win game scenario
    {
        _menuCurrentlyOpen = _winGameMenu;
        _menuCurrentlyOpen.SetActive(true);
        gameOver = true;
        LockCursorPause();

        // play 'You Win' audio clip
        _playerScript.winJingle();

    }

    public void UpdatePlayerScore() // helper function for displaying score in the win screen and lose screen
    {
        displayLoseScore.text = iScore.ToString("F0"); // lose screen score
        displayWinScore.text = iScore.ToString("F0"); // win screen score
    }
}
