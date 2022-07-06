using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] public GameObject _player;
    [SerializeField] public playerController _playerScript;

    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject playerDamageFlash;
    public bool isPaused = false;
    public Image HPBar;

    void Awake()
    {
        instance = this;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerScript = player.GetComponent<playerController>();
    }


}
