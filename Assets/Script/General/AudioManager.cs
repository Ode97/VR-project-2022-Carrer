using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundtrack;
    [SerializeField] private AudioSource audioSourceButton;
    [SerializeField] private Slider volume;
    [SerializeField] private AudioClip confirm;
    [SerializeField] private AudioClip error;
    [SerializeField] private AudioClip move;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip construction;
    
    // Start is called before the first frame update

    public void Confirm()
    {
        audioSourceButton.clip = confirm;
        audioSourceButton.Play();
    }
    
    public void Error()
    {
        audioSourceButton.clip = error;
        audioSourceButton.Play();
    }
    
    public void Move()
    {
        audioSourceButton.clip = move;
        audioSourceButton.Play();
    }
    
    public void Explosion()
    {
        audioSourceButton.clip = explosion;
        audioSourceButton.Play();
    }
    
    public void Construction()
    {
        audioSourceButton.clip = construction;
        audioSourceButton.Play();
    }

    public void ChangeVolume()
    {
        audioSourceButton.volume = volume.value;
        //soundtrack.volume = volume.value;
    }
}
