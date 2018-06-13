using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ohnir : Guardian
{

    public GameObject Arrow;

    public override bool Spell1Selection()
    {
        if (Endurance >= 100)
        {
            GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[4].enabled = true;
            return true;
        }
        return false;
    }
    public override bool Spell2Selection()
    {
        if (Endurance >= 200)
        {
            GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            return true;
        }
        return false;
    }
    public override bool Spell3Selection()
    {
        if (Endurance >= 200)
        {
            GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[6].enabled = true;
            return true;
        }
        return false;
    }
    public override bool Spell4Selection()
    {
        if (Endurance >= 700)
        {
            GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            GetComponentsInChildren<MeshRenderer>()[3].enabled = true;
            return true;
        }
        return false;
    }

    /* ________________________________________________________________________________________________________________________________________________________________________________________________ */

    public override void Spell1Activation(Vector3 hitPoint)
    {
        GetComponentsInChildren<MeshRenderer>()[4].enabled = false;

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (g != gameObject)
            {
                if (Manager.CalculRange(g.transform.position, transform.position) <= 5)
                {
                    g.GetComponent<Guardian>().TakeDammage(300);

                    Vector3 look = g.transform.position - transform.position / 10;
                    look.y = 2;
                    g.GetComponent<Rigidbody>().AddForce(look * 10);

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
        if (AbleToMoove)
        {
            Armor += 20;
            AbleToMoove = false;
        }
        else
        {
            Armor -= 20;
            AbleToMoove = true;
        }
        Endurance -= Spell2ForEndurance;
    }
    public override void Spell3Activation(Vector3 hitPoint)
    {


        Vector3 look = hitPoint - transform.position;
        look.y = 0;
        transform.rotation = Quaternion.LookRotation(look);
        GameObject arrowNow = Instantiate(Arrow, transform.position + transform.forward + transform.forward + transform.forward, transform.rotation);
        Destroy(arrowNow, 5);
        arrowNow.GetComponent<Rigidbody>().AddForce(transform.forward * 100 * Time.deltaTime);
        Endurance -= Spell3ForEndurance;

        GetComponentsInChildren<MeshRenderer>()[6].enabled = false;

    }
    public override void Spell4Activation(Vector3 hitPoint)
    {
        GetComponentsInChildren<MeshRenderer>()[3].enabled = false;

        Vector3 look = hitPoint - transform.position;
        look.y = 0;
        transform.rotation = Quaternion.LookRotation(look);

        MoovingSpeed = 10;
        Mooving = true;
        HitPoint = hitPoint;
        AbleToDo = false;

        IsTakingDommageWhenTouchMe = true;
        DommageTakeWhenTouchMe = 600;
        IsPuchingWhenTouchMe = true;

        Endurance -= Spell4ForEndurance;

    }

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        HpMax = 1000;
        Hp = 1000;

        Endurance = 500;
        EnduranceMax = 1000;

        Armor = 10;

        MooveRangeForEndurance = 50;
        OneMoove = 2;

        Spell1ForEndurance = 300;
        Spell2ForEndurance = 100;
        Spell3ForEndurance = 100;
        Spell4ForEndurance = 00;

        AbleToMoove = true;
    }
}
