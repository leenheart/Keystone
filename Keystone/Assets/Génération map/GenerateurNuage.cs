using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuage
{
    public GameObject avatar;
    public float vitess;
}

public class GenerateurNuage : MonoBehaviour {

    public float NuageParSeconde = 2;
    public int nbNuageMax = 60;
    public float nextNuage = 0;
    public Vector3 direction = new Vector3(0, 0, 0.75f);
    public float vitess = 1;

    List<Nuage> nuages = new List<Nuage>();

	// Use this for initialization
	void Start () {
        vitess = 0.5f;
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {

        direction += new Vector3(Random.Range(-0.01f, 0.0f), 0, 0);
        if (direction.x > 0.1) direction.x = 0.1f;
        if (direction.x < -0.1) direction.x = -0.1f;

        if (nuages.Count < nbNuageMax  && Time.time >= nextNuage)
        {
            //creer nuage
            Nuage n = new Nuage();
            n.avatar = (GameObject)Instantiate(Resources.Load("nuage"), new Vector3(Random.Range(-50, 100), Random.Range(-45, -2), -50), new Quaternion());
            n.avatar.transform.localScale = new Vector3(Random.Range(5,40), Random.Range(1, 5), Random.Range(10, 35));
            DontDestroyOnLoad(n.avatar);
            n.vitess = Random.Range(0.02f, 0.2f);
            nuages.Add(n);

            nextNuage = Time.time + NuageParSeconde + Random.Range(-1, 1);
        }

        for (int i = 0; i < nuages.Count; ++i)
        {

            nuages[i].avatar.transform.position += direction * nuages[i].vitess;
            Vector3 t = nuages[i].avatar.transform.position;
            if (t.x < -100 || t.x > 100 ||t.z < -50 || t.z > 230)
            {
                Destroy(nuages[i].avatar);
                nuages.Remove(nuages[i]);
            }
        }
	}
}
