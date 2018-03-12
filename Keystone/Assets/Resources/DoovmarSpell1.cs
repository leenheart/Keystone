using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoovmarSpell1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnCollisionStay(Collision collisionInfo)
        {
            Debug.Log(gameObject.name + " and " + collisionInfo.collider.name + " are still colliding");
        }

	// Update is called once per frame
	void Update () {
        
    }
}
