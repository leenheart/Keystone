using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public abstract class Guardian : MonoBehaviour {


    public float HpMax;
    public float Hp;
    public float Endurance;
    public float EnduranceMax;
    public int Armor;
    public int MooveRangeForEndurance;
    public int OneMoove;
    public int Spell1ForEndurance;
    public int Spell2ForEndurance;
    public int Spell3ForEndurance;
    public int Spell4ForEndurance;
    public bool AbleToMoove = true;
    public bool AbleToDo = true;
    public bool Mooving = false;
    public Vector3 HitPoint;
    public int MoovingSpeed;

    public void TakeDammage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            SceneManager.LoadScene("menu");
        }
    }

    public void Moove(Vector3 hitPoint)
    {
            GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            Endurance -= MooveRangeForEndurance;
            AbleToDo = false;
            Vector3 look = hitPoint - transform.position;
            look.y = 0;
            transform.rotation = Quaternion.LookRotation(look);
            Mooving = true;
            HitPoint = hitPoint;
            MoovingSpeed = 3;
    }

    public void Update()
    {
        if (!AbleToDo && Mooving)
        {
            if (Manager.CalculRange(transform.position + transform.forward * Time.deltaTime * MoovingSpeed, HitPoint) > Manager.CalculRange(transform.position, HitPoint))
            {
                AbleToDo = true;
                Mooving = false;

            }
            else
            {
               transform.position += transform.forward * Time.deltaTime * MoovingSpeed;
            }
        }
    }


    public abstract bool Spell1Selection();
    public abstract bool Spell2Selection();
    public abstract bool Spell3Selection();
    public abstract bool Spell4Selection();

    public bool MooveSelection()
    {
        if ( AbleToDo && Endurance >= MooveRangeForEndurance)
        {
            GetComponentsInChildren<MeshRenderer>()[1].enabled = true;
            //afficher range de deplacement
            return true;
        }
        return false;
    }

    /* _________________________________________________________________________________________________________________________________________________________________________________ */

    public abstract void Spell1Activation(Vector3 hitPoint);
    public abstract void Spell2Activation(Vector3 hitPoint);
    public abstract void Spell3Activation(Vector3 hitPoint);
    public abstract void Spell4Activation(Vector3 hitPoint);

}
