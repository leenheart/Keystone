using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

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
        o.SetActive(true);
    }

    public void GenerateMap()
    {
        GameObject.Find("MapGeneration 1").GetComponent<Generation>().Generate();
    }

    public void SetPlayeurGuardian(GameObject pl)
    {
        /* GameObject player = GameObject.Find("PlayerVirtual(Clone)");
         if (guardian == "Ohnir")
         {
             DontDestroyOnLoad(player);
             player.AddComponent<Ohnir>();
             player.transform.position = new Vector3(2, 1, 20);
            // player.GetComponent<Rigidbody>().isKinematic = false;
         }*/

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

    public void Lock()
    {
        foreach (var c in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (GameObject.Find("Button Manager save").GetComponent<ButtonManager>().Attacker)
            {
                c.GetComponent<Ohnir>().Attacker = true;
                c.GetComponent<Ohnir>().CmdInitiate(new Vector3(2, 1.3f, 20));
            }
            else if ((GameObject.Find("Button Manager save").GetComponent<ButtonManager>().Defender))
            {
                c.GetComponent<Ohnir>().Defender = true;
                c.GetComponent<Ohnir>().CmdInitiate(new Vector3(20, 1.3f, 20));
            }
           
        }
    }


    /* public void CmdSpawnGuardian(GameObject p)
     {
         GameObject mana = GameObject.Find("Manager");
         mana.GetComponent<NetworkIdentity>().AssignClientAuthority(GetComponent<NetworkIdentity>().connectionToClient);
         mana.GetComponent<Manager>().CmdSpawn(p);
     }*/

}