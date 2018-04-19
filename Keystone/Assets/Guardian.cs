using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Guardian : MonoBehaviour {


    public int HpMax;
    public int Hp;
    public int Endurance;
    public int EnduranceMax;
    public int Armor;
    public int MooveRangeForEndurance;

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
