using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Text valueText;
    public Slider slider;
    private int progress = 0;
    private int t = 0;
    
    private void Start()
    {
        
    }

    public void Reset(int v)
    {
        valueText.text = v.ToString();
        t = v;
        slider.value = 0;
        progress = 0;

    }

    public void OnSliderChange(float value)
    {
        var x = t - value;
        valueText.text = x.ToString();
        Debug.Log(valueText.text);
    }

    public void UpdateProgress()
    {
        progress++;
        slider.value = progress;
        var x = t - progress;
        valueText.text = x.ToString();
    }
}
