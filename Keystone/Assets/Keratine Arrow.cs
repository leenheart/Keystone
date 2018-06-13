using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeratineArrow : MonoBehaviour
{
    public int dommage = 150;

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Destroy(this);
            Destroy(gameObject);
            //collider.gameObject.GetComponent<Guardian>().TakeDammage(dommage);
            Destroy(this);
            Destroy(gameObject);
        }
        else
        {
            Destroy(this);
            Destroy(gameObject);
        }
        Destroy(this);
        Destroy(gameObject);
    }

}
