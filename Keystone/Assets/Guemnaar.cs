using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guemnaar : Guardian
{

    public override bool Spell1Selection()
    {
        if (Endurance >= 100)
        {
            GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[2].enabled = true;
            return true;
        }
        return false;
    }

    public override bool Spell2Selection()
    {
        return false;
    }

    public override bool Spell3Selection()
    {
        if (Endurance >= 100)
        {
            //GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            // GetComponentsInChildren<MeshRenderer>()[4].enabled = true;
            return true;
        }
        return false;
    }

    public override bool Spell4Selection()
    {
        if (Endurance >= 100)
        {
            //GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            // GetComponentsInChildren<MeshRenderer>()[4].enabled = true;
            return true;
        }
        return false;
    }

    public override void Spell1Activation(Vector3 hitPoint)
    {
        GetComponentsInChildren<MeshRenderer>()[2].enabled = false;

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (g != gameObject)
            {
                Vector2 posAdv = g.transform.position;
                Vector2 posMe = transform.position;

                if (Manager.CalculRange(g.transform.position, transform.position) <= 5) //si je suis dans un rectangle !
                {
                    g.GetComponent<Guardian>().TakeDammage(300);
                }
            }
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            if (Manager.CalculRange(g.transform.position, transform.position) <= 5)
            {
                Destroy(g);
            }

        }
        Endurance -= Spell1ForEndurance;
    }
    public override void Spell2Activation(Vector3 hitPoint)
    {
        //nothing
    }
    public override void Spell3Activation(Vector3 hitPoint)
    {
        gameObject.GetComponent<Collider>().enabled = false;
        GetComponentsInChildren<MeshRenderer>()[3].enabled = false;

        Vector3 look = hitPoint - transform.position;
        look.y = 0;
        transform.rotation = Quaternion.LookRotation(look);

        MoovingSpeed = 30;
        Mooving = true;
        HitPoint = hitPoint;
        AbleToDo = false;
        ExploseAtTheEndOfMove = true;

        Endurance -= Spell3ForEndurance;

    }
    public override void Spell4Activation(Vector3 hitPoint)
    {

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
