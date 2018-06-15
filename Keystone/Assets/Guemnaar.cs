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
            Debug.Log(true);
            return true;
        }
        Debug.Log(false);
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
        GetComponentsInChildren<MeshRenderer>()[3].enabled = false;
        Destroy(Instantiate(ExplosionFeu, transform.position, new Quaternion()), 3);
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
                if (g.name == ("TREE(Clone)"))
                {
                    GameObject.Find("Music").GetComponent<Music>().ArbreExplose();
                    Destroy(Instantiate(ExplosionArbre, g.transform.position + new Vector3(0, 1, 0), gameObject.transform.rotation), 3);
                }
                else
                {
                    Destroy(Instantiate(ExplosionRocher, g.transform.position + new Vector3(0, 1, 0), gameObject.transform.rotation), 3);
                }
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
        GetComponentsInChildren<MeshRenderer>()[4].enabled = false;

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
            
            GameObject g = Instantiate(demon, transform.position, transform.rotation);
            g.transform.Rotate(-90, 0, 0);
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

        Spell1ForEndurance = 100;
        Spell2ForEndurance = 100;
        Spell3ForEndurance = 100;
        Spell4ForEndurance = 100;

        AbleToMoove = true;
    }
}
