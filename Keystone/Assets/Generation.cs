using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Generation : MonoBehaviour {

    public int surfaceBaseX;
    public int sizeMapX;
    public int sizeMapY;
    public int fluctuation;
    public float cellSize;
    public int nbSmooth;

    private int[,] map;
    private Vector3[] vertices;
    private int[] triangles;
    private Mesh mesh;


    // Use this for initialization
    void Start()
    {

        mesh = GetComponent<MeshFilter>().mesh;

        /*cellSize = 1;
        surfaceBaseX = 8;
        sizeMapX = 30;
        sizeMapY = 40;
        fluctuation = 2;*/

        //generation tableau de 1 et 0 qui forme la map

        //initialization
        map = new int[sizeMapX, sizeMapY];
        for (int x = 0; x < sizeMapX; x++)
        {
            for (int y = 0; y < sizeMapY; y++)
            {
                map[x, y] = 0;
            }
        }
        for (int x = 0; x < 5; x++)
        {
            for (int y = sizeMapY / 2 - surfaceBaseX / 2; y < sizeMapY / 2 + surfaceBaseX / 2; y++)
            {
                map[x, y] = 1;
            }
        }
        for (int x = sizeMapX - 4; x < sizeMapX; x++)
        {
            for (int y = sizeMapY / 2 - surfaceBaseX / 2; y < sizeMapY / 2 + surfaceBaseX / 2; y++)
            {
                map[x, y] = 1;
            }
        }

        for (int y = sizeMapY / 2 - surfaceBaseX / 2 + UnityEngine.Random.Range(-6, 0); y < sizeMapY / 2 + surfaceBaseX / 2 + UnityEngine.Random.Range(0, 6); y++)
        {
            map[0, y] = 1;
        }

        //random
        int debut = -1;
        int fin = -1;
        int randomDebut = 0;
        int randmFin = 0;
        for (int x = 0; x < sizeMapX; x++)
        {
            //detection debut et fin
            for (int y = 0; y < sizeMapY - 1; y++)
            {
                if (debut == -1 && map[x, y] == 1)
                {
                    debut = y;
                }
                else if (fin == -1 && debut != -1 && map[x, y] == 0)
                {
                    fin = y;
                }
            }

            //ajout nouvelle ligne avec le random
            randomDebut = UnityEngine.Random.Range(-fluctuation - randomDebut, fluctuation + randomDebut);
            randmFin = UnityEngine.Random.Range(-fluctuation - randmFin, fluctuation + randmFin);
            for (int y = 0; y < sizeMapY; y++)
            {
                if (y >= debut + randomDebut && y < fin + randmFin)
                {
                    map[x, y] = 1;
                }
            }
        }

        //complete map
        for (int z = 0; z < nbSmooth; z++)
        {
            for (int x = 0; x < sizeMapX; x++)
            {
                for (int y = 0; y < sizeMapY; y++)
                {
                    int sommeEntoure = 0;
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (x + i > 0 && x + i < sizeMapX && y + j > 0 && y + j < sizeMapY)
                            {
                                sommeEntoure += map[x + i, y + j];
                            }

                        }
                    }
                    if (sommeEntoure > 3)
                    {
                        map[x, y] = 1;
                    }
                    else if (sommeEntoure == 1 || sommeEntoure == 2 || sommeEntoure == 0)
                    {
                        map[x, y] = 0;
                    }

                }
            }
        }

        //voir le tableau dans les logs
        /*for (int x = 0; x < sizeMapX; x++)
        {
            string s = "";
            for (int y = 0; y < sizeMapY; y++)
            {
                s += map[x, y];
            }
            Debug.Log(s);
        }*/

        vertices = new Vector3[(sizeMapX + 1) * (sizeMapY + 1)];
        triangles = new int[sizeMapX * sizeMapY * 6];

        int v = 0;
        int t = 0;

        float vertexOffSet = cellSize * 0.5f;

        for (int x = 0; x <= sizeMapX; x++)
        {
            for (int y = 0; y <= sizeMapY; y++)
            {
                vertices[v] = new Vector3((x * cellSize) - vertexOffSet, 0, (y * cellSize) - vertexOffSet);
                v++;
            }
        }

        v = 0;

        for (int x = 0; x < sizeMapX; x++)
        {
            for (int y = 0; y < sizeMapY; y++)
            {
                if (map[x, y] == 1)
                {
                    triangles[t] = v;
                    triangles[t + 1] = triangles[t + 4] = v + 1;
                    triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1);
                    triangles[t + 5] = v + (sizeMapY + 1) + 1;
                }
                v++;
                t += 6;

            }
            v++;
        }


        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        transform.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
	
	// Update is called once per frame
	void Update () {
    }
}
