using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{
    public AudioSource aTrack1;
    public AudioSource aTrack2;
    public AudioSource aTrack3;

    public int iTrackSelector;
    public int iTrackHistory;

    void Start()
    {
        iTrackSelector = Random.Range(0, 3);

        if(iTrackSelector == 0)
        {
            aTrack1.Play();
            iTrackHistory = 1;
        }
        else if (iTrackSelector == 1)
        {
            aTrack1.Play();
            iTrackHistory = 2;
        }
        else if(iTrackSelector == 2)
        {
            aTrack1.Play();
            iTrackHistory = 3;
        }

        SceneManager.sceneLoaded += DestroyMusic;
    }

    void Update()
    {
        if (aTrack1.isPlaying == false && aTrack2.isPlaying == false && aTrack3.isPlaying == false)
        {
            iTrackSelector=Random.Range(0, 3);

            if (iTrackSelector == 0 && iTrackHistory != 1)
            {
                aTrack1.Play();
                iTrackHistory = 1;
            }
            else if (iTrackSelector == 1 && iTrackHistory != 2)
            {
                aTrack2.Play();
                iTrackHistory = 2;
            }
            else if (iTrackSelector == 2 && iTrackHistory != 3)
            {
                aTrack3.Play();
                iTrackHistory = 3;
            }
        }
    }

    void DestroyMusic(Scene scene, LoadSceneMode mode)
    {
        if(scene == null)
        {
            Destroy(gameObject);
        }
    }
}
