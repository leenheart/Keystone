using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject rangeSpell1;
    public GameObject rangeSpell2;
    public GameObject rangeSpell3;
    public GameObject rangeSpell4;

    public Vector3 vectRangeSpell1;
    public Vector3 vectRangeSpell2;
    public Vector3 vectRangeSpell3;
    public Vector3 vectRangeSpell4;


    public Personnage(Vector3 position)
    {
        name = "Doovmar";
        hp = 200;
        mana = 100;
        regenMana = 25;
        nbMvt = 5;
        attack = 100;
        armure = 5;

        role = inGame.PlayeurTurn.attaquant;

        if (position.x < 10)
        {
            role = inGame.PlayeurTurn.defenseur;
        }
        if (position.x >= 10)
        {
            role = inGame.PlayeurTurn.attaquant;
        }

        rangeSpell1 = Resources.Load(name + " spell1") as GameObject;
        Instantiate(rangeSpell1, Vector3.zero, Quaternion.identity);
        GameObject.FindGameObjectWithTag("Doovmar spell1").GetComponent<Renderer>().enabled = false;

        rangeSpell2 = Resources.Load(name + " spell2") as GameObject;
        Instantiate(rangeSpell2, Vector3.zero, Quaternion.identity);
        GameObject.FindGameObjectWithTag("Doovmar spell2").GetComponent<Renderer>().enabled = false;

        rangeSpell3 = Resources.Load(name + " spell3") as GameObject;
        Instantiate(rangeSpell3, Vector3.zero, Quaternion.identity);
        GameObject.FindGameObjectWithTag("Doovmar spell3").GetComponent<Renderer>().enabled = false;

        rangeSpell4 = Resources.Load(name + " spell4") as GameObject;
        Instantiate(rangeSpell4, Vector3.zero, Quaternion.identity);
        GameObject.FindGameObjectWithTag("Doovmar spell4").GetComponent<Renderer>().enabled = false;


        vectRangeSpell1 = Vector3.zero;
        vectRangeSpell2 = Vector3.zero;
        vectRangeSpell3 = Vector3.zero;
        vectRangeSpell4 = Vector3.zero;

       /* if (role == inGame.PlayeurTurn.attaquant)
        {
            inGame.posAttaquant = transform.position;
        }
        if (role == inGame.PlayeurTurn.defenseur)
        {
            inGame.posDefenseur = transform.position;
        }*/
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
        role = inGame.PlayeurTurn.attaquant;

        if (position.x < 10)
        {
            role = inGame.PlayeurTurn.defenseur;
        }
        if (position.x >= 10)
        {
            role = inGame.PlayeurTurn.attaquant;
        }

        rangeSpell1 = Resources.Load(name + " spell1") as GameObject;
        Instantiate(rangeSpell1, Vector3.zero, Quaternion.identity);
        rangeSpell1.SetActive(false);

        rangeSpell2 = Resources.Load(name + " spell2") as GameObject;
        Instantiate(rangeSpell2, Vector3.zero, Quaternion.identity);
        rangeSpell2.SetActive(false);

        rangeSpell3 = Resources.Load(name + " spell3") as GameObject;
        Instantiate(rangeSpell3, Vector3.zero, Quaternion.identity);
        rangeSpell3.SetActive(false);

        rangeSpell4 = Resources.Load(name + " spell4") as GameObject;
        Instantiate(rangeSpell4, Vector3.zero, Quaternion.identity);
        rangeSpell4.SetActive(false);

        vectRangeSpell1 = Vector3.zero;
        vectRangeSpell2 = Vector3.zero;
        vectRangeSpell3 = Vector3.zero;
        vectRangeSpell4 = Vector3.zero;

        if (role == inGame.PlayeurTurn.attaquant)
        {
            inGame.posAttaquant = transform.position;
        }
        if (role == inGame.PlayeurTurn.defenseur)
        {
            inGame.posDefenseur = transform.position;
        }
    }

    public void Move(Vector2 movement)
    {
        transform.Translate(movement * Time.deltaTime);
        if(role == inGame.PlayeurTurn.attaquant)
        {
            inGame.posAttaquant = transform.position;
        }
        if (role == inGame.PlayeurTurn.defenseur)
        {
            inGame.posDefenseur = transform.position;
        }
    }

    public void Spell1()
    {
        Debug.Log("spell1");
        if (role == inGame.PlayeurTurn.attaquant && Player.Distance(inGame.posAttaquant, inGame.posDefenseur) < 5)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<Player>().personnage.role != role)
                {
                    Debug.Log("condiont ok ");
                    player.GetComponent<Player>().personnage.TakeDommage(100);
                }
            }
        }
        if (role == inGame.PlayeurTurn.defenseur)
        {
            
        }
    }

    public void TakeDommage(int dommage)
    {
        hp -= dommage;// * armure / 100;
        if (hp <= 0)
        {
            Die();
            Application.Quit();
        }
        Debug.Log("predn domaggeeee !!!");
        GameObject.FindGameObjectWithTag("HpBarre").GetComponent<Image>().fillAmount = hp;
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

    public int getHp()
    {
        return hp;
    }

    public int getMana()
    {
        return mana;
    }

    public bool Spell1Ok(Vector3 pos)
    {
        return true;
    }
    public bool Spell2Ok(Vector3 pos)
    {
        return true;
    }
    public bool Spell3Ok(Vector3 pos)
    {
        return true;
    }
    public bool Spell4Ok(Vector3 pos)
    {
        return true;
    }
}
