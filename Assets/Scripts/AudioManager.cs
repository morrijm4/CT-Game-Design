using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] AudioSource musicsource;
    [SerializeField] AudioSource SFXSource;
    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip shoot;
    public AudioClip pellet;

    private void Start()
    {
        musicsource.clip = background;
        musicsource.Play(); 
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}



