using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathTools;

public class TerrainCreator : MonoBehaviour
{
    delegate float ComputeHeigthXDelegate(float kX);
    delegate float ComputeValueDelegate(float kX, float kZ);
    delegate Vector3 ComputeVertexPos(float k1, float k2);

    MeshFilter m_Mf;

    [SerializeField] AnimationCurve m_GlassProfile;
    [SerializeField] Texture2D m_HeightMap;

    private void Awake()
    {
        m_Mf = GetComponent<MeshFilter>();
        // m_Mf.sharedMesh = GenerateTriangle();
        //m_Mf.sharedMesh = GenerateQuad(new Vector3(4,0,2));
        //m_Mf.sharedMesh = GenerateStrip(new Vector3(4, 0, 2),10, k=>.125f*Mathf.Sin(k*Mathf.PI*2*4) );

        //Vector3 halfSize = new Vector3(4, 0, 2);
        //m_Mf.sharedMesh = GeneratePlane( 20,10, (kX,kZ) => new Vector3(Mathf.Lerp(-halfSize.x, halfSize.x, kX),
        //                                               0,
        //                                               Mathf.Lerp(-halfSize.z, halfSize.z, kZ)));//);// .125f * Mathf.Sin(kX* Mathf.PI * 2 * 4) * Mathf.Cos(kZ * Mathf.PI * 2 * 4));

        //m_Mf.sharedMesh = GenerateCylinder(40, 10, 2, 6, (kx,kZ)=>m_GlassProfile.Evaluate(kZ));

        m_Mf.sharedMesh = GenerateTerrainFromHeightFunction(100, 100, new Vector3(10, 3, 10),
            (kX, kZ) => m_HeightMap.GetPixel((int)(kX * m_HeightMap.width), (int)(kZ * m_HeightMap.height)).grayscale
            );
        gameObject.AddComponent<MeshCollider>();
    }

    Mesh GenerateTerrainFromHeightFunction(int nSegmentsX, int nSegmentsZ, Vector3 size, ComputeValueDelegate heightFunc)
    {
        Vector3 halfSize = size * .5f;
        ComputeVertexPos terrainFunc = (kX, kZ) => new Vector3(
                            Mathf.Lerp(-halfSize.x, halfSize.x, kX),
                            size.y * heightFunc(kX, kZ),
                            Mathf.Lerp(-halfSize.z, halfSize.z, kZ));
        return GeneratePlane(nSegmentsX, nSegmentsZ, terrainFunc);
    }

    Mesh GeneratePlane(int nSegmentsX, int nSegmentsZ, ComputeVertexPos posFunction)
    {
        Mesh mesh = new Mesh();
        mesh.name = "3DWrappedPlaneObject";

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        //Vector3 halfSize = size * .5f;

        Vector3[] vertices = new Vector3[(nSegmentsX + 1) * (nSegmentsZ + 1)];
        int[] triangles = new int[2 * nSegmentsX * nSegmentsZ * 3];
        Vector2[] uv = new Vector2[vertices.Length];
        //Vector3[] normals = new Vector3[vertices.Length];

        //vertices
        for (int i = 0; i < nSegmentsX + 1; i++)
        {
            float kX = (float)i / nSegmentsX; // ratio
            int indexOffset = i * (nSegmentsZ + 1);

            for (int j = 0; j < nSegmentsZ + 1; j++)
            {
                float kZ = (float)j / nSegmentsZ; // ratio

                vertices[indexOffset + j] = posFunction(kX, kZ);                                                                                                                                             //new Vector3(halfSize.x, y, -halfSize.z), k); // bottom line

                uv[indexOffset + j] = new Vector2(kX, kZ);

            }

        }

        //triangles
        int index = 0;
        for (int i = 0; i < nSegmentsX; i++)
        {
            int indexOffset = (nSegmentsZ + 1) * i;
            for (int j = 0; j < nSegmentsZ; j++)
            {
                // 1st triangle of segment
                triangles[index++] = indexOffset + j;
                triangles[index++] = indexOffset + j + 1;
                triangles[index++] = indexOffset + j + 1 + nSegmentsZ + 1;

                //2nd triangle of segment
                triangles[index++] = indexOffset + j;
                triangles[index++] = indexOffset + j + 1 + nSegmentsZ + 1;
                triangles[index++] = indexOffset + j + nSegmentsZ + 1;
            }
        }



        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        //mesh.normals = normals;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
