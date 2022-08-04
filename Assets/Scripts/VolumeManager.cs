using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] Slider mainVolumeSlider; //To access the slider

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("mainVolume"))
        {
            PlayerPrefs.SetFloat("mainVolume", 1);
            Load();
        }
        else
            Load(); 
    }

    public void ChangeMainVolume() //Changes the main volume based on slider
    {
        AudioListener.volume = mainVolumeSlider.value;
        Save();

    }

    private void Save() //Saves user preferences so the user doesn't need to change it over and over again
    {
        PlayerPrefs.SetFloat("mainVolume", mainVolumeSlider.value);
    }

    private void Load() //Loads preferences
    {
        mainVolumeSlider.value = PlayerPrefs.GetFloat("mainVolume");
    }
}
