using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    private Transform _this;
    public Vector3 dir;

	// Use this for initialization
	void Start () {
        _this = this.gameObject.GetComponent<Transform>();
         dir = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
        dir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            dir = Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            dir = Vector3.right;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            dir = Vector3.forward;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            dir = Vector3.back;
        }

        _this.position += dir * Time.deltaTime * 5;
    }
}
