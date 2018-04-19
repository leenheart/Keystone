using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    int turn;
    float endNextTurn;

   
    public GameObject Attacker;
    public GameObject Defender;

    GameObject PlayeurNow;

    public float turnTime;
    public string Selection;

    // Use this for initialization
    void Start()
    {
        Selection = "";
        turn = 0;
        endNextTurn = turnTime;
        PlayeurNow = Attacker;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= endNextTurn) //passage au tour suivant
        {
            turn++;
            endNextTurn += turnTime;

            if (PlayeurNow == Attacker) // changement du joueur 
            {
                PlayeurNow = Defender;
            }
            else
            {
                PlayeurNow = Attacker;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (Selection == "Spell1")
                {
                    PlayeurNow.GetComponent<Guardian>().Spell1Activation(hit);                            
                }
                else if (Selection == "Spell2")
                {
                    PlayeurNow.GetComponent<Guardian>().Spell2Activation(hit);
                }
                else if (Selection == "Spell3")
                {
                    PlayeurNow.GetComponent<Guardian>().Spell3Activation(hit);
                }
                else if (Selection == "Spell4")
                {
                    PlayeurNow.GetComponent<Guardian>().Spell4Activation(hit);
                }

                else if (Selection == "Moove")
                {                                  
                    //si la position que le mec a donner est correcte
                    if (CalculRange(PlayeurNow.transform.position, hit.point) <= PlayeurNow.GetComponent<Guardian>().MooveRangeForEndurance && hit.collider.name != "dec(Clone)" && hit.collider.name != "arbre(Clone)")
                    {

                        PlayeurNow.transform.position = hit.point + Vector3.up;
                        PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
                        PlayeurNow.GetComponent<Guardian>().Endurance -= PlayeurNow.GetComponent<Guardian>().MooveRangeForEndurance;
                    }                                    

                }
                Selection = ""; 
            }
        }
    }

    public static float CalculRange(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2));
    }

    public void Moove()
    {
             
        if (PlayeurNow.GetComponent<Guardian>().Endurance >= PlayeurNow.GetComponent<Guardian>().MooveRangeForEndurance) {
            Selection = "Moove";
            PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = true;
            
            //afficher range de deplacement
        }
    }
    public void Spell1()
    {
        if (PlayeurNow.GetComponent<Guardian>().Spell1Selection())
        {
            Selection = "Spell1";
        }
        
    }
    public void Spell2()
    {
        if (PlayeurNow.GetComponent<Guardian>().Spell2Selection())
        {
            Selection = "Spell2";
        }


    }
    public void Spell3()
    {
        if (PlayeurNow.GetComponent<Guardian>().Spell3Selection())
        {
            Selection = "Spell3";
        }

    }
    public void Spell4()
    {
        if (PlayeurNow.GetComponent<Guardian>().Spell4Selection())
        {
            Selection = "Spell4";
        }

    }
}