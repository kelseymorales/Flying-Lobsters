using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
        if (PlayerPrefs.HasKey(sVolumeParameter) == false)
        {
            PlayerPrefs.SetFloat(sVolumeParameter, _volumeSlider.value);
        }
        else
        {
            _volumeSlider.value = PlayerPrefs.GetFloat(sVolumeParameter); 
        }
    }
}
