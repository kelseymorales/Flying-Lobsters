using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;


public class VolumeManager : MonoBehaviour
{
    [SerializeField] string sVolumeParameter = "MasterVolume"; //String to save the playerPreferences. 
    [SerializeField] Slider _volumeSlider; //To access the slider
    [SerializeField] AudioMixer _mixer; // To access the mixer 
    [SerializeField] private Toggle _toggle; //Toggle used to mute the audio completely
    private bool isToggleEventDisable; //Boolean to check if the toggle is disabled. 

    

    private void Awake() 
    {
        _volumeSlider.onValueChanged.AddListener(HandleSliderValueChanged);
        _toggle.onValueChanged.AddListener(HandleToggleValueChanged);

        
    }

    private void HandleSliderValueChanged(float value) //Will convert the change in the slider (value 1-0) to decibels
    {
        if (value <= 0)
            _mixer.SetFloat(sVolumeParameter, -75.0f); 
        else
            _mixer.SetFloat(sVolumeParameter, Mathf.Log10(value) * 20);
        isToggleEventDisable = true; 
        _toggle.isOn = _volumeSlider.value > _volumeSlider.minValue;
        isToggleEventDisable = false;

        if (!GameManager._instance.options.ContainsKey(sVolumeParameter))
        {
            GameManager._instance.options.Add(sVolumeParameter, _volumeSlider.value.ToString());
            GameManager._instance.sNames.Add(sVolumeParameter);
        }
        else
        {
            GameManager._instance.options[sVolumeParameter] = _volumeSlider.value.ToString();
        }

        PlayerPrefs.SetFloat(sVolumeParameter, _volumeSlider.value);
        PlayerPrefs.Save();

    }

    private void HandleToggleValueChanged(bool enableSound) //Handles the muting and setting back the sound to a value using the toggle
    {
        if (isToggleEventDisable)
            return; 
        if (enableSound)
            _volumeSlider.value = _volumeSlider.maxValue;
        else
            _volumeSlider.value = _volumeSlider.minValue; 
    }


    // Start is called before the first frame update
    void Start()
    {
        _volumeSlider.value = PlayerPrefs.GetFloat(sVolumeParameter, _volumeSlider.value); 
    }

    //public void SaveOptions()
    //{
    //    string sStringSeparator = "|";
        
    //    //Creates writers to both values and names files
    //    StreamWriter writerValues = new StreamWriter(File.OpenWrite(Application.dataPath + "/saveAudioValues.txt"));
    //    StreamWriter writerNames = new StreamWriter(File.OpenWrite(Application.dataPath + "/saveAudioNames.txt")); 

    //    for (int i = 0; i < GameManager._instance.options.Count; i++)
    //    {
    //        //In both cases we add a separator
    //        writerNames.Write(GameManager._instance.sNames[i] + sStringSeparator); //Saves name to the  names file
    //        writerValues.Write(GameManager._instance.options[GameManager._instance.sNames[i]] + sStringSeparator); //Saves value to the values file
    //    }

         
    //    //string sSaveStringValues = string.Join(sStringSeparator, sContentsValues);
    //    // string sSaveStringNames = string.Join(sStringSeparator, sContentsNames);

    //    //Closes writers
    //    writerValues.Close(); 
    //    writerNames.Close(); 

    //    //File.WriteAllText(Application.dataPath + "/saveAudioValues.txt", sSaveStringValues);
    //   // File.WriteAllText(Application.dataPath + "/saveAudioNames.txt", sSaveStringNames);
    //}

    
}
