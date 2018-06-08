using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public Sprite Att;
    public Sprite Def;

    public Image MooveButton;
    public Image Spell1Button;
    public Image Spell2Button;
    public Image Spell3Button;
    public Image Spell4Button;

    public bool isStart = false;

    public int turn;
    public float endNextTurn;

    float DiffTimeTurn;

    public bool AbleToDo;

    public GameObject Attacker;
    public GameObject Defender;

    public GameObject PlayerAvatar;

    public Image textPlayerTurn;

    public Text textTimeToPlay;

    public Image Mana;
    public Image Vie;

    GameObject PlayeurNow;

    public float turnTime;
    public string Selection;

    public string ColliderName;

    // Use this for initialization
    void Start()
    {
        AbleToDo = true;
        // Attacker = GameObject.Find("PlayerOhnir(Clone)");
        DontDestroyOnLoad(gameObject);
        Selection = "";
        turn = 0;
        endNextTurn = turnTime;
        PlayeurNow = Attacker;

        MooveButton = GameObject.Find("Button deplacement").GetComponent<Image>();
        Spell1Button = GameObject.Find("Spell 1").GetComponent<Image>();
        Spell2Button = GameObject.Find("Spell 2").GetComponent<Image>();
        Spell3Button = GameObject.Find("Spell 3").GetComponent<Image>();
        Spell4Button = GameObject.Find("Spell 4").GetComponent<Image>();

        textPlayerTurn = GameObject.Find("TurnPlayeur").GetComponent<Image>();
        textTimeToPlay = GameObject.Find("Timer").GetComponent<Text>();
        Mana = GameObject.Find("Mana Bar").GetComponent<Image>();
        Vie = GameObject.Find("HP bar").GetComponent<Image>();

    }

    public void PassTurn()
    {
        if (PlayerAvatar == PlayeurNow)
        {
            if (GameObject.Find("Client"))
            {
                GameObject.Find("Client").GetComponent<Client>().PassTurn();
            }
            else
            {
                GameObject.Find("Server").GetComponent<Server>().PassTurn();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isStart) return;

        DiffTimeTurn = endNextTurn - Time.time;
        textTimeToPlay.text = ((int)DiffTimeTurn).ToString();
        if (Time.time >= endNextTurn) //passage au tour suivant
        {

            turn++;

            foreach (GameObject ohnir in GameObject.FindGameObjectsWithTag("Player"))
            {
                ohnir.GetComponent<Ohnir>().Endurance += 60 * ohnir.GetComponent<Ohnir>().EnduranceMax / 100;
                if (ohnir.GetComponent<Ohnir>().Endurance > ohnir.GetComponent<Ohnir>().EnduranceMax) ohnir.GetComponent<Ohnir>().Endurance = ohnir.GetComponent<Ohnir>().EnduranceMax;
            }

            endNextTurn = Time.time + turnTime;

            if (PlayeurNow == Attacker) // changement du joueur 
            {
                PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
                PlayeurNow.GetComponentsInChildren<MeshRenderer>()[3].enabled = false;
                PlayeurNow.GetComponentsInChildren<MeshRenderer>()[4].enabled = false;
                PlayeurNow.GetComponentsInChildren<MeshCollider>()[0].enabled = false;
                Spell1Button.color = Color.white;
                Spell2Button.color = Color.white;
                Spell3Button.color = Color.white;
                Spell4Button.color = Color.white;
                MooveButton.color = Color.white;
                textPlayerTurn.sprite = Def;
                Vector3 vect = GameObject.Find("TurnPlayeur").transform.position;
                vect.x += 600;
                GameObject.Find("TurnPlayeur").transform.position = vect;
                PlayeurNow = Defender;
            }
            else
            {
                PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
                PlayeurNow.GetComponentsInChildren<MeshRenderer>()[3].enabled = false;
                PlayeurNow.GetComponentsInChildren<MeshRenderer>()[4].enabled = false;
                PlayeurNow.GetComponentsInChildren<MeshCollider>()[0].enabled = false;
                Spell1Button.color = Color.white;
                Spell2Button.color = Color.white;
                Spell3Button.color = Color.white;
                Spell4Button.color = Color.white;
                MooveButton.color = Color.white;
                textPlayerTurn.sprite = Att;
                Vector3 vect = GameObject.Find("TurnPlayeur").transform.position;
                vect.x -= 600;
                GameObject.Find("TurnPlayeur").transform.position = vect;
                PlayeurNow = Attacker;
            }
        }

        Mana.fillAmount = (PlayerAvatar.GetComponent<Guardian>().Endurance / PlayerAvatar.GetComponent<Guardian>().EnduranceMax) / 2;
        Vie.fillAmount = (PlayerAvatar.GetComponent<Guardian>().Hp / PlayerAvatar.GetComponent<Guardian>().HpMax) / 2;


        if (PlayerAvatar == PlayeurNow)
        {
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
                    PlayeurNow.GetComponentsInChildren<MeshRenderer>()[2].gameObject.transform.localScale = new Vector3(3, 0.001f, CalculRange(hit.point, PlayeurNow.transform.position) / 20);
                }
                else if (Selection == "Moove")
                {
                    int dist = (int)CalculRange(hit.point, PlayeurNow.transform.position) / 2;
                    if (dist * 10 <= PlayeurNow.GetComponent<Guardian>().OneMoove * PlayeurNow.GetComponent<Guardian>().Endurance)
                        PlayeurNow.GetComponentsInChildren<MeshRenderer>()[1].gameObject.transform.localScale = new Vector3(dist * 10, 0.001f, dist * 10);
                }

                if (AbleToDo && Input.GetButtonDown("Fire1"))
                {
                    if (Selection == "Spell1")
                    {
                        PlayeurNow.GetComponent<Guardian>().Spell1Activation(hit.point);
                        if (GameObject.Find("Client"))
                        {
                            GameObject.Find("Client").GetComponent<Client>().Spell(hit.point, 1);
                        }
                        else
                        {
                            GameObject.Find("Server").GetComponent<Server>().Spell(hit.point, 1);
                        }
                        Spell1Button.color = Color.white;
                        Selection = "";
                    }
                    else if (Selection == "Spell2")
                    {
                        PlayeurNow.GetComponent<Guardian>().Spell2Activation(hit.point);
                        if (GameObject.Find("Client"))
                        {
                            GameObject.Find("Client").GetComponent<Client>().Spell(hit.point, 2);
                        }
                        else
                        {
                            GameObject.Find("Server").GetComponent<Server>().Spell(hit.point, 2);
                        }
                        Spell2Button.color = Color.white;
                        Selection = "";
                    }
                    else if (Selection == "Spell3")
                    {
                        PlayeurNow.GetComponent<Guardian>().Spell3Activation(hit.point);
                        if (GameObject.Find("Client"))
                        {
                            GameObject.Find("Client").GetComponent<Client>().Spell(hit.point, 3);
                        }
                        else
                        {
                            GameObject.Find("Server").GetComponent<Server>().Spell(hit.point, 3);
                        }
                        Spell3Button.color = Color.white;
                        Selection = "";
                    }
                    else if (Selection == "Spell4")
                    {
                        ColliderName = hit.collider.name;
                        PlayeurNow.GetComponent<Guardian>().Spell4Activation(hit.point);
                        if (GameObject.Find("Client"))
                        {
                            GameObject.Find("Client").GetComponent<Client>().Spell(hit.point, 4);
                        }
                        else
                        {
                            GameObject.Find("Server").GetComponent<Server>().Spell(hit.point, 4);
                        }
                        Spell4Button.color = Color.white;
                        Selection = "";
                    }

                    else if (Selection == "Moove")
                    {
                        //Debug.Log((hit.collider.name != "dec(Clone)") + " " + (hit.collider.name != "arbre(Clone)") + " " + PlayeurNow.GetComponent<Guardian>().AbleToMoove + " " + (CalculRange(transform.position, hit.point) <= PlayeurNow.GetComponent<Guardian>().OneMoove));
                        //Debug.Log(CalculRange(transform.position, hit.point)+ " <= " + PlayeurNow.GetComponent<Guardian>().OneMoove);
                        if (hit.collider.name != "dec(Clone)" && hit.collider.name != "arbre(Clone)" && PlayeurNow.GetComponent<Guardian>().AbleToMoove && CalculRange(PlayeurNow.transform.position, hit.point) <= PlayeurNow.GetComponent<Guardian>().OneMoove)
                        {
                            PlayeurNow.GetComponent<Guardian>().Moove(hit.point);
                            if (GameObject.Find("Client"))
                            {
                                GameObject.Find("Client").GetComponent<Client>().Moove(hit.point);
                            }
                            else
                            {
                                GameObject.Find("Server").GetComponent<Server>().Moove(hit.point);
                            }
                        }
                        MooveButton.color = Color.white;
                        Selection = "";
                    }
                }
            }
        }

    }

    public static float CalculRange(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Sqrt(Mathf.Pow(pos1.x - pos2.x, 2) + Mathf.Pow(pos1.y - pos2.y, 2));
    }

    public void Moove()
    {
        if (PlayerAvatar == PlayeurNow)
        {
            PlayeurNow.GetComponent<Guardian>().MooveSelection();
            MooveButton.color = Color.green;
            Selection = "Moove";
        }
    }
    public void Spell1()
    {
        if (PlayerAvatar == PlayeurNow && AbleToDo && PlayeurNow.GetComponent<Guardian>().Spell1Selection())
        {
            Selection = "Spell1";
            Spell1Button.color = Color.green;
        }
    }
    public void Spell2()
    {
        if (PlayerAvatar == PlayeurNow && AbleToDo && PlayeurNow.GetComponent<Guardian>().Spell2Selection())
        {
            Selection = "Spell2";
            Spell2Button.color = Color.green;
        }
    }
    public void Spell3()
    {
        if (PlayerAvatar == PlayeurNow && AbleToDo && PlayeurNow.GetComponent<Guardian>().Spell3Selection())
        {
            Selection = "Spell3";
            Spell3Button.color = Color.green;
        }

    }
    public void Spell4()
    {
        if (PlayerAvatar == PlayeurNow && AbleToDo && PlayeurNow.GetComponent<Guardian>().Spell4Selection())
        {
            Selection = "Spell4";
            Spell4Button.color = Color.green;
        }

    }
}