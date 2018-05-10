﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    GameObject playeur;
    public bool Attacker;
    public bool Defender;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBtn(string PlayScene)
    {
        SceneManager.LoadScene(PlayScene);
    }

    public void ExitBtn()
    {
        Application.Quit();
    }

    public void ActivateGameObject(GameObject o)
    {
        DontDestroyOnLoad(o);
        o.SetActive(true);
    }



    public void GenerateMap()
    {
        if (GameObject.Find("Client"))
        {
        GameObject.Find("Client").GetComponent<Client>().GenerateMap();
        }
        else
        {
            GameObject.Find("MapGeneration 1").GetComponent<Generation>().Generate();
        }

    }

    public void Ready()
    {
        if (GameObject.Find("Client"))
        {
            GameObject.Find("Client").GetComponent<Client>().Ready();
        }
        else
        {
            GameObject.Find("Server").GetComponent<Server>().Ready();
        }

    }

    public void SetPlayeurGuardian(GameObject pl)
    {

        playeur = pl;
        DontDestroyOnLoad(playeur);

    }

    public void att()
    {
        Attacker = true;
    }
    public void def()
    {
        Defender = true;
    }

    public void Connect()
    {
        GameObject.Find("Client").GetComponent<Client>().Connect();
    }

}