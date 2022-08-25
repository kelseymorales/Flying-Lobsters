using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [System.Serializable]
    class Volume
    {
        public string _volumeName;

        public float fVolumeOnToggle;

        public Slider _slider;

        public Toggle _toggle;
    }
    

    [SerializeField] private Volume[] _volumeElements;
    [SerializeField] private AudioMixer _mixer;

    public void Start()
    {
        if (PlayerPrefs.HasKey(_volumeElements[0]._volumeName))
        {
            for (int i = 0; i < _volumeElements.Length; i++)
            {
                _volumeElements[i]._slider.value = PlayerPrefs.GetFloat(_volumeElements[i]._volumeName);
            }
        }
        else
        {
            for (int i = 0; i < _volumeElements.Length; i++)
            {
                PlayerPrefs.SetFloat(_volumeElements[i]._volumeName, _volumeElements[i]._slider.value);
                PlayerPrefs.Save();
            }
        }

        for (int i = 0; i < _volumeElements.Length; i++)
        {
            if (_volumeElements[i]._slider.value == 0.0f)
            {
                _mixer.SetFloat(_volumeElements[i]._volumeName, -75.0f);
            }
            else
            {
                _mixer.SetFloat(_volumeElements[i]._volumeName, Mathf.Log10(_volumeElements[i]._slider.value) * 10.0f);
            }

            if (_volumeElements[i]._slider.value > 0.0f)
            {
                _volumeElements[i]._toggle.isOn = true;
            }

            _volumeElements[i].fVolumeOnToggle = _volumeElements[i]._slider.value;
        }
    }

    public void VolumeChange(int id)
    {
        Volume current = _volumeElements[id];

        if (current._slider.value == 0.0f && current._toggle.isOn == false)
        {
            return;
        }

        current.fVolumeOnToggle = current._slider.value;
        
        if (current._slider.value == 0.0f)
        {
            _mixer.SetFloat(current._volumeName, -75.0f);
            current._toggle.isOn = false;
        }
        else
        {
            _mixer.SetFloat(current._volumeName, Mathf.Log10(current._slider.value) * 10.0f);

            current._toggle.isOn = true;
        }

        PlayerPrefs.SetFloat(current._volumeName, current._slider.value);
        PlayerPrefs.Save();
    }

    public void OnToggle(int id)
    {
        Volume current = _volumeElements[id];

        if (current._toggle.isOn)
        {
            current._slider.value = current.fVolumeOnToggle;

            if (current._slider.value == 0.0f)
            {
                current._slider.value = 0.5f;
                _mixer.SetFloat(current._volumeName, Mathf.Log10(current._slider.value) * 10.0f);
            }
            else
            {
                _mixer.SetFloat(current._volumeName, Mathf.Log10(current._slider.value) * 10.0f);
            }
             current.fVolumeOnToggle = current._slider.value;
        }
        else
        {
            current.fVolumeOnToggle = current._slider.value;
            current._slider.value = 0.0f;
            _mixer.SetFloat(current._volumeName, -75.0f);
        }

        PlayerPrefs.SetFloat(current._volumeName, current._slider.value);
        PlayerPrefs.Save();
    }
}
