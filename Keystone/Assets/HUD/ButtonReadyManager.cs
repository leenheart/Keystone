using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonReadyManager : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	}
    public void BeReady()
    {
        Player.pret = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
