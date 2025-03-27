using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SimplePlane : MonoBehaviour
{
    // Different layers of noise
    public List<NoiseSettings> noiseSettings = new List<NoiseSettings>();

    [Range(1f, 10000f)]
    public float size = 1000f;

    [Range(10, 255)]
    public int segments = 100;

    [Range(0f, 255f)]
    public float noiseHeight = 10f;

    // [Range(0f, 25f)]
    // public float frequency = 1f;

    [Range(-10f, 10f)]
    public float threshold = 0f;

    [Range(-100f, 100f)]
    public float xshift = 0.0f;
    [Range(-100f, 100f)]
    public float yshift = 0.0f;

    public bool useThreshold = false;

    public bool drawVertices = false;

    Mesh mesh;

    private void OnDrawGizmos()
    {
        GenerateMesh();

        /* OLD CODE
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
                    Gizmos.DrawSphere(new Vector3(x, Mathf.PerlinNoise(x * (frequency / size), y * (frequency / size)) * noiseHeight, y), 0.5f);
                }
            }
        }
        */
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
        List<Vector2> UVs = new List<Vector2>();

        float delta = size / (segments - 1);

        // Vertices
        for (int i = 0; i < segments; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                float x = i * delta;
                float y = j * delta;

                // Use Perlin Noise
                float noiseValue = 0;

                for(int k = 0; k < noiseSettings.Count; k++)
                {
                    noiseValue += noiseSettings[k].amplitude * 2.0f * (Mathf.PerlinNoise(noiseSettings[k].frequency * (x + xshift), noiseSettings[k].frequency * (y + yshift)) - 0.5f);
                }


                if(useThreshold)
                {
                    if(noiseValue < threshold)
                    {
                        noiseValue = threshold;
                    }
                }
                float height = noiseHeight * noiseValue;

                // Add the vertex
                vertices.Add(new Vector3(x, height, y));

                // Add the UVs
                UVs.Add(new Vector2((x + xshift) / size, (y + yshift) / size));
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
        mesh.SetUVs(0, UVs);
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
