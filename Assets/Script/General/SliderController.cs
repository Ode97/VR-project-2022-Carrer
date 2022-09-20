using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Text valueText;
    public Slider slider;
    private int progress = 0;

    private void Start()
    {
        valueText.text = "0";
        slider.value = 0;
    }

    public void Reset()
    {
        valueText.text = "0";
        slider.value = 0;
        progress = 0;

    }
    
    public void OnSliderChange(float value)
    {
        valueText.text = value.ToString();
    }

    public void UpdateProgress()
    {
        progress++;
        slider.value = progress;
    }
}
