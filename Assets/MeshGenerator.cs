using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    public int m_size_x;
    public int m_size_y;
    public int m_size_z;
    public int m_nbSegments;

    MeshFilter m_Mf;

    void Awake()
    {
        m_Mf = GetComponent<MeshFilter>();
        m_Mf.sharedMesh = GenerateStripe(new Vector3(m_size_x, m_size_y, m_size_z), m_nbSegments);
    }

    Mesh GenerateTriangle()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Triangle";

        Vector3[] vertices = new Vector3[3];
        int[] triangles = new int[3];

        vertices[0] = new Vector3(1, 0, 0);
        vertices[1] = new Vector3(0, 1, 0);
        vertices[2] = new Vector3(0, 0, 1);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }

    Mesh GenerateQuad(Vector3 size)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Quad";

        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(-size.x * .5f, 0, -size.z * .5f);
        vertices[1] = new Vector3(-size.x * .5f, 0, size.z * .5f);
        vertices[2] = new Vector3(size.x * .5f, 0, size.z * .5f);
        vertices[3] = new Vector3(size.x * .5f, 0, -size.z * .5f);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }

    Mesh GenerateStripe(Vector3 size, int nbSegementX)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Stripe";

        Vector3[] vertices = new Vector3[(nbSegementX + 1)* 2];
        int[] triangles = new int[nbSegementX * 6]; // 6 car 2 triangles par stripe

        float step_x = size.x /(nbSegementX + 2);
        int cpt = 0;

        Debug.Log("Vertices");
        for (int i = 1; i < (nbSegementX + 2); i++)
        {
            vertices[cpt] = new Vector3((step_x * i) - size.x * 0.5f, 0,  size.z * 0.5f);
            vertices[cpt+1] = new Vector3((step_x * i) - (size.x * 0.5f), 0, -size.z * 0.5f);
            cpt += 2;
        }

        int idxTriangles = 0;

        Debug.Log("Triangles");
        for (int i = 0; i <= (triangles.Length/3)-1; i++)
        {
            if(i % 2 == 0){
                triangles[idxTriangles] = i+2;
                triangles[idxTriangles+1] = i+1; 
                triangles[idxTriangles+2] = i;
            }
            else{
                triangles[idxTriangles] = i;
                triangles[idxTriangles+1] = i+1; 
                triangles[idxTriangles+2] = i+2;
            }
            
            idxTriangles +=3;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
