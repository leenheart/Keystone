using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine;

public class inGame : MonoBehaviour
{

    private float finChooseAct;
    private float debutChooseAct;
    private float timeToChoseAct;
    private float timeToDoAct;

    private string etape;
    public enum PlayeurTurn { attaquant, defenseur };
    public PlayeurTurn playeurTurn;
    private int turn;

    public Player playerAttaquant;
    public Player playerDefender;

    public Queue sauvegarde_actions;



    // Use this for initialization
    void Start()
    {

        turn = 1;

        finChooseAct = 10f;
        debutChooseAct = 5f;
        etape = "choisir actions";
        playeurTurn = PlayeurTurn.attaquant;
        timeToChoseAct = 10f;
        timeToDoAct = 5f;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(etape + " " + playeurTurn);
        if (etape == "choisir actions" && Time.time > finChooseAct /*&& bouton passé*/)
        {
            
            etape = "actions";

            finChooseAct = Time.time + timeToChoseAct + timeToDoAct;
            debutChooseAct = Time.time + timeToDoAct;
            Debug.Log(etape);
        }

        if (etape == "actions" && Time.time > debutChooseAct)
        {
            etape = "choisir actions";
            if (playeurTurn == PlayeurTurn.attaquant)
            playeurTurn = PlayeurTurn.defenseur;
            else playeurTurn = PlayeurTurn.attaquant;
            //Debug.Log(etape + " " + Time.time);
        }





    }

    public string getEtape()
    {
        return etape;
    }

    public PlayeurTurn getPlayeurTurn()
    {
        return playeurTurn;
    }
}
