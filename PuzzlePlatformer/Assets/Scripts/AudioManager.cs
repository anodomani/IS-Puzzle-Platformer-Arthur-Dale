using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool globalSound;
    public bool playOnStart;
    public float lerpRate;

    public float audioLevel;
    public new AudioClip[] audio;
    public AudioSource audioSource;
    public bool looping;
    public int medHotCold;
    public bool warpSound;

    // Start is called before the first frame update
    void Start()
    {
        if (globalSound)
        {
            DontDestroyOnLoad(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        if (playOnStart)
        {
            if (looping)
            {
                audioSource.loop = true;
                PlayOneshot();
            }
        }
    }

    public void Update()
    {
        if (warpSound && Input.GetButtonDown("Warp"))
        {
            PlayOneshot();
        }
    }

    public void PlayOneshot()
    {
        for (int i = 0; i < audio.Length; i++)
        {
            audioSource.PlayOneShot(audio[i], audioLevel);
            print("Playing Oneshot");
        }
    }
}
