using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Manager : NetworkBehaviour
{
    public Sprite Att;
    public Sprite Def;

    public int turn;
    float endNextTurn;

    [SyncVar(hook = "Sync")]
    float DiffTimeTurn;

    void Sync(float t)
    {
        Debug.Log("Sync !!!" + t);
    }

    public bool AbleToDo;
    public GameObject Attacker;
    public GameObject Defender;

    public UnityEngine.UI.Image textPlayerTurn;

    public Text textTimeToPlay;

    public UnityEngine.UI.Image Mana;
    public UnityEngine.UI.Image Vie;

    GameObject PlayeurNow;

    public float turnTime;
    public string Selection;

    // Use this for initialization
    void Start()
    {
        AbleToDo = true;
        Attacker = GameObject.Find("PlayerOhnir(Clone)");
        DontDestroyOnLoad(gameObject);
        Selection = "";
        turn = 0;
        endNextTurn = turnTime;
        PlayeurNow = Attacker;

    }

 public void PassTurn()
    {
        endNextTurn = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Mana.fillAmount = (Attacker.GetComponent<Ohnir>().Endurance / Attacker.GetComponent<Ohnir>().EnduranceMax) / 2;
        Vie.fillAmount = (Attacker.GetComponent<Ohnir>().Hp / Attacker.GetComponent<Ohnir>().HpMax) / 2;

        DiffTimeTurn = endNextTurn - Time.time;
        textTimeToPlay.text = (DiffTimeTurn).ToString();
        if (Time.time >= endNextTurn) //passage au tour suivant
        {

            turn++;
            Attacker.GetComponent<Ohnir>().Endurance += 60 * Attacker.GetComponent<Ohnir>().EnduranceMax / 100;
            if (Attacker.GetComponent<Ohnir>().Endurance > Attacker.GetComponent<Ohnir>().EnduranceMax) Attacker.GetComponent<Ohnir>().Endurance = Attacker.GetComponent<Ohnir>().EnduranceMax;
            endNextTurn += turnTime;

            if (PlayeurNow == Attacker) // changement du joueur 
            {
                PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
                PlayeurNow.GetComponentsInChildren<MeshRenderer>()[3].enabled = false;
                PlayeurNow.GetComponentsInChildren<MeshRenderer>()[4].enabled = false;
                PlayeurNow.GetComponentsInChildren<MeshCollider>()[0].enabled = false;
                GameObject.Find("TurnPlayeur").GetComponent<UnityEngine.UI.Image>().sprite = Def;
                PlayeurNow = Defender;
            }
            else
            {
                /* PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
                 PlayeurNow.GetComponentsInChildren<MeshRenderer>()[3].enabled = false;
                 PlayeurNow.GetComponentsInChildren<MeshRenderer>()[4].enabled = false;
                 PlayeurNow.GetComponentsInChildren<MeshCollider>()[0].enabled = false;*/
                GameObject.Find("TurnPlayeur").GetComponent<UnityEngine.UI.Image>().sprite = Att;
                PlayeurNow = Attacker;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //ONLI FOR OHNIR
            if (Selection == "Spell4")
            {
                Vector3 look = hit.point - PlayeurNow.transform.position;
                look.y = 0;
                PlayeurNow.GetComponentsInChildren<MeshRenderer>()[2].gameObject.transform.rotation = Quaternion.LookRotation(look);
            }

            if (AbleToDo && Input.GetButtonDown("Fire1"))
            {
                if (Selection == "Spell1")
                {
                    PlayeurNow.GetComponentsInChildren<MeshRenderer>()[4].enabled = true;
                    PlayeurNow.GetComponentsInChildren<MeshCollider>()[0].enabled = true;
                    PlayeurNow.GetComponent<Guardian>().Spell1Activation(hit);
                    Selection = "";
                    PlayeurNow.GetComponent<Guardian>().Endurance -= PlayeurNow.GetComponent<Guardian>().Spell1ForEndurance;
                }
                else if (Selection == "Spell2")
                {
                    PlayeurNow.GetComponent<Guardian>().Spell2Activation(hit);
                    Selection = "";
                    PlayeurNow.GetComponent<Guardian>().Endurance -= PlayeurNow.GetComponent<Guardian>().Spell2ForEndurance;
                }
                else if (Selection == "Spell3")
                {
                    PlayeurNow.GetComponent<Guardian>().Spell3Activation(hit);
                    Selection = "";
                    PlayeurNow.GetComponent<Guardian>().Endurance -= PlayeurNow.GetComponent<Guardian>().Spell3ForEndurance;
                }
                else if (Selection == "Spell4")
                {
                    PlayeurNow.GetComponent<Guardian>().Spell4Activation(hit);
                    AbleToDo = false;
                    PlayeurNow.GetComponent<Guardian>().Endurance -= PlayeurNow.GetComponent<Guardian>().Spell4ForEndurance;
                    PlayeurNow.GetComponentsInChildren<MeshRenderer>()[3].enabled = false;
                }

                else if (Selection == "Moove" && PlayeurNow.GetComponent<Guardian>().AbleToMoove )
                {
                    //si la position que le mec a donner est correcte
                    if (CalculRange(PlayeurNow.transform.position, hit.point) <= PlayeurNow.GetComponent<Guardian>().OneMoove && hit.collider.name != "dec(Clone)" && hit.collider.name != "arbre(Clone)")
                    {
                        //PlayeurNow.transform.position = hit.point + Vector3.up;
                        PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
                        PlayeurNow.GetComponent<Guardian>().Endurance -= PlayeurNow.GetComponent<Guardian>().MooveRangeForEndurance;
                        AbleToDo = false;
                        Vector3 look =  hit.point - PlayeurNow.transform.position  ;
                        look.y = 0;
                        PlayeurNow.transform.rotation = Quaternion.LookRotation(look);
                    }
                }
            }
        }

        if (!AbleToDo && Selection == "Moove")
        {
            if (CalculRange(PlayeurNow.transform.position + PlayeurNow.transform.forward * Time.deltaTime * 3, hit.point) > CalculRange(PlayeurNow.transform.position, hit.point))
            {
                AbleToDo = true;
                Selection = "";
            }
            else
            {
                PlayeurNow.transform.position += PlayeurNow.transform.forward * Time.deltaTime * 3;
            }
        }
        else if (!AbleToDo && Selection == "Spell4")
        {
            if (CalculRange(PlayeurNow.transform.position + PlayeurNow.transform.forward * Time.deltaTime * 10, hit.point) > CalculRange(PlayeurNow.transform.position, hit.point))
            {
                PlayeurNow.GetComponent<CapsuleCollider>().enabled = false;
                AbleToDo = true;
                Selection = "";
            }
            else
            {
                PlayeurNow.transform.position += PlayeurNow.transform.forward * Time.deltaTime *  10;
            }
        }
    }

    public static float CalculRange(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2));
    }

    public void Moove()
    { 
        if (AbleToDo && PlayeurNow.GetComponent<Guardian>().Endurance >= PlayeurNow.GetComponent<Guardian>().MooveRangeForEndurance) {
            Selection = "Moove";
            PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = true;
            
            //afficher range de deplacement
        }
    }
    public void Spell1()
    {
        if (AbleToDo &&  PlayeurNow.GetComponent<Guardian>().Spell1Selection())
        {
            PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            PlayeurNow.GetComponentsInChildren<MeshRenderer>()[4].enabled = true;
            Selection = "Spell1";
        }
    }
    public void Spell2()
    {
        if (AbleToDo && PlayeurNow.GetComponent<Guardian>().Spell2Selection())
        {
            PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            Selection = "Spell2";
        }
    }
    public void Spell3()
    {
        if (AbleToDo && PlayeurNow.GetComponent<Guardian>().Spell3Selection())
        {
            PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            Selection = "Spell3";
        }

    }
    public void Spell4()
    {
        if (AbleToDo &&PlayeurNow.GetComponent<Guardian>().Spell4Selection())
        {
            PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
            PlayeurNow.GetComponentsInChildren<MeshRenderer>()[3].enabled = true;
            Selection = "Spell4";
        }

    }
}