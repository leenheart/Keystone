using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowOfHorn : MonoBehaviour
{
    void OnCollisionEnter(Collision collider)
    {

        if (collider.gameObject.name == "arbre(Clone)" || collider.gameObject.name == "dec(Clone)" || collider.gameObject.name == "Castle")
        {
            Destroy(gameObject);
        }
        if (collider.gameObject.name == "Playeur")
        {
            collider.gameObject.GetComponent<Guardian>().TakeDammage(400);
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
