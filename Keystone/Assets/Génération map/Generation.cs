using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Generation : MonoBehaviour {

    public int surfaceBaseX;
    public int sizeMapX;
    public int sizeMapY;
    public int sizeMapZ;
    public int fluctuation;
    public float cellSize;
    public int nbSmooth;

    private int[,] map;
    private int[][,] underMap;

    private Vector3[] vertices;
    private int[] triangles;
    private Mesh mesh;

    void UpdateMesh(Mesh m)
    {
        m.Clear();
        m.vertices = vertices;

        m.triangles = triangles;
        m.RecalculateNormals();
    }

    void MakeSurface(int y, int x, ref int t, ref int v)
    {
        if (map[x, y - 1] == 0)
        {

            if (x != 0 && map[x - 1, y - 1] == 1 && (x >= sizeMapX - 1 || map[x + 1, y - 1] == 0))
            {
                triangles[t] = v -1;
                triangles[t + 1] = v ;
                triangles[t + 2] = v + (sizeMapY + 1) ;

                t += 3;
            }
            else if (x < sizeMapX - 1 && map[x + 1, y - 1] == 1 && (x == 0 || map[x - 1, y - 1] == 0))
            {
                triangles[t] = v + (sizeMapY + 1) ;
                triangles[t + 2] = v ;
                triangles[t + 1] = v + (sizeMapY + 1) -1 ;

                t += 3;
            }

        }
        else if (y < map.GetLength(1) - 1 && map[x, y + 1] == 0)
        {
            if (x != 0 && map[x - 1, y + 1] == 1 && (x >= sizeMapX - 1 || map[x + 1, y + 1] == 0))
            {
                triangles[t] = v + 2;
                triangles[t + 2] = v + 1;
                triangles[t + 1] = v + (sizeMapY + 1) + 1;

                t += 3;
            }
            else if ( x < sizeMapX - 1 && map[x + 1, y + 1] == 1 && (x == 0 || map[x - 1, y + 1] == 0))
            {
                triangles[t] = v + (sizeMapY + 1) + 1;
                triangles[t + 1] = v + 1;
                triangles[t + 2] = v + (sizeMapY + 1) + 2;
                t += 3;
            }
        }

        triangles[t] = v;
        triangles[t + 1] = triangles[t + 4] = v + 1;
        triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1);
        triangles[t + 5] = v + (sizeMapY + 1) + 1;

    }

    void MakeFaceYSurf(int y, int x, ref int t, ref int v, int[,] tab )
    {
        if (tab[x, y - 1] == 0)
        {
            if (x != 0 && map[x - 1, y - 1] == 1 && (x >= sizeMapX - 1 || map[x + 1, y - 1] == 0))
            {
                t += 6;
                triangles[t] = v - 1;
                triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1);
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1);
            }
            else if ( x < sizeMapX - 1 && map[x + 1, y - 1] == 1 && (x == 0 || map[x - 1, y - 1] == 0))
            {

                t += 6;
                triangles[t] = triangles[t + 3] = v;
                triangles[t + 5]  = v + (sizeMapY + 1) * (sizeMapX + 1) ;
                triangles[t + 1]  = v + (sizeMapY + 1)-1;
                triangles[t + 2] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1);
            }
            else
            {
                t += 6;
                triangles[t] = v;
                triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1);
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1);
            }
        }
        if (y < tab.GetLength(1) - 1)
        {
            if (tab[x, y + 1] == 0)
            {
                if (x != 0 && map[x - 1, y + 1] == 1 && (x >= sizeMapX - 1 || map[x + 1, y + 1] == 0))
                {
                    t += 6;
                    triangles[t] = v + 2;
                    triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) + 1;
                    triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                    triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1) + 1;
                }
                else if (x < sizeMapX - 1 && map[x + 1, y + 1] == 1 && (x == 0 || map[x - 1, y + 1] == 0))
                {
                    t += 6;
                    triangles[t] = triangles[t + 3] = v + 1;
                    triangles[t + 5] = v + (sizeMapY + 1) + 2;
                    triangles[t + 1]  = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                    triangles[t + 2] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1) + 1;
                }
                else
                {
                    t += 6;
                    triangles[t] = v + 1;
                    triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) + 1;
                    triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                    triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1) + 1;
                }
            }
        }
        
    }

    void MakeFaceY(int y, int x, ref int t, ref int v, int[,] tab, int[,] tabPrec)
    {
        if (tab[x, y - 1] == 0)
        {
            /*if ( x != 0  && tabPrec[x - 1 , y - 1] == 1 && map[x - 1, y - 1] == 1 && (x >= sizeMapX - 1 || map[x + 1, y - 1] == 0))
            {
                //diag gauche
                t += 6;
                triangles[t] = v - 1;
                triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1);
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1);
            }
            else if (x < sizeMapX - 1 && tabPrec[x + 1, y - 1] == 1  &&  map[x + 1, y - 1] == 1 && (x == 0 || map[x - 1, y - 1] == 0))
            {
                //diag droite
                t += 6;
                triangles[t] = triangles[t + 3] = v;
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 1] = v + (sizeMapY + 1) - 1;
                triangles[t + 2] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1);
            }
            else*/
            {
                t += 6;
                triangles[t] = v;
                triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1);
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1);
            }
        }
        if (y < tab.GetLength(1) - 1)
        {
            if (tab[x, y + 1] == 0)
            {
                /*if (x != 0  && tabPrec[x -1, y + 1] == 1 && map[x - 1, y + 1] == 1 && (x >= sizeMapX - 1 || map[x + 1, y + 1] == 0))
                {
                    t += 6;
                    triangles[t] = v + 2;
                    triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) + 1;
                    triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                    triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1) + 1;
                }
                else if (x < sizeMapX - 1 && tabPrec[x + 1, y + 1] == 1 && map[x + 1, y + 1] == 1 && (x == 0 || map[x - 1, y + 1] == 0))
                {
                    t += 6;
                    triangles[t] = triangles[t + 3] = v + 1;
                    triangles[t + 5] = v + (sizeMapY + 1) + 2;
                    triangles[t + 1] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                    triangles[t + 2] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1) + 1;
                }
                else*/
                {
                    t += 6;
                    triangles[t] = v + 1;
                    triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) + 1;
                    triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                    triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1) + 1;
                }
            }
        }

    }

    void MakeFaceX(int y, int x, ref int t, ref int v, int[,] tab, int makeTest)
    {
        if (tab[x, y - 1] == 0)
        {
           if (makeTest == 1 || tab[x - 1, y] == 0)
            {
                t += 6;
                triangles[t] = triangles[t + 3] = v;
                triangles[t + 5]  = v + 1;
                triangles[t + 1]  = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 2] = triangles[t + 4]= v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
            }

            if (makeTest == 2 || tab[x + 1, y] == 0)
            {
                t += 6;
                triangles[t+2] = triangles[t + 4]= v + sizeMapY + 1;
                triangles[t + 5] = v + 1 + sizeMapY + 1;
                triangles[t + 1]  = v + (sizeMapY + 1) * (sizeMapX + 1) + sizeMapY + 1;
                triangles[t ] = triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1 + sizeMapY + 1;
            }
        }
        else 
        {
            if (makeTest == 1 || tab[x - 1, y] == 0)
            {
                t += 6;
                triangles[t] = v;
                triangles[t + 2] = triangles[t + 3] = v + 1;
                triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
            }

            if (makeTest == 2 || tab[x + 1, y] == 0)
            {
                t += 6;
                triangles[t ] = v + sizeMapY + 1;
                triangles[t + 1] = triangles[t + 4] = v + 1 + sizeMapY + 1;
                triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1) + sizeMapY + 1;
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1 + sizeMapY + 1;
            }
        }
    }

    void PrintTab(int[,] tab)
    {
        //voir le tableau dans les logs
        for (int x = 0; x < sizeMapX; x++)
        {
            string s = "";
            for (int y = 0; y < sizeMapY; y++)
            {
                s += tab[x, y];
            }
            Debug.Log(s);
        }
    }

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
        underMap = new int[sizeMapZ][,];

        for (int x = 0; x < sizeMapX; x++)
        {
            for (int y = 0; y < sizeMapY; y++)
            {
                map[x, y] = 0;
            }
        }
        for (int z = 0; z < sizeMapZ; z++)
        {
            underMap[z] = new int[sizeMapX, sizeMapY];
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

        //PrintTab(map);

        for (int x = 0; x < sizeMapX; x++)
        {
            for (int y = 0; y < sizeMapY; y++)
            {
                underMap[0][x, y] = map[x, y];
            }
        }

        for (int z = 1; z < sizeMapZ; z++)
        {
            if (z - 1 >= 0)
            {

                for (int x = 0; x < sizeMapX; x++)
                {
                    for (int y = 0; y < sizeMapY; y++)
                    {
                        underMap[z][x, y] = underMap[z - 1][x, y];
                    }
                }
            }
            int rand = 0;
            for (int x = 0; x < sizeMapX; x++)
            {
                for (int y = 0; y < sizeMapY; y++)
                {
                    //Debug.Log(x + " " + y + " " + z);

                    if ((y == 0 || y == sizeMapY - 1) || (underMap[z][x, y] == 1 && (underMap[z][x, y - 1] == 0 || underMap[z][x, y + 1] == 0)))
                    {
                        if (x == 0 || x == sizeMapX - 1)
                        {
                            rand = UnityEngine.Random.Range(0, sizeMapZ - z - 2);
                        }
                        else if (underMap[z][x + 1, y] == 0 && underMap[z][x - 1, y] == 0)
                        {
                            rand = UnityEngine.Random.Range(0, sizeMapZ - z - 2);
                        }
                        else if (underMap[z][x + 1, y] == 0 || underMap[z][x - 1, y] == 0)
                        {
                            rand = UnityEngine.Random.Range(0, 2 + sizeMapZ - z - 4);
                        }
                        else if (underMap[z][x + 1, y] == 1 && underMap[z][x - 1, y] == 1)
                        {
                            rand = UnityEngine.Random.Range(0, 6 + sizeMapZ - z - 6);
                        }
                        if (rand <= 3)
                        {
                            underMap[z][x, y] = 0;
                        }
                    }

                    int sommeEntoure = 0;
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (x + i > 0 && x + i < sizeMapX && y + j > 0 && y + j < sizeMapY)
                            {
                                sommeEntoure += underMap[z][x + i, y + j];
                            }

                        }
                    }
                    if (sommeEntoure <= 4)
                    {
                        underMap[z][x, y] = 0;
                    }
                }
            }
        }

        vertices = new Vector3[(sizeMapX + 1) * (sizeMapY + 1) * (sizeMapZ + 1)];
        //Debug.Log(" ici     " + (sizeMapX + 1) * (sizeMapY + 1) * (sizeMapZ + 1));
        triangles = new int[sizeMapX * sizeMapY * 48 * (sizeMapZ)];

        int v = 0;
        int t = 0;

        //float vertexOffSet = cellSize * 0.5f;

        for (int z = 0; z <= sizeMapZ; z++)
        {
            for (int x = 0; x <= sizeMapX; x++)
            {
                for (int y = 0; y <= sizeMapY; y++)
                {
                    vertices[v] = new Vector3((x * cellSize) + UnityEngine.Random.Range(-0.25f, 0.25f) /*- vertexOffSet*/, -z, (y * cellSize) + UnityEngine.Random.Range(-0.25f, 0.25f) /*- vertexOffSet*/);
                    v++;
                }
            }
        }

        v = 0;
        for (int x = 0; x < sizeMapX; x++)
        {
            for (int y = 0; y < sizeMapY; y++)
            {
                if (map[x, y] == 1 && x == 0)
                {

                    MakeSurface(y, x, ref t, ref v);
                    MakeFaceX(y, x, ref t, ref v, map, 1);
                    MakeFaceYSurf(y, x, ref t, ref v, map);

                }
                else if (x == sizeMapX - 1 && map[x, y] == 1)
                {

                    MakeSurface(y, x, ref t, ref v);
                    MakeFaceX(y, x, ref t, ref v, map, 2);
                    MakeFaceYSurf(y, x, ref t, ref v, map);
                }
                else if (map[x, y] == 1)
                {
                    MakeSurface(y, x, ref t, ref v);
                    MakeFaceYSurf(y, x, ref t, ref v, map);
                    MakeFaceX(y, x, ref t, ref v, map, 0);
                }
                t += 6;

                v++;
            }
            v++;
        }

        UpdateMesh(mesh);



        for (int z = 1; z < sizeMapZ; z++)
        {
            v += sizeMapY + 1;
            //Debug.Log(z);
            //PrintTab(underMap[z]);
            for (int x = 0; x < sizeMapX; x++)
            {
                for (int y = 0; y < sizeMapY; y++)
                {

                        if (underMap[z][x, y] == 1 && x == 0)
                        {
                            MakeFaceX(y, x, ref t, ref v, underMap[z], 1);
                            MakeFaceY(y, x, ref t, ref v, underMap[z],  underMap[z - 1]);

                        }
                        else if (x == sizeMapX - 1 && underMap[z][x, y] == 1)
                        {
                            MakeFaceX(y, x, ref t, ref v, underMap[z], 2);
                            MakeFaceY(y, x, ref t, ref v, underMap[z],  underMap[z - 1]);
                        }
                        else if (underMap[z][x, y] == 1)
                        {
                            MakeFaceY(y, x, ref t, ref v, underMap[z], underMap[z - 1]);
                            MakeFaceX(y, x, ref t, ref v, underMap[z], 0);
                        }
                    
                    v++;

                }
                v++;
            }

        }

        Mesh m = GameObject.FindWithTag("UnderMap").GetComponent<MeshFilter>().mesh;
        UpdateMesh(m);

        transform.GetComponent<MeshCollider>().sharedMesh = mesh;

        int random = 0;
        for (int x = 0; x < sizeMapX; x++)
        {
            for (int y = 0; y < sizeMapY; y++)
            {
                random = UnityEngine.Random.Range(0, 15);
               if (random == 1 && map[x,y] == 1)
                {
                    Instantiate(Resources.Load("dec"), new Vector3(x, 0, y) , Quaternion.identity);
                }
            }
        }

        for (int x = 0; x < sizeMapX; x++)
        {
            for (int y = 0; y < sizeMapY; y++)
            {
                random = UnityEngine.Random.Range(0, 50);
                if (random == 1 && map[x, y] == 1)
                {
                    Instantiate(Resources.Load("arbre"), new Vector3(x, 1, y), new Quaternion(0,-90,-90,0));
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
    }
}
