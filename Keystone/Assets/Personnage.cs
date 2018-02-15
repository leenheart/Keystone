using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Personnage : MonoBehaviour
{

    private string name;
    private int hp;
    private int mana;
    private int regenMana;
    private int nbMvt;
    private int attack;
    private int armure;

    // Use this for initialization
    void Start()
    {
        name = "Personnage";
        hp = 1000;
        mana = 100;
        regenMana = 25;
        nbMvt = 5;
        attack = 100;
        armure = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Move(Vector2 movement)
    {
        transform.Translate(movement * Time.deltaTime);
    }

    void TakeDommage(int dommage)
    {
        hp -= dommage * armure / 100;
    }

    int Attack()
    {
        return attack;
    }

}
