using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolSliderScript : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    
    void Start()
    {
        _slider.onValueChanged.AddListener(val => GameSetting.Instance.ChangeMasterVolume(val));
    }
}
