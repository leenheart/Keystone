using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guemnaar : Guardian
{

    public GameObject demon;

    public override bool Spell1Selection()
    {
        if (Endurance >= 100)
        {
            GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[2].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[4].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[3].enabled = true;
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
            GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[2].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[3].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[4].enabled = true;
            return true;
        }
        return false;
    }

    public override bool Spell4Selection()
    {
        if (Endurance >= 100)
        {
            GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[2].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[3].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[4].enabled = false;
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

                if (Manager.CalculRange(g.transform.position, transform.position) <= 5) //si je suis dans un rectangle !
                {
                    g.GetComponent<Guardian>().TakeDammage(GetDommage(300));
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
        DommageExplosion = GetDommage(300);

        Endurance -= Spell3ForEndurance;

    }
    public override void Spell4Activation(Vector3 hitPoint)
    {
        if (!GameObject.Find("Demon"))
        {
            GameObject g = Instantiate(demon, transform);
            if (GameObject.Find("Server"))
            {
                g.GetComponentsInChildren<SpriteRenderer>()[0].enabled = false;
                g.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
            }
            else
            {
                g.GetComponentsInChildren<SpriteRenderer>()[0].enabled = true;
                g.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
            }
        }
        Endurance -= Spell4ForEndurance;
    }


    public int GetDommage(int dommage)
    {
        if (Hp <= HpMax / 2)
        {
            return dommage * 2;
        }
        return dommage;
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
