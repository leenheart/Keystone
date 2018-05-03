using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ohnir : Guardian {


    [SyncVar]
    public bool Attacker = false;
    [SyncVar]
    public bool Defender = false;

    public GameObject Arrow;

    Quaternion pos;

    [Command]
    public void CmdInitiate(Vector3 v)
    {
        if (isLocalPlayer)
        {
            gameObject.transform.position = v;
            //gameObject.transform.rotation = new Quaternion(0,0,90,0);
        }
    }

    public override bool Spell1Selection()
    {
        if (Endurance >= 100)
        {
            return true;
        }
        return false;
    }

    public override bool Spell2Selection()
    {
        if (Endurance >= 200)
        {
            return true;
        }
        return false;
    }
    public override bool Spell3Selection()
    {
        if (Endurance >= 200)        
        {  
            return true;
        }
        return false;
    }
    public override  bool Spell4Selection()
    {
        if (Endurance >= 700)
        {
            return true;
        }
        return false;
    }

    /* ________________________________________________________________________________________________________________________________________________________________________________________________ */

    public override void Spell1Activation(RaycastHit hit)
    {
        GetComponentsInChildren<MeshRenderer>()[4].enabled = false;
    }
    public override void Spell2Activation(RaycastHit hit)
    {
        if (AbleToMoove)
        {
            Armor += 60 * Armor / 100;
            AbleToMoove = false;
        }
        else
        {
            Armor -= 60 * Armor / 100;
            AbleToMoove = true;
        }
    }
    public override void Spell3Activation(RaycastHit hit)
    {
    
            
        Vector3 look = hit.point - transform.position;
        look.y = 0;
        transform.rotation = Quaternion.LookRotation(look);
        GameObject arrowNow = Instantiate(Arrow, transform.position + transform.forward , transform.rotation);
        Destroy(arrowNow, 5);
        arrowNow.GetComponent<Rigidbody>().AddForce(transform.forward * 100 * Time.deltaTime);
            
      
    }

    void OnCollisionEnter(Collision collider)
    {
        Debug.Log(collider.gameObject.name);
        if (collider.gameObject.name == "arbre(Clone)" || collider.gameObject.name == "dec(Clone)" || collider.gameObject.name == "Castle")
        {
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.name == "Playeur")
        {
            collider.gameObject.GetComponent<Guardian>().TakeDammage(250);
        }
    }

    public override void Spell4Activation(RaycastHit hit)
    {
        if (hit.collider.name != "dec(Clone)" && hit.collider.name != "arbre(Clone)")
        {
            Debug.Log("Spell4");
            //PlayeurNow.transform.position = hit.point + Vector3.up;
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponentsInChildren<MeshRenderer>()[3].enabled = false;
            Vector3 look = hit.point - transform.position;
            look.y = 0;
            transform.rotation = Quaternion.LookRotation(look);
        
        }
    }

    // Use this for initialization
    void Start()
    {
        pos = transform.rotation;
        DontDestroyOnLoad(gameObject);
        HpMax = 1000;
        Hp = 1000;
        Endurance = 1000;
        EnduranceMax = 1000;
        Armor = 10;
        MooveRangeForEndurance = 100;
        OneMoove = 2;
        Spell1ForEndurance = 100;
        Spell2ForEndurance = 200;
        Spell3ForEndurance = 200;
        Spell4ForEndurance = 700;
        AbleToMoove = true;
    }



    [Command]
    public void CmdSpawn()
    {
        DontDestroyOnLoad(gameObject);
        //Spawn the GameObject you assign in the Inspector
        NetworkServer.Spawn(gameObject);
    }
}
