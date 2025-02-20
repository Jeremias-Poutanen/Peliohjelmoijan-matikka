using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class SimpleMesh : MonoBehaviour
{

    [Header("Disc")]
    [Range(3, 255)]
    public int segments = 10;
    [Range(0.1f, 100f)]
    public float radius = 5f;

    [Header("Donut")]
    [Range(3, 255)]
    public int donutSegments = 10;
    [Range(0.1f, 100f)]
    public float innerRadius = 5f;
    [Range(0.1f, 100f)]
    public float thickness = 3f;

    private Mesh GenerateDonutMesh()
    {
        Mesh mesh = new Mesh();

        // Generate the actual mesh

        // Vertices
        List<Vector3> verts = new List<Vector3>();

        List<Vector2> uvs = new List<Vector2>();

        float delta_angle = 360f / donutSegments;

        for (int i = 0; i < donutSegments; i++)
        {
            float angle = i * delta_angle;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * innerRadius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * innerRadius;
            // Add the vertex into a suitable data structure (array, list)
            Vector3 vertex = new Vector3(x, 0, y);
            verts.Add(vertex);

            Vector2 uv = (new Vector2(1, 1) + (new Vector2(x, y) / (innerRadius + thickness))) / 2f;
            uvs.Add(uv);

            x = Mathf.Cos(angle * Mathf.Deg2Rad) * (innerRadius + thickness);
            y = Mathf.Sin(angle * Mathf.Deg2Rad) * (innerRadius + thickness);
            vertex = new Vector3(x, 0, y);
            verts.Add(vertex);

            uv = (new Vector2(1, 1) + (new Vector2(x, y) / (innerRadius + thickness))) / 2f;
            uvs.Add(uv);
        }

        // Triangles
        List<int> tris = new List<int>();

        for (int i = 0; i < donutSegments - 1; i++)
        {
            tris.Add(i*2);
            tris.Add(i*2 + 3);
            tris.Add(i*2 + 1);

            tris.Add(i*2);
            tris.Add(i*2 + 2);
            tris.Add(i*2 + 3);
        }
        
        tris.Add((donutSegments - 1) * 2);
        tris.Add(1);
        tris.Add((donutSegments - 1) * 2 + 1);

        tris.Add((donutSegments - 1) * 2);
        tris.Add(0);
        tris.Add(1);


        // Set vertices for the mesh
        mesh.SetVertices(verts);
        // Set triangles for the mesh
        mesh.SetTriangles(tris, 0);

        mesh.SetUVs(0, uvs);

        mesh.RecalculateNormals();

        return mesh;
    }
    void Update()
    {
        Mesh mesh = GenerateDonutMesh();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private Mesh GenerateDiscMesh()
    {
        Mesh mesh = new Mesh();

        // Generate the actual mesh

        // Vertices
        List<Vector3> verts = new List<Vector3>();

        verts.Add(Vector3.zero); // Center of the disc

        float delta_angle = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * delta_angle;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            // Add the vertex into a suitable data structure (array, list)
            Vector3 vertex = new Vector3(x, 0, y);
            verts.Add(vertex);
        }

        // Triangles
        List<int> tris = new List<int>();

        for (int i = 0; i < segments - 1; i++)
        {
            tris.Add(0);
            tris.Add(i+2);
            tris.Add(i+1);
        }
        // Last triangle
        tris.Add(0);
        tris.Add(1);
        tris.Add(segments);

        // Set vertices for the mesh
        mesh.SetVertices(verts);
        // Set triangles for the mesh
        mesh.SetTriangles(tris, 0);

        mesh.RecalculateNormals();

        return mesh;
    }

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        // Generate the actual mesh

        // Vertices
        Vector3[] verts = new Vector3[4];
        //                     X     Z
        verts[0] = new Vector3(0, 0, 0);
        verts[1] = new Vector3(1, 0, 0);
        verts[2] = new Vector3(0, 0, 1);
        verts[3] = new Vector3(1, 0, 1);

        // Triangles
        int[] tris = new int[6];
        // 1st triangle
        tris[0] = 0;
        tris[1] = 3;
        tris[2] = 1;
        // 2nd triangle
        tris[3] = 0;
        tris[4] = 2;
        tris[5] = 3;

        // Set vertices for the mesh
        mesh.vertices = verts;
        // Set triangles for the mesh
        mesh.triangles = tris;

        mesh.RecalculateNormals();

        return mesh;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
}
