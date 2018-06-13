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
            collider.gameObject.GetComponent<Guardian>().TakeDammage(dommage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
}
