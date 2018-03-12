using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personnage : MonoBehaviour
{

    private string name;
    private int hp;
    private int mana;
    private int regenMana;
    private int nbMvt;
    private int attack;
    private int armure;
    private inGame.PlayeurTurn role;

    public Personnage(Vector3 position)
    {
        name = "Personnage";
        hp = 1000;
        mana = 100;
        regenMana = 25;
        nbMvt = 5;
        attack = 100;
        armure = 5;
        if (position.x < 10)
            role = inGame.PlayeurTurn.defenseur;
        if (position.x >= 10) role = inGame.PlayeurTurn.attaquant;
    }

    public Personnage(string name, int hp,int mana, int regenMana, int nbMvt, int attack, int armure, Vector3 position)
    {
        this.name = name;
        this.hp = hp;
        this.mana = mana;
        this.regenMana = regenMana;
        this.nbMvt = nbMvt;
        this.attack = attack;
        this.armure = armure;
        if (position.x < 10 )
        role = inGame.PlayeurTurn.defenseur;
        if (position.x >= 10) role = inGame.PlayeurTurn.attaquant;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector2 movement)
    {
        transform.Translate(movement * Time.deltaTime);
    }

    public void TakeDommage(int dommage)
    {
        hp -= dommage * armure / 100;
        if (hp <= 0)
        {
            Die();
        }
    }

    public int Attack()
    {
        return attack;
    }

    public void Die()
    {
        //FIXME
    }

    public inGame.PlayeurTurn GetRole()
    {
        return role;
    }

}
