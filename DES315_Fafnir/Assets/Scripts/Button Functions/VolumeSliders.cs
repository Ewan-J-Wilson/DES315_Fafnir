using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    [SerializeField] AudioType AudioBus;

    public void Start()  
    { GetComponent<Slider>().onValueChanged.AddListener(ValueChanged); }

    private void ValueChanged(float value)
    { Audiomanager.ChangeVolume(value, AudioBus); }

}
