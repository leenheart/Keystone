using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(Resources.Load("Playeur"), Vector3.zero + Vector3.up * 5, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.G))
        { 
            Instantiate(Resources.Load("Dec"), Vector3.zero + Vector3.left * Random.Range(-25.0f, 25.0f) + Vector3.forward * Random.Range(-25.0f, 25.0f), Quaternion.identity);
        }
    }
}