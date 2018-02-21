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
    private string role;

    public Personnage()
    {
        name = "Personnage";
        hp = 1000;
        mana = 100;
        regenMana = 25;
        nbMvt = 5;
        attack = 100;
        armure = 5;
        role = "attaquant";
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

    public string GetRole()
    {
        return role;
    }

}
