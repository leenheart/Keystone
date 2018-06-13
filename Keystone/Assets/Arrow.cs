using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public int dommage = 250;

    void OnCollisionEnter(Collision collider)
    {
        Debug.Log("arrow tuch : " + collider.gameObject.name);
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("take dommage");
            collider.gameObject.GetComponent<Guardian>().TakeDammage(dommage);
        }
        else
        {
            Debug.Log("Destroy arrow");
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
