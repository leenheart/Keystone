using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ohnir : Guardian
{

    public GameObject Arrow;

    Quaternion pos;

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
        GetComponentsInChildren<MeshCollider>()[0].enabled = true;
        GetComponentsInChildren<MeshRenderer>()[4].enabled = false;
        Endurance -= Spell1ForEndurance;
    }
    public override void Spell2Activation(Vector3 hitPoint)
    {
        if (AbleToMoove)
        {
            Armor += 60 * Armor / 100;
            AbleToMoove = false;
        }
        else
        {
            Armor -= 60 * Armor / 100;
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

    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.name == "Arrow(Clone)")
        {
            Debug.Log("ARROW TUCH :" + collider.gameObject.name);
            Destroy(collider.gameObject);
            TakeDammage(250);
        }
        if (collider.gameObject.tag == "Player")
        {
            HitPoint = transform.position;
            Debug.Log(collider.gameObject.name + "something append ");
            collider.gameObject.GetComponent<Guardian>().TakeDammage(250);
        }
    }

    public override void Spell4Activation(Vector3 hitPoint)
    {
        //if (GameObject.Find("Manager").GetComponent<Manager>().ColliderName != "dec(Clone)" && GameObject.Find("Manager").GetComponent<Manager>().ColliderName != "arbre(Clone)")
        {
            //PlayeurNow.transform.position = hit.point + Vector3.up;
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponentsInChildren<MeshRenderer>()[3].enabled = false;
            Vector3 look = hitPoint - transform.position;
            look.y = 0;
            transform.rotation = Quaternion.LookRotation(look);
            // if (GameObject.Find("Server"))
            {
                MoovingSpeed = 10;
                Mooving = true;
                HitPoint = hitPoint;
                AbleToDo = false;
            }
        }
        Endurance -= Spell4ForEndurance;

    }

    // Use this for initialization
    void Start()
    {
        pos = transform.rotation;
        DontDestroyOnLoad(gameObject);
        HpMax = 1000;
        Hp = 1000;
        Endurance = 1000;
        EnduranceMax = 1000;
        Armor = 10;
        MooveRangeForEndurance = 100;
        OneMoove = 2;
        Spell1ForEndurance = 100;
        Spell2ForEndurance = 200;
        Spell3ForEndurance = 200;
        Spell4ForEndurance = 700;
        AbleToMoove = true;
    }
}
