using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Guardian : NetworkBehaviour {


    public float HpMax;
    public float Hp;
    public float Endurance;
    public float EnduranceMax;
    public int Armor;
    public int MooveRangeForEndurance;
    public int OneMoove;
    public int Spell1ForEndurance;
    public int Spell2ForEndurance;
    public int Spell3ForEndurance;
    public int Spell4ForEndurance;
    public bool AbleToMoove;

    public void TakeDammage(int damage)
    {
        Hp -= damage;
    }


    public abstract bool Spell1Selection();
    public abstract bool Spell2Selection();
    public abstract bool Spell3Selection();
    public abstract bool Spell4Selection();

    /* _________________________________________________________________________________________________________________________________________________________________________________ */

    public abstract void Spell1Activation(RaycastHit hit);

    public abstract void Spell2Activation(RaycastHit hit);

    public abstract void Spell3Activation(RaycastHit hit);

    public abstract void Spell4Activation(RaycastHit hit);


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
