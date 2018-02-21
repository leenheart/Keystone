using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    private string pseudo;

    public Personnage personnage;
    public inGame inGameObject;

    void awake()
    {
        inGameObject = GameObject.FindGameObjectWithTag("inGame").GetComponent<inGame>();
    }

    public Player()
    {
        pseudo = "Personnage";

    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
        {
            return;
        }
        //seul le joueur a qui appartient le gameobject pourra faire :

        if (inGameObject.getEtape() == "choisir actions" && inGameObject.getPlayeurTurn() == personnage.GetRole()) 
        {
            //si c a moi de jouer et que je dois choisir :
            Debug.Log("c'est a moi de jouer !!!" + Time.time);
        }

        if (inGameObject.getEtape() == "actions" && inGameObject.getPlayeurTurn() == personnage.GetRole())
        {
            //faire toutes les actions putaain
        }
    }

}
