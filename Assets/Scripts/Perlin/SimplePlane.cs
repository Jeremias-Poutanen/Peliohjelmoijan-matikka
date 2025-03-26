using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SimplePlane : MonoBehaviour
{
    

    [Range(1f, 10000f)]
    public float size = 1000f;

    [Range(10, 255)]
    public int segments = 100;

    [Range(0f, 255f)]
    public float noiseHeight = 10f;

    [Range(0f, 25f)]
    public float noiseAmplitude = 1f;

    public bool drawVertices = false;

    Mesh mesh;

    private void OnDrawGizmos()
    {
        GenerateMesh();

        float delta = size / (segments - 1);

        for (int i = 0; i < segments; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                float x = i * delta;
                float y = j * delta;

                if (drawVertices)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(new Vector3(x, Mathf.PerlinNoise(x * (noiseAmplitude / size), y * (noiseAmplitude / size)) * noiseHeight, y), 0.5f);
                }
            }
        }
    }

    void GenerateMesh()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "Terrain Mesh";
        }
        else
        {
            mesh.Clear();
        }

        List<Vector3> vertices = new List<Vector3>();
        List<int> tris = new List<int>();

        float delta = size / (segments - 1);

        // Vertices
        for (int i = 0; i < segments; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                float x = i * delta;
                float y = j * delta;

                vertices.Add(new Vector3(x, Mathf.PerlinNoise(x * (noiseAmplitude / size), y * (noiseAmplitude / size)) * noiseHeight, y));
            }
        }

        // Triangles
        for (int i = 0; i < segments-1; i++)
        {
            for (int j = 0; j < segments-1; j++)
            {
                // "Upper left"
                int ul = j * segments + i;
                // "Upper right
                int ur = ul + 1;

                // "Lower left"
                int ll = ul + segments;
                // "Lower right"
                int lr = ll + 1;

                // First triangle
                tris.Add(ll);
                tris.Add(ul);
                tris.Add(ur);

                // Second triangle
                tris.Add(ll);
                tris.Add(ur);
                tris.Add(lr);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
