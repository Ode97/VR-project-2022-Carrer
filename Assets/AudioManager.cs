using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private Slider volume;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeVolume()
    {
        _audioSource.volume = volume.value;
    }
}
