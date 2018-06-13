using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Generation : MonoBehaviour
{

    public int surfaceBaseX;
    public int sizeMapX;
    public int sizeMapY;
    public int sizeMapZ;
    public int fluctuation;
    public float cellSize;
    public int nbSmooth;

    public string MapString;

    public int[,] map;

    private int[,] cielTab;
    private int[][,] underMap;

    private Vector3[] vertices;
    private int[] triangles;
    private Mesh mesh;

    /*void Awake()
    {
        gameObject.SetActive(true);
    }*/

    void UpdateMesh(Mesh m)
    {
        m.Clear();
        m.vertices = vertices;

        m.triangles = triangles;
        m.RecalculateNormals();
    }

    void MakeSurface(int y, int x, ref int t, ref int v)
    {
        if (y <= 1 || map[x, y - 1] == 0)
        {

            if (x != 0 && y != 0 && map[x - 1, y - 1] == 1 && (x >= sizeMapX - 1 || map[x + 1, y - 1] == 0))
            {
                triangles[t] = v - 1;
                triangles[t + 1] = v;
                triangles[t + 2] = v + (sizeMapY + 1);

                t += 3;
            }
            else if (y == 0 || (x < sizeMapX - 1 && map[x + 1, y - 1] == 1 && (x == 0 || map[x - 1, y - 1] == 0)))
            {
                triangles[t] = v + (sizeMapY + 1);
                triangles[t + 2] = v;
                triangles[t + 1] = v + (sizeMapY + 1) - 1;

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
            else if (x < sizeMapX - 1 && map[x + 1, y + 1] == 1 && (x == 0 || map[x - 1, y + 1] == 0))
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

    void MakeFaces(int y, int x, ref int t, ref int v, int[,] tab, int[,] tabDessus, int[,] tabDessous)
    {
        bool pencher = false;
        if ((y == 0 || tab[x, y - 1] == 0))
        {
            // faces Y
            if (x != 0 && (y == 0 || tab[x - 1, y - 1] == 1) && (y == 0 || x >= sizeMapX - 1 || tab[x + 1, y - 1] == 0) && (y == 0 || tabDessus[x, y - 1] == 1)) // diagonal gauche
            {
                t += 6;
                triangles[t] = v - 1;
                triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1);
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1);
            }
            else if (x < sizeMapX - 1 && (y == 0 || tab[x + 1, y - 1] == 1) && (x == 0 || y == 0 || tab[x - 1, y - 1] == 0) && (y == 0 || tabDessus[x, y - 1] == 1)) // diagonal droite
            {

                t += 6;
                triangles[t] = triangles[t + 3] = v;
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 1] = v + (sizeMapY + 1) - 1;
                triangles[t + 2] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1);
            }
            else if (tabDessous[x, y] == 0 && ((x == 0 || tab[x - 1, y] == 0) || (x >= sizeMapX - 1 || tab[x + 1, y] == 0)))// pencher
            {
                tab[x, y] = 0;

                pencher = true;
                t += 6;
                triangles[t] = v;
                triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1);
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1) + 1;

                t += 6;
                triangles[t + 3] = v;
                triangles[t + 5] = v + 1;
                triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;


                if ((x != 0 && tab[x - 1, y] == 1 && tab[x - 2, y] == 1))
                {
                    triangles[t + 2] = v;
                    triangles[t + 1] = v + (sizeMapY + 1) * (sizeMapX + 1);
                    triangles[t] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                }

                t += 6;
                triangles[t + 4] = v + sizeMapY + 1;
                triangles[t + 5] = v + 1 + sizeMapY + 1;
                triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1 + sizeMapY + 1;


                if ((x < sizeMapX - 2 && tab[x + 1, y] == 1 && tab[x + 2, y] == 1))
                {
                    triangles[t + 2] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1 + sizeMapY + 1;
                    triangles[t + 1] = v + (sizeMapY + 1) * (sizeMapX + 1) + sizeMapY + 1;
                    triangles[t] = v + sizeMapY + 1;
                }
            }
            else // basic
            {
                t += 6;
                triangles[t] = v;
                triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1);
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1);
            }

            //faces X
            if (!pencher && (x == 0 || tab[x - 1, y] == 0)) // gauche
            {
                t += 6;
                triangles[t] = triangles[t + 3] = v;
                triangles[t + 5] = v + 1;
                triangles[t + 1] = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 2] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
            }

            if (!pencher && (x >= sizeMapX - 1 || tab[x + 1, y] == 0)) // droite
            {
                t += 6;
                triangles[t + 2] = triangles[t + 4] = v + sizeMapY + 1;
                triangles[t + 5] = v + 1 + sizeMapY + 1;
                triangles[t + 1] = v + (sizeMapY + 1) * (sizeMapX + 1) + sizeMapY + 1;
                triangles[t] = triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1 + sizeMapY + 1;
            }
        }
        if (y < tab.GetLength(1) - 1)
        {
            if ((y >= sizeMapY || tab[x, y + 1] == 0))
            {
                //Faces Y
                if (x != 0 && tab[x - 1, y + 1] == 1 && (x >= sizeMapX - 1 || tab[x + 1, y + 1] == 0) && tabDessus[x, y + 1] == 1)// diagonal gauche
                {
                    t += 6;
                    triangles[t] = v + 2;
                    triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) + 1;
                    triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                    triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1) + 1;
                }
                else if (x < sizeMapX - 1 && tab[x + 1, y + 1] == 1 && (x == 0 || tab[x - 1, y + 1] == 0) && tabDessus[x, y + 1] == 1)// diagonal droite
                {
                    t += 6;
                    triangles[t] = triangles[t + 3] = v + 1;
                    triangles[t + 5] = v + (sizeMapY + 1) + 2;
                    triangles[t + 1] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                    triangles[t + 2] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1) + 1;
                }
                else if (tabDessous[x, y] == 0 && ((x == 0 || tab[x - 1, y] == 0) || (x >= sizeMapX - 1 || tab[x + 1, y] == 0)))// pencher
                {

                    tab[x, y] = 0;
                    pencher = true;
                    t += 6;
                    triangles[t] = v + 1;
                    triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) + 1;
                    triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1);
                    triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1);

                    t += 6;

                    triangles[t] = v;
                    triangles[t + 2] = v + 1;
                    triangles[t + 1] = v + (sizeMapY + 1) * (sizeMapX + 1);

                    if ((x != 0 && tab[x - 1, y] == 1 && tab[x - 2, y] == 1))
                    {
                        triangles[t + 4] = v + 1;
                        triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1);
                        triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                    }

                    t += 6;

                    triangles[t] = v + sizeMapY + 1;
                    triangles[t + 1] = v + 1 + sizeMapY + 1;
                    triangles[t + 2] = v + (sizeMapY + 1) * (sizeMapX + 1) + sizeMapY + 1;

                    if ((x < sizeMapX - 2 && tab[x + 1, y] == 1 && tab[x + 2, y] == 1))
                    {
                        triangles[t + 3] = v + 1 + sizeMapY + 1;
                        triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + sizeMapY + 1;
                        triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1 + sizeMapY + 1;
                    }

                }
                else// basic
                {
                    t += 6;
                    triangles[t] = v + 1;
                    triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) + 1;
                    triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
                    triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + (sizeMapY + 1) + 1;
                }
            }
            //Faces X
            if (!pencher && (x == 0 || tab[x - 1, y] == 0)) // gauche
            {
                t += 6;
                triangles[t] = v;
                triangles[t + 2] = triangles[t + 3] = v + 1;
                triangles[t + 1] = triangles[t + 4] = v + (sizeMapY + 1) * (sizeMapX + 1);
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1;
            }
            if (!pencher && (x >= sizeMapX - 1 || tab[x + 1, y] == 0)) // droite
            {
                t += 6;
                triangles[t] = v + sizeMapY + 1;
                triangles[t + 1] = triangles[t + 4] = v + 1 + sizeMapY + 1;
                triangles[t + 2] = triangles[t + 3] = v + (sizeMapY + 1) * (sizeMapX + 1) + sizeMapY + 1;
                triangles[t + 5] = v + (sizeMapY + 1) * (sizeMapX + 1) + 1 + sizeMapY + 1;
            }

        }
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        mesh = GetComponent<MeshFilter>().mesh;

        map = new int[sizeMapX, sizeMapY];

        /*sizeMapZ = 20;
        nbSmooth = 3;

        cellSize = 1;
        surfaceBaseX = 8;
        sizeMapX = 30;
        sizeMapY = 40;
        fluctuation = 2;*/
    }

    private void Smooth(int nbSmooth)
    {
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
    }


    private void GenerateTrou(int x, int y, int ZeroOrOne)
    {

        int hautmax = Random.Range(y - 3 - ZeroOrOne, y);
        int basmax = Random.Range(y + 1, y + 3 + ZeroOrOne);

        int gauchemax = Random.Range(x - 6 - ZeroOrOne, x);
        int droitemax = Random.Range(x + 1, x + 6 + ZeroOrOne);


        // Debug.Log("generate trou :" + x + " " + y +"  " + ZeroOrOne + " haut " + hautmax + " bas " + basmax);
        for (int xx = gauchemax; xx < droitemax; xx++)
        {
            for (int yy = hautmax; yy < basmax; yy++)
            {
                if (ZeroOrOne == 1)
                {
                    if (xx <= 0) return;
                    if (yy <= 0) return;
                    if (xx >= sizeMapX - 1) return;
                    if (yy >= sizeMapY - 1) return;
                }
                else
                {
                    if (xx <= 3) return;
                    if (yy <= 3) return;
                    if (xx >= sizeMapX - 3) return;
                    if (yy >= sizeMapY - 3) return;
                }

                //Debug.Log(xx + " " + yy);
                map[xx, yy] = ZeroOrOne;
            }
        }

    }

    public void GenerateMap()
    {
        DontDestroyOnLoad(gameObject);
        mesh = GetComponent<MeshFilter>().mesh;

        map = new int[sizeMapX, sizeMapY];

        /*sizeMapZ = 20;
        nbSmooth = 3;

        cellSize = 1;
        surfaceBaseX = 8;
        sizeMapX = 30;
        sizeMapY = 40;
        fluctuation = 2;*/

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

        for (int y = sizeMapY / 2 - surfaceBaseX / 2 + Random.Range(-6, 0); y < sizeMapY / 2 + surfaceBaseX / 2 + Random.Range(0, 6); y++)
        {
            map[0, y] = 1;
        }

        Debug.Log(map);

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
            randomDebut = Random.Range(-fluctuation - randomDebut, fluctuation + randomDebut);
            randmFin = Random.Range(-fluctuation - randmFin, fluctuation + randmFin);
            for (int y = 0; y < sizeMapY; y++)
            {
                if (y >= debut + randomDebut && y < fin + randmFin)
                {
                    map[x, y] = 1;
                }
            }
        }

        //complete map
        Smooth(nbSmooth - 1);
        int nbcase = 0;

        //generate trou
        while (nbcase > 1000)
        {

            if (Random.Range(0, 1) < 0.6)
            {
                int centreX = Random.Range(8, sizeMapX - 8);
                int centreY = Random.Range(8, sizeMapY - 8);

                while (map[centreX, centreY] != 1)
                {
                    centreX = Random.Range(8, sizeMapX - 8);
                    centreY = Random.Range(8, sizeMapY - 8);
                }

                //GenerateTrouOr(centreX, centreY, Random.Range(1, 3), 0);
                GenerateTrou(centreX, centreY, 0);
                GenerateTrou(centreX + 1, centreY, 0);
                GenerateTrou(centreX, centreY + 1, 0);
                GenerateTrou(centreX, centreY - 1, 0);
            }
            nbcase = 0;
            for (int x = 0; x < sizeMapX; x++)
            {
                for (int y = 0; y < sizeMapY; y++)
                {
                    nbcase += map[x, y];

                }
            }
            Debug.Log(nbcase);
        }

        nbcase = 0;
        while (nbcase < 600)
        {
            //generate petite ile
            if (Random.Range(0, 1) < 0.7)
            {
                int centreX = Random.Range(0, sizeMapX);
                int centreY = Random.Range(0, sizeMapY);

                while (map[centreX, centreY] != 0)
                {
                    centreX = Random.Range(0, sizeMapX);
                    centreY = Random.Range(0, sizeMapY);
                }

                //GenerateTrouOr(centreX, centreY, Random.Range(2, 5), 1);
                GenerateTrou(centreX, centreY, 1);
                GenerateTrou(centreX + 1, centreY, 1);
                GenerateTrou(centreX, centreY + 1, 1);
                GenerateTrou(centreX, centreY - 1, 1);
            }
            nbcase = 0;
            for (int x = 0; x < sizeMapX; x++)
            {
                for (int y = 0; y < sizeMapY; y++)
                {
                    nbcase += map[x, y];

                }
            }
            Debug.Log(nbcase);
        }
        Smooth(1);


        // create the string:
        MapString = "";
        // Assume C[i] has the number of the shuffled card
        for (int i = 0; i < sizeMapX; i++)
        {
            for (int j = 0; j < sizeMapY; j++)
            {
                string crd = "" + map[i, j];
                MapString += crd;
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

    public void Generate()
    {
        DontDestroyOnLoad(gameObject);
        mesh = GetComponent<MeshFilter>().mesh;

        map = new int[sizeMapX, sizeMapY];

        /*sizeMapZ = 20;
        nbSmooth = 3;

        cellSize = 1;
        surfaceBaseX = 8;
        sizeMapX = 30;
        sizeMapY = 40;
        fluctuation = 2;*/

        //PrintTab(map);
        string crd = MapString.ToString();
        int index = 0;
        //Debug.Log(crd.Length);
        for (int i = 0; i < sizeMapX; i++)
        {
            for (int j = 0; j < sizeMapY; j++)
            {
                //Debug.Log(index +" " + crd[index] + " -48 " );
                map[i, j] = crd[index] - 48;
                index++;

            }
        }

        cielTab = new int[sizeMapX, sizeMapY];
        underMap = new int[sizeMapZ][,];

        for (int x = 0; x < sizeMapX; x++)
        {
            for (int y = 0; y < sizeMapY; y++)
            {
                cielTab[x, y] = 1;
            }
        }
        for (int z = 0; z < sizeMapZ; z++)
        {
            underMap[z] = new int[sizeMapX, sizeMapY];
        }

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
                    float r = 0;
                    if (z != 0)
                    {
                        r = UnityEngine.Random.Range(-0.25f, 0.25f);
                    }
                    vertices[v] = new Vector3((x * cellSize) + UnityEngine.Random.Range(-0.25f, 0.25f) /*- vertexOffSet*/, -(z * 3) + r, (y * cellSize) + UnityEngine.Random.Range(-0.25f, 0.25f) /*- vertexOffSet*/);
                    v++;
                }
            }
        }

        v = 0;
        for (int x = 0; x < sizeMapX; x++)
        {
            for (int y = 0; y < sizeMapY; y++)
            {
                if (map[x, y] == 1)
                {
                    MakeSurface(y, x, ref t, ref v);
                    MakeFaces(y, x, ref t, ref v, underMap[0], cielTab, underMap[1]);
                }
                t += 6;
                v++;
            }
            v++;
        }

        UpdateMesh(mesh);

        triangles = new int[sizeMapX * sizeMapY * 48 * (sizeMapZ)];

        for (int z = 1; z < sizeMapZ; z++)
        {
            v += sizeMapY + 1;

            for (int x = 0; x < sizeMapX; x++)
            {
                for (int y = 0; y < sizeMapY; y++)
                {
                    if (underMap[z][x, y] == 1)
                    {
                        MakeFaces(y, x, ref t, ref v, underMap[z], underMap[z - 1], underMap[z + 1]);
                    }
                    v++;
                }
                v++;
            }
        }

        Mesh m = GetComponentsInChildren<MeshFilter>()[1].mesh;

        UpdateMesh(m);

        GetComponent<MeshCollider>().sharedMesh = mesh;


        //pont
        int nbVide = 0;

        for (int x = 0; x < sizeMapX; x++)
        {
            for (int y = 0; y < sizeMapY; y++)
            {
                if (map[x, y] == 0 && (x != 0 && map[x - 1, y] == 1))
                {
                    nbVide++;
                    if (nbVide > 4)
                    {
                        for (int xx = x; xx < sizeMapX; xx++)
                        {
                            if (map[xx, y - 2] == 1)
                            {
                                DontDestroyOnLoad(Instantiate(Resources.Load("Pont 2"), new Vector3(x - 1, 0, y - 2), new Quaternion()));
                                for (int xxx = x; xxx < xx + 2; xxx++)
                                {
                                    DontDestroyOnLoad(Instantiate(Resources.Load("pont"), new Vector3(xxx, 0, y - 2), new Quaternion()));
                                }
                                DontDestroyOnLoad(Instantiate(Resources.Load("Pont 2"), new Vector3(xx + 2, 0, y - 2), Quaternion.Inverse(new Quaternion())));
                                xx = sizeMapX;
                            }
                        }
                        y += 4;
                        nbVide = 0;
                    }
                }
                else
                {
                    nbVide = 0;
                }
            }
        }

        for (int y = 0; y < sizeMapY; y++)
        {
            for (int x = 0; x < sizeMapX; x++)
            {
                if (map[x, y] == 0 && (y != 0 && map[x, y - 1] == 1))
                {
                    nbVide++;
                    if (nbVide > 2)
                    {
                        for (int yy = y; yy < sizeMapY; yy++)
                        {
                            if (map[x - 1, yy] == 1)
                            {
                                DontDestroyOnLoad(Instantiate(Resources.Load("Pont 2"), new Vector3(x - 1, 0, y - 1), new Quaternion()));
                                for (int yyy = y; yyy < yy + 2; yyy++)
                                {
                                    DontDestroyOnLoad(Instantiate(Resources.Load("pont"), new Vector3(x - 1, 0, yyy), new Quaternion()));
                                }
                                DontDestroyOnLoad(Instantiate(Resources.Load("Pont 2"), new Vector3(x - 1, 0, yy + 2), Quaternion.Inverse(new Quaternion())));
                                yy = sizeMapY;
                            }
                        }
                        x += 4;
                        nbVide = 0;
                    }
                }
                else
                {
                    nbVide = 0;
                }
            }
        }

    }

    public void generateObstacle()
    {
        int random = 0;
        int randomArbre = 0;

        for (int x = 2; x < sizeMapX - 2; x++)
        {
            for (int y = 0; y < sizeMapY; y++)
            {
                if (map[x, y] == 1)
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
                    if (sommeEntoure > 7)
                    {
                        random = Random.Range(0, 50);
                        randomArbre = Random.Range(0, 50);

                        if (random == 1)
                        {
                            DontDestroyOnLoad(Instantiate(Resources.Load("dec"), new Vector3(x, 0.25f, y), Quaternion.identity));
                        }
                        else if (randomArbre == 1)
                        {
                            DontDestroyOnLoad(Instantiate(Resources.Load("tree"), new Vector3(x, 2, y), new Quaternion()));
                        }
                    }
                }
            }
        }
    }

}
        