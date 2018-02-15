using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    private string scene;
    private int turn;

	// Use this for initialization
	void Start ()
    {
        scene = "menu";
        turn = 0;
    }

    // changer de scene :  SceneManager.LoadScene(scene)


    // Update is called once per frame
    void Update () {

        if (scene == "menu")
        {

        }
        else if (scene == "selectionPersonnage")
        {

        }
        else if (scene == "inGame")
        {
            //spawn map
            //spawn personnage 


            //defenseur  = choisi x action il a 15 s // si lets go alors fin timer
            //toutes les actoins du defenseur // si quelqun meurt fin
            //attaquant  = choisi x action il a 15 s // si lets go alors fin timer
            //toutes les actoins du attaquant // si quelqun meurt fin

            //turn ++ et on recommence



        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(Resources.Load("Playeur"), Vector3.zero + Vector3.up * 5, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.G))
        { 
            Instantiate(Resources.Load("Dec"), new Vector3(0,2,0) + Vector3.left * UnityEngine.Random.Range(-25.0f, 25.0f) + Vector3.forward * UnityEngine.Random.Range(-25.0f, 25.0f), Quaternion.identity);
        }
    }
}