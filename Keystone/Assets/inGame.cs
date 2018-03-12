using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class inGame : MonoBehaviour
{

    private float finChooseAct;
    private float debutChooseAct;
    private float timeToChoseAct;
    private float timeToDoAct;

    public string etape;
    public enum PlayeurTurn { attaquant, defenseur };
    public PlayeurTurn playeurTurn;
    private int turn;

    public static bool passturn;
    public static bool start;

    public Player playerAttaquant;
    public Player playerDefender;

    public Queue sauvegarde_actions;

    public Text textEtape;
    public Text textPlayerturn;
    public Text textTimer;

    public static Vector3 posAttaquant;
    public static Vector3 posDefenseur;



    // Use this for initialization
    void Start()
    {
        posAttaquant = Vector3.zero;
        posDefenseur = Vector3.zero;

        passturn = false;
        start = false;
        turn = 1;

        textEtape = GameObject.FindGameObjectWithTag("TextStep").GetComponent<Text>();
        textPlayerturn = GameObject.FindGameObjectWithTag("TextPlayerTurn").GetComponent<Text>();
        textTimer = GameObject.FindGameObjectWithTag("TextTimer").GetComponent<Text>();


        finChooseAct = 5f;
        debutChooseAct = 1f;
        etape = "choisir actions";
        playeurTurn = PlayeurTurn.attaquant;
        timeToChoseAct = 5f;
        timeToDoAct = 1f;

    }

    // Update is called once per frame
    void Update()
    {
       

        if (Player.pret)
        {
            start = Player.pret;

            //GameObject.FindGameObjectWithTag("ButtonReady").active = false;
        }

        if (start)
        {
            textTimer.text = (Mathf.Abs(Time.time - finChooseAct) * 1000 / 1000).ToString();

            if (etape == "choisir actions" && Time.time > finChooseAct || passturn)
            {
                passturn = false;
                etape = "actions";
                textEtape.text = "actions";
                finChooseAct = Time.time + timeToChoseAct + timeToDoAct;
                debutChooseAct = Time.time + timeToDoAct;
            }

            if (etape == "actions" && Time.time > debutChooseAct)
            {
                etape = "choisir actions";
                textEtape.text = "choisir actions";

                if (playeurTurn == PlayeurTurn.attaquant)
                {
                    textPlayerturn.text = "defender";
                    playeurTurn = PlayeurTurn.defenseur;
                }

                else
                {
                    playeurTurn = PlayeurTurn.attaquant;
                    textPlayerturn.text = "attacker";
                }
                
                //Debug.Log(etape + " " + Time.time);
            }
        }
        else
        {

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
