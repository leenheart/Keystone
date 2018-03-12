using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{

    private string pseudo;
    public Camera cam;
    public Personnage personnage;
    public inGame inGameObject;
    public Queue sauvegarde_actions;

    public GameObject rangeDep;

    public GameObject GOspell1;
    public GameObject GOspell2;
    public GameObject GOspell3;
    public GameObject GOspell4;

    public Image hpBarre;
    public Image manaBarre;

    public static bool pret;
    public static bool pass;
    public static bool moove;

    public static bool spell1;
    public static bool spell2;
    public static bool spell3;
    public static bool spell4;


    // Use this for initialization
    void Start()
    {
        pseudo = "Personnage";
        sauvegarde_actions = new Queue();
        cam = GetComponent<Camera>() as Camera;

        rangeDep = GameObject.FindGameObjectWithTag("Rangedep");

        inGameObject = GameObject.FindGameObjectWithTag("inGame").GetComponent<inGame>();
        personnage = new Personnage(transform.position);

        GOspell1 = GameObject.FindGameObjectWithTag("Doovmar spell1");
        GOspell2 = GameObject.FindGameObjectWithTag("Doovmar spell2");
        GOspell3 = GameObject.FindGameObjectWithTag("Doovmar spell3");
        GOspell4 = GameObject.FindGameObjectWithTag("Doovmar spell4");

        
        pass = false;

        hpBarre = GameObject.FindGameObjectWithTag("HpBarre").GetComponent<Image>();
        manaBarre = GameObject.FindGameObjectWithTag("ManaBarre").GetComponent<Image>();

        moove = false;

        spell1 = false;
        spell2 = false;
        spell3 = false;
        spell4 = false;
    }

    public float Distance(Vector3 dep1, Vector3 dep2)
    {
        return Mathf.Sqrt(Mathf.Pow(dep1.x - dep2.x, 2) + Mathf.Pow(dep1.y - dep2.y, 2));
    }

    public bool DeplacementOk(Vector3 deplacement)
    {
        Vector3 position = transform.position;
 
        if (Distance(deplacement, position) <= 5) return true;

        return false;
        
    }

    // La fonction qui renvoie le vecteur entre la postion du personnage et du click (curseur)
    public void Pass()
    {
        //if(inGameObject.getEtape() == "choisir actions" )//&& inGameObject.getPlayeurTurn() == personnage.GetRole())
        pass = true;
    }

    public void Ready()
    {
        pret = true;
        Debug.Log("pret");
    }

    public void Moove()
    {
        //if (inGameObject.getEtape() == "choisir actions" )//&& inGameObject.getPlayeurTurn() == personnage.GetRole())
            moove = true;
    }

    public void Spell1()
    {
        //if (inGameObject.getEtape() == "choisir actions" )//&& inGameObject.getPlayeurTurn() == personnage.GetRole())
            spell1 = true;
    }
    public void Spell2()
    {
        //if (inGameObject.getEtape() == "choisir actions")// && inGameObject.getPlayeurTurn() == personnage.GetRole())
            spell2 = true;
    }
    public void Spell3()
    {
       // if (inGameObject.getEtape() == "choisir actions" && inGameObject.getPlayeurTurn() == personnage.GetRole())
            spell3 = true;
    }
    public void Spell4()
    {
        //if (inGameObject.getEtape() == "choisir actions" && inGameObject.getPlayeurTurn() == personnage.GetRole())
            spell4 = true;
    }

    // Update is called once per frame
    void Update()
    {

        hpBarre.fillAmount = personnage.getHp();
        manaBarre.fillAmount = personnage.getMana();

        if (isLocalPlayer && inGame.start)
        {


            //seul le joueur a qui appartient le gameobject pourra faire :

            if (inGameObject.getEtape() == "choisir actions" && inGameObject.getPlayeurTurn() == personnage.GetRole())
            {
                if (pass)
                {
                    inGame.passturn = true;
                    pass = false;
                }
                

                if (moove)
                {
                    rangeDep.transform.position = transform.position + Vector3.down;
                    rangeDep.GetComponent<Renderer>().enabled = true;
                }
                if (spell1)
                {
                    GOspell1.GetComponent<Renderer>().enabled = true;
                    GOspell1.transform.position = transform.position + Vector3.down;
                    
                }
                if (spell2)
                {
                    GOspell2.GetComponent<Renderer>().enabled = true;
                    GOspell2.transform.position = transform.position + Vector3.down;

                }
                if (spell3)
                {
                    GOspell3.GetComponent<Renderer>().enabled = true;
                    GOspell3.transform.position = transform.position + Vector3.down;
                }
                if (spell4)
                {
                    GOspell4.GetComponent<Renderer>().enabled = true;
                    GOspell4.transform.position = transform.position + Vector3.down;
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 position = hit.point;
                        if (moove && DeplacementOk(position))
                        {
                           
                            rangeDep.GetComponent<Renderer>().enabled = false;
                            Action deplacement = new Action(Action.Type.deplacement, hit.point);
                            sauvegarde_actions.Enqueue(deplacement);
                            moove = false;
                        }
                        if (spell1 && personnage.Spell1Ok(position))
                        {
                            GOspell1.GetComponent<Renderer>().enabled = false;
                            Action action = new Action(Action.Type.spell1, hit.point);
                            sauvegarde_actions.Enqueue(action);
                            spell1 = false;
                        }
                        if (spell2 && personnage.Spell2Ok(position))
                        {
                            GOspell2.GetComponent<Renderer>().enabled = false;
                            Action action = new Action(Action.Type.spell2, hit.point);
                            sauvegarde_actions.Enqueue(action);
                            spell2 = false;
                        }
                        if (spell3 && personnage.Spell3Ok(position))
                        {
                            GOspell3.GetComponent<Renderer>().enabled = false;
                            Action action = new Action(Action.Type.spell3, hit.point);
                            sauvegarde_actions.Enqueue(action);
                            spell3 = false;
                        }
                        if (spell4 && personnage.Spell4Ok(position))
                        {
                            GOspell4.GetComponent<Renderer>().enabled = false;
                            Action action = new Action(Action.Type.spell4, hit.point);
                            sauvegarde_actions.Enqueue(action);
                            spell4 = false;
                        }
                    }

                }

            }

            if (rangeDep.GetComponent<Renderer>().enabled && inGameObject.getEtape() == "actions")
            {
                rangeDep.GetComponent<Renderer>().enabled = false;
                GOspell1.GetComponent<Renderer>().enabled = false;
                GOspell2.GetComponent<Renderer>().enabled = false;
                GOspell3.GetComponent<Renderer>().enabled = false;
                GOspell4.GetComponent<Renderer>().enabled = false;
            }
            if (inGameObject.getEtape() == "actions" && inGameObject.getPlayeurTurn() == personnage.GetRole() && sauvegarde_actions.Count > 0)
            {

                Action act = sauvegarde_actions.Dequeue() as Action;
                if (act.type == Action.Type.deplacement && DeplacementOk(act.coordonnees))
                transform.position = act.coordonnees + Vector3.up;
                if (act.type == Action.Type.spell1)
                {

                }

            }
        }
    }
}

