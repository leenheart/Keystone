using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ohnir : Guardian {
    
    public Mesh BlowOfHorn;
    public Material RhinoShield;
    public Mesh KeratinArrow;
    public Mesh Charge;

    public GameObject Arrow;


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
        if (Endurance >= 800)
        {
            return true;
        }
        return false;
    }

    /* ________________________________________________________________________________________________________________________________________________________________________________________________ */

    public override void Spell1Activation(RaycastHit hit)
    {

    }
    public override void Spell2Activation(RaycastHit hit)
    {

    }
    public override void Spell3Activation(RaycastHit hit)
    {
       // if (hit.collider.name == "KeratinArrow")
        {
            
            Vector3 look = hit.point - transform.position;
            look.y = 0;
            transform.rotation = Quaternion.LookRotation(look);
            GameObject arrowNow = Instantiate(Arrow, transform.position + transform.forward , transform.rotation);
            Destroy(arrowNow, 5);
            arrowNow.GetComponent<Rigidbody>().AddForce(transform.forward * 100 * Time.deltaTime);
            
        }
    }
    public override void Spell4Activation(RaycastHit hit)
    {

    }

    // Use this for initialization
    void Start()
    {
        HpMax = 1000;
        Hp = 1000;
        Endurance = 1000;
        EnduranceMax = 1000;
        Armor = 10;
        MooveRangeForEndurance = 100;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
