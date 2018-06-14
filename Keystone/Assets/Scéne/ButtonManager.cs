using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    GameObject playeur;
    public bool Attacker;
    public bool Defender;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void IpRemplir()
    {
        string s = GameObject.Find("IpText").GetComponent<Text>().text;
        if (s != "" && s.Length > 7 && s.Length <= 10)
        {
            GameObject.Find("Find Match").GetComponent<Button>().enabled = true;
            if (GameObject.Find("Client"))
            {
                GameObject.Find("Client").GetComponent<Client>().Ip = s;
            }
        }
        else
        {
            GameObject.Find("Find Match").GetComponent<Button>().enabled = false;
        }
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

    public void ActivateGameObjectString(string s)
    {
        GameObject.Find(s).GetComponent<Image>().enabled = true;
    }


    public void GenerateMapClient()
    {
        if (GameObject.Find("Client"))
        {
            GameObject.Find("Client").GetComponent<Client>().GenerateMap();
        }

    }

    public void GenerateMapServer()
    {
        if (GameObject.Find("Server"))
        {
            GameObject.Find("MapGeneration 1").GetComponent<Generation>().Generate();
            GameObject.Find("MapGeneration 1").GetComponent<Generation>().generateObstacle();
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
        GameObject.Find("Button Ready").SetActive(false);
    }

    public void SetPlayerOhnir()
    {
        if (GameObject.Find("Client"))
        {
            GameObject.Find("Client").GetComponent<Client>().me = GameObject.Find("Client").GetComponent<Client>().playerOhnir;
            GameObject.Find("Ohnir Att").GetComponent<Image>().enabled = true;
            GameObject.Find("Guemnnar Att").GetComponent<Image>().enabled = false;
            GameObject.Find("Client").GetComponent<Client>().SendMeSelect("Ohnir Att", "Guemnnar Att");
        }
        else
        {
            GameObject.Find("Server").GetComponent<Server>().me = GameObject.Find("Server").GetComponent<Server>().playerOhnir;
            GameObject.Find("Ohnir Deff").GetComponent<Image>().enabled = true;
            GameObject.Find("Guemnnar Deff").GetComponent<Image>().enabled = false;
            GameObject.Find("Server").GetComponent<Server>().SendMeSelect("Ohnir Deff", "Guemnnar Deff");

        }
    }

    public void SetPlayerGuemnaar()
    {
        if (GameObject.Find("Client"))
        {
            GameObject.Find("Client").GetComponent<Client>().me = GameObject.Find("Client").GetComponent<Client>().playerGuemnaar;
            GameObject.Find("Ohnir Att").GetComponent<Image>().enabled = false;
            GameObject.Find("Guemnnar Att").GetComponent<Image>().enabled = true;
            GameObject.Find("Client").GetComponent<Client>().SendMeSelect("Guemnnar Att", "Ohnir Att");
        }
        else
        {
            GameObject.Find("Server").GetComponent<Server>().me = GameObject.Find("Server").GetComponent<Server>().playerGuemnaar;
            GameObject.Find("Ohnir Deff").GetComponent<Image>().enabled = false;
            GameObject.Find("Guemnnar Deff").GetComponent<Image>().enabled = true;
            GameObject.Find("Server").GetComponent<Server>().SendMeSelect("Guemnnar Deff", "Ohnir Deff");
        }
    }

    public void LockPlayer()
    {
        if (GameObject.Find("Client") && GameObject.Find("Client").GetComponent<Client>().me)
        {
            GameObject.Find("Client").GetComponent<Client>().SpawnPlayer(GameObject.Find("Client").GetComponent<Client>().me, GameObject.Find("Client").GetComponent<Client>().ourClientId);
            GameObject.Find("Client").GetComponent<Client>().SendMe();
            PlayBtn("inGame");
        }
        else if (GameObject.Find("Server").GetComponent<Server>().me)
        {
            GameObject.Find("Server").GetComponent<Server>().SpawnPlayer(GameObject.Find("Server").GetComponent<Server>().me, GameObject.Find("Server").GetComponent<Server>().hostId);
            GameObject.Find("Server").GetComponent<Server>().SendMe();
            PlayBtn("inGame");
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