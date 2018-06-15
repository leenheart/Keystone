using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{


    public AudioSource MusicSource;
    public AudioSource AmbianceSource;
    public AudioSource SpellSource;
    public AudioSource ButtonSource;


    public AudioClip clipMenu;
    public AudioClip clipJeuOhnirDef;


    public AudioClip ArbreDestroy;
    public AudioClip EOhnir;
    public AudioClip BruitRhino;
    public AudioClip BruitPasRhino;
    public AudioClip BruitButton;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        MusicSource.clip = clipMenu;
        MusicSource.Play();
    }

    public void SetMusicToOhnirDef()
    {
        MusicSource.volume = 1;
        MusicSource.clip = clipJeuOhnirDef;
        MusicSource.Play();
    }

    public void ArbreExplose()
    {
        if (!AmbianceSource.isPlaying)
        {
            AmbianceSource.clip = ArbreDestroy;
            AmbianceSource.Play();
        }
    }

    public void AmbianceEOhnir()
    {
        if (!SpellSource.isPlaying)
        {
            SpellSource.clip = EOhnir;
                 SpellSource.time = 0.4f;
            SpellSource.Play();
        }
    }

    public void AmbianceRhinoBruit()
    {
        if (!AmbianceSource.isPlaying)
        {
            AmbianceSource.clip = BruitRhino;
            AmbianceSource.time = 0.5f;
            AmbianceSource.Play();
        }
    }

    public void AmbiancePasRhino()
    {
        if (!SpellSource.isPlaying)
        {
            SpellSource.clip = BruitPasRhino;
            SpellSource.Play();
        }
    }

    public void ButtonSort()
    {
        ButtonSource.clip = BruitButton;
        ButtonSource.Play();
    }
}

