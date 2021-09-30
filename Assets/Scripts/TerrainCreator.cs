using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapNoise;
using MyMathTools;

public class TerrainCreator : MonoBehaviour
{
    delegate float ComputeHeigthXDelegate(float kX);
    delegate float ComputeValueDelegate(float kX, float kZ);
    delegate Vector3 ComputeVertexPos(float k1, float k2);

    MeshFilter m_Mf;
    [SerializeField] int m_XSize;
    [SerializeField] int m_ZSize;

    private void Awake()
    {
        m_Mf = GetComponent<MeshFilter>();

        float offSetX = Random.Range(-200, 200);
        float offSetZ = Random.Range(-200, 200);

        m_Mf.sharedMesh = GenerateTerrainFromHeightFunction(m_XSize, m_ZSize, new Vector3(m_XSize, 64, m_ZSize),
            (kX, kZ) => MyNoise.noiseMap(kX*(m_XSize/500)+offSetX,kZ*(m_ZSize/500)+offSetZ));/*Mathf.PerlinNoise((kX * 0.001f * m_XSize) + offSetX, (kZ * 0.001f * m_ZSize) + offSetZ) * 1.25f +
                        Mathf.PerlinNoise((kX * 0.0004f * m_XSize) + offSetX2, (kZ * 0.0004f * m_ZSize) + offSetZ2) * 6f +
                        Mathf.PerlinNoise((kX * 0.0004f * m_XSize) + offSetX3, (kZ * 0.0004f * m_ZSize) + offSetZ3) * 6f
            ) ;

            500 est la taille du bruit de base on veut garder cette taille
*/

        gameObject.AddComponent<MeshCollider>();
    }

    Mesh GenerateTerrainFromHeightFunction(int nSegmentsX, int nSegmentsZ, Vector3 size, ComputeValueDelegate heightFunc)
    {
        Vector3 halfSize = size * .5f;
        ComputeVertexPos terrainFunc = (kX, kZ) => new Vector3(
                            Mathf.Lerp(-halfSize.x, halfSize.x, kX),
                            size.y * heightFunc(kX*5, kZ*5),
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
