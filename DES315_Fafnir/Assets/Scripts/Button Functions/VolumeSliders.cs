using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    [SerializeField] AudioType AudioBus;
    private const float soundCooldown = 0.3f;
    private float soundTimer = 0f;

    public void Start()  
    { 
        Slider slider = GetComponent<Slider>();
        // Load the audio level from settings
        slider.value = PlayerPrefs.HasKey("Audio: " + AudioBus.ToString()) ?
            PlayerPrefs.GetFloat("Audio: " + AudioBus.ToString()) : slider.maxValue;
        // Add the trigger for the slider being changed
        slider.onValueChanged.AddListener(ValueChanged);
    }

    private void ValueChanged(float value)
    { 
        // Change the volume
        Audiomanager.ChangeVolume(value, AudioBus);
        if (soundTimer >= soundCooldown) { 
            Audiomanager.instance.PlayAudio("Kick"); 
            soundTimer = 0f;
        }
        // Save the new value
        PlayerPrefs.SetFloat("Audio: " + AudioBus.ToString(), value);
        PlayerPrefs.Save();
    }

    private void Update() { 
        if (soundTimer <= soundCooldown)
        { soundTimer += Time.deltaTime; }
    }

}
