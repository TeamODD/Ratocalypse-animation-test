using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Mesh[] meshes;
    public Material[] mat;
    private MeshFilter meshFilter;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            meshFilter.sharedMesh = meshes[0];
            gameObject.GetComponent<MeshRenderer>().material = mat[1];
        }
    }
}
