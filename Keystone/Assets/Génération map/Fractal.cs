using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

    public Mesh mesh;
    public Material material;
    public int maxDepth;
    public float childScale;

    private int depth;

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<MeshFilter>().mesh = mesh ;
        gameObject.AddComponent<MeshRenderer>().material = material;
        if (depth < maxDepth)
        {
            for (int j = 0; j < Random.Range(1f, 3f); j++)
            {
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this);
            }
        }
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.active = false;
            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.active = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Initialize(Fractal parent)
    {
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        Vector3 vectScale = new Vector3(1, 0, 1);
        Vector3 vectPosition = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        transform.localScale +=  vectScale * childScale;
        transform.localPosition = vectPosition;
    }

}
