using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public abstract class Guardian : MonoBehaviour
{


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

    public GameObject ExplosionArbre;
    public GameObject ExplosionRocher;

    public RectTransform HealthBar;

    public Vector3 HitPoint;
    public int MoovingSpeed;

    public bool IsTakingDommageWhenTouchMe = false;
    public bool IsPuchingWhenTouchMe = false;

    public bool ExploseAtTheEndOfMove = false;
    public int DommageExplosion = 300;

    public int DommageTakeWhenTouchMe = 500;

    void OnCollisionEnter(Collision collider)
    {
        if (IsTakingDommageWhenTouchMe)
        {
            if (collider.gameObject.tag == "Player")
            {
                collider.gameObject.GetComponent<Guardian>().TakeDammage(DommageTakeWhenTouchMe);
                if (IsPuchingWhenTouchMe)
                {
                    Vector3 look = HitPoint - transform.position / 10;
                    look.y = 2;
                    collider.gameObject.GetComponent<Rigidbody>().AddForce(look*10);
                    IsPuchingWhenTouchMe = false;
                    IsTakingDommageWhenTouchMe = false;
                    HitPoint = transform.position;
                }
                //IsTakingDommageWhenTouchMe = false;
            }
            if (collider.gameObject.tag == "Obstacle")
            {
                if (collider.gameObject.name == ("TREE(Clone)"))
                {
                    Destroy(Instantiate(ExplosionArbre, collider.gameObject.transform.position + new Vector3(0,1,0), collider.gameObject.transform.rotation), 3);
                }
                else
                {
                    Destroy(Instantiate(ExplosionRocher, collider.gameObject.transform), 3);
                }
                Destroy(collider.gameObject);
            }

        }
        /*else if (Mooving)
        {
            if (collider.gameObject.tag == "Obstacle")
            {
                HitPoint = transform.position;
            }
        }*/

    }

    public void TakeDammage(int damage)
    {
        Hp -= damage - damage*Armor/100;

        HealthBar.sizeDelta = new Vector2(Hp/10, HealthBar.sizeDelta.y);

        if (Hp <= 0)
        {
            //fixme game over
            SceneManager.LoadScene("menu");
        }
    }

    public void Moove(Vector3 hitPoint)
    {

        //if (GameObject.Find("Server"))
        {
            MoovingSpeed = 3;
            Mooving = true;
            HitPoint = hitPoint;
            AbleToDo = false;
        }

        GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
        Endurance -= MooveRangeForEndurance * (int) Manager.CalculRange(HitPoint,transform.position);

        Vector3 look = hitPoint - transform.position;
        look.y = 0;
        transform.rotation = Quaternion.LookRotation(look);

    }

    public void Update()
    {
        Vector3 look = Camera.main.transform.position - transform.position;
        look.y = 0;
        GetComponentsInChildren<Canvas>()[0].gameObject.transform.rotation = Quaternion.LookRotation(look);

        if (transform.position.y < -50)
        {
            TakeDammage(1000000);
            transform.position = new Vector3(0, 0, 0);
            //FIXME LE JOUEUR MEURT !
        }
        if (!AbleToDo && Mooving)
        {
            if (Manager.CalculRange(transform.position + transform.forward * Time.deltaTime * MoovingSpeed, HitPoint) >= Manager.CalculRange(transform.position, HitPoint))
            {
                AbleToDo = true;
                Mooving = false;

                if (ExploseAtTheEndOfMove)
                {

                    foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (g != gameObject)
                        {
                            if (Manager.CalculRange(g.transform.position, transform.position) <= 5)
                            {
                                g.GetComponent<Guardian>().TakeDammage(DommageExplosion);

                                look = g.transform.position - transform.position / 10;
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
                }

                ExploseAtTheEndOfMove = false;
                IsTakingDommageWhenTouchMe = false;
                IsPuchingWhenTouchMe = false;

            }
            else
            {
                //if (GameObject.Find("MapGeneration 1").GetComponent<Generation>().map[(int) transform.position.x, (int) transform.position.z] == 1)
                if (transform.position.y > 0)
                {
                    transform.position += transform.forward * Time.deltaTime * MoovingSpeed;
                }

            }
        }
    }


    public abstract bool Spell1Selection();
    public abstract bool Spell2Selection();
    public abstract bool Spell3Selection();
    public abstract bool Spell4Selection();

    public bool MooveSelection()
    {
        if (AbleToDo && Endurance >= MooveRangeForEndurance)
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
