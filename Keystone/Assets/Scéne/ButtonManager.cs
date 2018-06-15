using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    GameObject playeur;

    public GameObject Ohnir;
    public GameObject Guemnaar;

    public GameObject spell1;
    public GameObject spell2;
    public GameObject spell3;
    public GameObject spell4;

    public Sprite Spell1G;
    public Sprite Spell2G;
    public Sprite Spell3G;
    public Sprite Spell4G;

    public GameObject me;

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
        if (PlayScene == "menu")
        {
            Restart();
        }
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

    public void DeActivateGameObject(GameObject o)
    {
        o.SetActive(false);
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
        else if (GameObject.Find("Server"))
        {
            GameObject.Find("Server").GetComponent<Server>().Ready();
        }
        else
        {
            GameObject.Find("Manager").GetComponent<Manager>().enabled = true;
            foreach (var g in GameObject.FindGameObjectsWithTag("Player"))
            {
                g.GetComponent<Rigidbody>().isKinematic = false;
                if (g.GetComponent<Guardian>().IsIA)
                {
                    GameObject.Find("Manager").GetComponent<Manager>().Defender = g;
                }
                else
                {
                    GameObject.Find("Manager").GetComponent<Manager>().Attacker = g;
                    GameObject.Find("Manager").GetComponent<Manager>().PlayerAvatar = g;
                }
            }
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
            GameObject.Find("Attt").GetComponent<Image>().enabled = true;
            GameObject.Find("Defff").GetComponent<Image>().enabled = true;
        }
        else if (GameObject.Find("Server"))
        {
            GameObject.Find("Server").GetComponent<Server>().me = GameObject.Find("Server").GetComponent<Server>().playerOhnir;
            GameObject.Find("Ohnir Deff").GetComponent<Image>().enabled = true;
            GameObject.Find("Guemnnar Deff").GetComponent<Image>().enabled = false;
            GameObject.Find("Server").GetComponent<Server>().SendMeSelect("Ohnir Deff", "Guemnnar Deff");
            GameObject.Find("Attt").GetComponent<Image>().enabled = true;
            GameObject.Find("Defff").GetComponent<Image>().enabled = true;

        }
        //IA
        else
        {
            GameObject.Find("Ohnir Att").GetComponent<Image>().enabled = true;
            GameObject.Find("Ohnir Deff").GetComponent<Image>().enabled = true;
            GameObject.Find("Guemnnar Att").GetComponent<Image>().enabled = false;
            GameObject.Find("LockAndStart").GetComponent<Image>().enabled = true;

            me = Ohnir;
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
        else if (GameObject.Find("Server"))
        {
            GameObject.Find("Server").GetComponent<Server>().me = GameObject.Find("Server").GetComponent<Server>().playerGuemnaar;
            GameObject.Find("Ohnir Deff").GetComponent<Image>().enabled = false;
            GameObject.Find("Guemnnar Deff").GetComponent<Image>().enabled = true;
            GameObject.Find("Server").GetComponent<Server>().SendMeSelect("Guemnnar Deff", "Ohnir Deff");
        }
        //IA
        else
        {
            GameObject.Find("Guemnnar Att").GetComponent<Image>().enabled = true;
            GameObject.Find("Ohnir Deff").GetComponent<Image>().enabled = true;
            GameObject.Find("Ohnir Att").GetComponent<Image>().enabled = false;
            GameObject.Find("LockAndStart").GetComponent<Image>().enabled = true;
            me = Guemnaar;
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
        else if (GameObject.Find("Server") && GameObject.Find("Server").GetComponent<Server>().me)
        {
            GameObject.Find("Server").GetComponent<Server>().SpawnPlayer(GameObject.Find("Server").GetComponent<Server>().me, GameObject.Find("Server").GetComponent<Server>().hostId);
            GameObject.Find("Server").GetComponent<Server>().SendMe();
            PlayBtn("inGame");
        }
        else
        {
            GameObject g = (GameObject)Instantiate(Resources.Load("MapGeneration 1"), new Vector3(0, 0, 0), new Quaternion());
            DontDestroyOnLoad(g);
            g.GetComponent<Generation>().GenerateMap();
            g.GetComponent<Generation>().Generate();
            g.GetComponent<Generation>().generateObstacle();

            PlayBtn("inGame");

            g = Instantiate(Ohnir, new Vector3(48, 3, 25), Quaternion.Euler(0, -90, 0));
            DontDestroyOnLoad(g);
            g.GetComponent<Guardian>().IsIA = true;
            g.GetComponentsInChildren<SpriteRenderer>()[0].enabled = false;
            g.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;

            GameObject gg = Instantiate(me, new Vector3(2, 3, 25), Quaternion.Euler(0, 90, 0));
            DontDestroyOnLoad(gg);
            
            gg.GetComponentsInChildren<SpriteRenderer>()[0].enabled = true;
            gg.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
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

    public void DoBruit()
    {
        GameObject.Find("Music").GetComponent<Music>().ButtonSort();
    }

    public static void Restart()
    {
        SceneManager.LoadScene("menu");
        foreach (var g in FindObjectsOfType<GameObject>())
        {
            if (!g) return;
            if (g.scene.name != "menu")
            {
                DestroyImmediate(g);
            }
            
        }
        SceneManager.LoadScene("menu");
    }
}