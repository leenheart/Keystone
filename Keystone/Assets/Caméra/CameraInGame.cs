using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInGame : MonoBehaviour
{

    private Transform _this;
    public Vector3 dir;
    public int speed;
    public float ZoomSpeed = 2f;


    // Use this for initialization
    void Start()
    {
        _this = this.gameObject.GetComponent<Transform>();
        dir = Vector3.zero;
        speed = 10;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0,0,-speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0,0, speed * Time.deltaTime);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (transform.position.z > 2)
            {
                transform.position += new Vector3(0, ZoomSpeed, 0);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (transform.position.z > 2)
            {
                transform.position -= new Vector3(0, ZoomSpeed, 0);
            }
        }
    }
}
