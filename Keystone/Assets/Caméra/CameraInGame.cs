using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInGame : MonoBehaviour
{

    private Transform _this;
    public Vector3 dir;
    public int speed;
    public float ZoomSpeed = 2f;

    private bool istransparentobstacle = false;


    // Use this for initialization
    void Start()
    {
        _this = this.gameObject.GetComponent<Transform>();
        dir = Vector3.zero;
        speed = 10;

        if (GameObject.Find("Client"))
        {
            GameObject.Find("CanvasDef").SetActive(false);
        }
        else
        {
            GameObject.Find("CanvasAtt").SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("pont")) Destroy(g);
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle")) Destroy(g);
            GameObject.Find("MapGeneration 1").GetComponent<Generation>().Generate();
            GameObject.Find("MapGeneration 1").GetComponent<Generation>().generateObstacle();
            GameObject.Find("MapGeneration 1").GetComponent<Generation>().GenerateMap();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float alpha;
            if (istransparentobstacle)
            {
                alpha = 1;
                istransparentobstacle = false;
            }
            else
            {
                alpha = 0.2f;
                istransparentobstacle = true;
            }
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                Color c = g.GetComponent<Renderer>().material.color;
                c.a = alpha;
                g.GetComponent<Renderer>().material.color = c;
            }

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if ( transform.position.x < 50)
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x > -10 )
            {
                transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.position.z > -30 )
            {
                transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if ( transform.position.z < 30)
            {
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if ( transform.position.y < 55)
            {
                transform.position += new Vector3(0, ZoomSpeed, 0);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (transform.position.y  > 2 )
            {
                transform.position -= new Vector3(0, ZoomSpeed, 0);
            }
        }

        if (Input.GetMouseButton(1))
        {
            // transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X")*5 );
            transform.RotateAround(transform.position, Vector3.left, Input.GetAxis("Mouse Y") * 3);
        }
    }
}

