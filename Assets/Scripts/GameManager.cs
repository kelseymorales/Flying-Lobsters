using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


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

    // game menus
    public GameObject _pauseMenu;
    public GameObject _playerDeadMenu;
    public GameObject _winGameMenu;
    public GameObject _winLevelMenu;
    public GameObject _loseGameMenu;

    //control display screen
    public GameObject _controlWindow;

    //game menu first options for implemented keyboard input
    public EventSystem _eventSystem;
    public GameObject _pauseMenuFirstOption;
    public GameObject _loseGameMenuFirstOption;
    public GameObject _playerDeadMenuFirstOption;
    public GameObject _winGameMenuFirstOption;
    public GameObject _winLevelFirstOption;
    public GameObject _bombUi;

    // UI - sniperZoom
    public GameObject SniperScope;

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

    // UI - player ammo total, ammo clip, current shots left in clip, and player grenade ammo
    public TMP_Text ammoTotal;
    public TMP_Text clipSize;
    public TMP_Text shotsInClip;
    public TMP_Text grenadeAmmo;

    //UI - Power-ups badge to show when a power-up is active

    public Image _powerUpShield;
    public Image _powerUpDamage;
    public Image _powerUpSpeed;
    public Image _powerUpAmmo;

    //Boss UI
    [SerializeField] public string sBossName;
    public TMP_Text _bossName;
    public Image _bossHealth;
    public GameObject _bossHealthBar;

    [Header("Audio\n------------------------------")]
    [SerializeField] AudioMixer _mixer; //Audio mixer Main

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
    [SerializeField] TMP_Text displayLoseScore;                 // UI component that displays score on lose screen
    [SerializeField] private bool bRestartCurrentLevel;
    [SerializeField] private int buildIndex;

    [Header("Text Prompts\n------------------------------")]
    [SerializeField] public GameObject defuseLabel;             // reference to prompt shown to defuse bombs
    [SerializeField] public GameObject grenadeDefuseLabel;      //reference to prompt show to defuse grenade

    // stores variables keeping track of enemies killed, enemies spawned, bombs defused, bombs active, and original bomb total
    [HideInInspector] public int iEnemyKillGoal;
    [HideInInspector] public int iEnemiesKilled;
    [HideInInspector] public int iBombsActive;
    [HideInInspector] public int iBombsDefusedCounter;
    [HideInInspector] public int iGrenadesDefusedCounter;              //helper int for grenades defused
    [HideInInspector] public int iBombTotalOrig;

    [HideInInspector] GameObject _menuCurrentlyOpen;

    Coroutine _defuseFunction;          // stores reference to any game menu currently open/active

    public bool isDefusingTrap;
    public bool isDefusingGrenade; //bool for defusing enemy grenades
    public bool isGrenadeDefused;

    public bool didWin = false;
    public bool didWinLevel = false;

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

        s = GameObject.FindGameObjectsWithTag("Bomb");

        iBombsActive = s.Length;
        
        if (iBombsActive == 0)
        {
            _bombUi.gameObject.SetActive(false);
        }
        else
        {

            tBombTotal.text = iBombsActive.ToString("F0");

            StartCoroutine(bombTick());  
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadAudioSettings();
        }
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
                //Setting up event system to show highlighted button
                _eventSystem.SetSelectedGameObject(null);
                _eventSystem.SetSelectedGameObject(_pauseMenuFirstOption);

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

    public void Restart()
    {
        UnlockCursorUnpause();

        if (bRestartCurrentLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            if (didWinLevel == true || didWin == true)
            {
                
                if (SceneManager.GetActiveScene().buildIndex == 2)
                {
                    SceneManager.LoadScene(5);
                }
                else if (SceneManager.GetActiveScene().buildIndex == 3)
                {
                    SceneManager.LoadScene(6);
                }
                else if (SceneManager.GetActiveScene().buildIndex == 4)
                {
                    SceneManager.LoadScene(7);
                }
            }
            else
            {
                
                if (SceneManager.GetActiveScene().buildIndex == 2)
                {
                    SceneManager.LoadScene(10);
                }
                else if (SceneManager.GetActiveScene().buildIndex == 3)
                {
                    SceneManager.LoadScene(11);
                }
                else if (SceneManager.GetActiveScene().buildIndex == 4)
                {
                    SceneManager.LoadScene(12);
                }
            }
        }
    }

    public void PlayerDead()
    {
        gameOver = true;
        isPaused = true;
        _menuCurrentlyOpen = _playerDeadMenu;
        _menuCurrentlyOpen.SetActive(true);
        LockCursorPause();

        //Setting up event system to show highlighted button
        _eventSystem.SetSelectedGameObject(null);
        _eventSystem.SetSelectedGameObject(_playerDeadMenuFirstOption);

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

        if(iEnemiesKilled % 5 == 0) //Checks kill number and set power-up drop flag to true
        {
            _playerScript.isReadyForDrop = true; 
        }

        tEnemiesDead.text = iEnemiesKilled.ToString("F0");
        iScore += iEnemiesKilled * 10; // score tracking for enemy kills - also bonus points are allocated for headshot kills in playerController shoot function
        UpdatePlayerScore(); // call to helper function to update score on win/lose screens
    }

    public void CallDefuse(bombGoal bomb)
    {
        _defuseFunction = StartCoroutine(Defuse(bomb));
    }

    IEnumerator Defuse(bombGoal bomb)
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
        
        if (!isDefusingTrap && !isDefusingGrenade) //Checks whether we are defusing a bomb or a trap and grenade
        {
            // update game goals 
            iBombsActive--;
            iBombsDefusedCounter++;
            iScore += 50;
            UpdatePlayerScore(); // call to helper function to update score on win/lose screens

            //deactivate UI elements showing defusing in process

            tBombsDefused.text = iBombsDefusedCounter.ToString("F0"); // update bombs defused UI element

            StopSpawners();
        }


        //deactivate UI elements showing defusing in process
        _defuseCountdownObject.SetActive(false);
        _defuseSlider.SetActive(false);

        _playerScript.UnlockInPlace(); // unlock player position

        defuseLabel.SetActive(false); // make sure the prompt to defuse bombs deactivates now that bomb is defused
        grenadeDefuseLabel.SetActive(false);



        _playerScript.defuseJingle(); // play defuse audio jingle

        bomb.SetDefusedState();       // tells the bomb it is defused

        //resets trap and grenade bools after defusing
        if (isDefusingTrap)
            isDefusingTrap = false;
        if (isDefusingGrenade)
            isDefusingGrenade = false;

        // Level Win Condition
        if (iBombsActive == 0)
        {
            levelWin = true;

            WinLevel();
            // will need to be removed later, as the win game should be called at the end of level 3, and all we need here is the level win trigger for transitioning levels
        }
    }

    public void StopDefuseing()
    {
        if (_defuseFunction == null)
        {
            return;
        }

        StopCoroutine(_defuseFunction);

        // activate UI elements showing defusing in process
        _defuseCountdownObject.SetActive(false);
        _defuseSlider.SetActive(false);

        _playerScript.UnlockInPlace();
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
        Instantiate(_detonation);

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
        //Setting up event system to show highlighted button
        _eventSystem.SetSelectedGameObject(null);
        _eventSystem.SetSelectedGameObject(_loseGameMenuFirstOption);

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

    public void updateAmmoCount()   // helper function for updating UI with current ammo count
    {
        ammoTotal.text = _playerScript.iTotalWeaponAmmo.ToString("F0"); // ammo pool total
        shotsInClip.text = _playerScript.iWeaponAmmo.ToString("F0"); // current Clip
        clipSize.text = _playerScript.iWeaponAmmoOrig.ToString("F0");
    }

    public void updateGrenadeCount() // helper function for updating UI with current grenade count
    {
        grenadeAmmo.text = _playerScript.iGrenadeCount.ToString("F0");
    }

    public void WinLevel()   // helper function for win game scenario
    {
        didWinLevel = true;
        _menuCurrentlyOpen = _winLevelMenu;
        _menuCurrentlyOpen.SetActive(true);
        gameOver = true;
        LockCursorPause();

        //Setting up event system to show highlighted button
        _eventSystem.SetSelectedGameObject(null);
        _eventSystem.SetSelectedGameObject(_winLevelFirstOption);

        // play 'You Win' audio clip
        _playerScript.winJingle();

    }


    public void WinGame()   // helper function for win game scenario
    {
        didWin = true;
        _menuCurrentlyOpen = _winGameMenu;
        _menuCurrentlyOpen.SetActive(true);
        gameOver = true;
        LockCursorPause();

        //Setting up event system to show highlighted button
        _eventSystem.SetSelectedGameObject(null);
        _eventSystem.SetSelectedGameObject(_winGameMenuFirstOption);

        // play 'You Win' audio clip
        _playerScript.winJingle();
        didWin = true;

    }

    public void UpdatePlayerScore() // helper function for displaying score in the win screen and lose screen
    {
        displayLoseScore.text = iScore.ToString("F0"); // lose screen score
        displayWinScore.text = iScore.ToString("F0"); // win screen score
    }

    public void OpenOptionsInGame() //Created to open the menu inside the actual game and not in the main menu
    {
        _menuCurrentlyOpen.SetActive(false);
        _menuCurrentlyOpen = _optionsMenu;
        _menuCurrentlyOpen.SetActive(true);
        LockCursorPause();
    }

    public void OpenControlWindow()
    {
        _menuCurrentlyOpen.SetActive(false);
        _menuCurrentlyOpen = _controlWindow;
        _menuCurrentlyOpen.SetActive(true);
        LockCursorPause();
    }

    public void CloseControlWindow()
    {
        _menuCurrentlyOpen.SetActive(false);
        _menuCurrentlyOpen = _pauseMenu;
        _menuCurrentlyOpen.SetActive(true);
    } 

    public void CloseOptionsInGame() //Created to close the menu inside the actual game and not in the main menu
    {
        _menuCurrentlyOpen.SetActive(false);
        _menuCurrentlyOpen = _pauseMenu;
        _menuCurrentlyOpen.SetActive(true);
    }

    public void LoadAudioSettings()
    {
        _mixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
        _mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        _mixer.SetFloat("EffectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("EffectsVolume")) * 20);
    }

    public void SetBossHealthBarActive(bool state)
    {
        _bossHealthBar.SetActive(state);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2))
        {
            SceneManager.LoadScene(8);
        }
        else if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3))
        {           
            
            SceneManager.LoadScene(9);
            
        }
        
        
    }
}
