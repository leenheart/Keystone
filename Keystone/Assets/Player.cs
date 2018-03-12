using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{

    private string pseudo;
    public Camera cam;
    public Personnage personnage;
    public inGame inGameObject;
    public Queue sauvegarde_actions;
    public GameObject rangeDep;


    // Use this for initialization
    void Start()
    {
        pseudo = "Personnage";
        sauvegarde_actions = new Queue();
        cam = GetComponent<Camera>() as Camera;
        rangeDep = GameObject.FindGameObjectWithTag("Rangedep");
        inGameObject = GameObject.FindGameObjectWithTag("inGame").GetComponent<inGame>();
        personnage = new Personnage(transform.position);
    }

    // La fonction qui renvoie le vecteur entre la postion du personnage et du click (curseur)


    // Update is called once per frame
    void Update()
    {

        if (isLocalPlayer)
        {

            rangeDep.transform.position = transform.position + Vector3.down;
            rangeDep.GetComponent<Renderer>().enabled = true;

            //seul le joueur a qui appartient le gameobject pourra faire :

            if (inGameObject.getEtape() == "choisir actions" && inGameObject.getPlayeurTurn() == personnage.GetRole())
            {
                //si c a moi de jouer et que je dois choisir :
                //Debug.Log("c'est a moi de jouer !!!" + Time.time);

                // Lancer un sort

                if ((Input.GetKeyDown(KeyCode.A)))
                {
                    // Lance le sort 1
                }
                if ((Input.GetKeyDown(KeyCode.Z)))
                {
                    // Lance le sort 2
                }
                if ((Input.GetKeyDown(KeyCode.E)))
                {
                    // Lance le sort 3
                }
                if ((Input.GetKeyDown(KeyCode.R)))
                {
                    // Lance le sort 4

                }

                // déplacement

                if (Input.GetButtonDown("Fire1"))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {

                        Action deplacement = new Action(Action.Type.deplacement, hit.point);
                        sauvegarde_actions.Enqueue(deplacement);

                    }

                }

            }

            if (inGameObject.getEtape() == "actions" && inGameObject.getPlayeurTurn() == personnage.GetRole() && sauvegarde_actions.Count > 0)

            {
                rangeDep.GetComponent<Renderer>().enabled = false;
                Action act = sauvegarde_actions.Dequeue() as Action;
                transform.position = act.coordonnees + Vector3.up;
            }
        }

    }
}

