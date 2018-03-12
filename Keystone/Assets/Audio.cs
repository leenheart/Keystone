using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {

    private AudioSource audio;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
	}
    public void toogleMuteMusic()
    {
        if (audio.mute)
        {
            audio.mute = false;
        }
        else
        {
            audio.mute = true;
        }
    }

}

