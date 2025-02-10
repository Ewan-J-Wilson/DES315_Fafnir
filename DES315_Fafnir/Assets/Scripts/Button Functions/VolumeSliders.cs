using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    [SerializeField] AudioType AudioBus;

    public void Start()  
    { 
        Slider slider = GetComponent<Slider>();
        // Load the audio level from settings
        slider.value = PlayerPrefs.GetFloat("Audio: " + AudioBus.ToString());
        // Add the trigger for the slider being changed
        slider.onValueChanged.AddListener(ValueChanged);
    }

    private void ValueChanged(float value)
    { 
        // Change the volume
        Audiomanager.ChangeVolume(value, AudioBus);
        // Save the new value
        PlayerPrefs.SetFloat("Audio: " + AudioBus.ToString(), value);
        PlayerPrefs.Save();
    }

}
