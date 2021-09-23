using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using MyMathTools;

static class CubeToSphere
{
    public static Vector3[] origins = new Vector3[6]
    {
        new Vector3(-1.0f, -1.0f, -1.0f),
        new Vector3(1.0f, -1.0f, -1.0f),
        new Vector3(1.0f, -1.0f, 1.0f),
        new Vector3(-1.0f, -1.0f, 1.0f),
        new Vector3(-1.0f, 1.0f, -1.0f),
        new Vector3(-1.0f, -1.0f, 1.0f)
    };
    public static Vector3[] rights = new Vector3[6]
    {
        new Vector3(2.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 2.0f),
        new Vector3(-2.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, -2.0f),
        new Vector3(2.0f, 0.0f, 0.0f),
        new Vector3(2.0f, 0.0f, 0.0f)
    };
    public static Vector3[] ups = new Vector3[6]
    {
        new Vector3(0.0f, 2.0f, 0.0f),
        new Vector3(0.0f, 2.0f, 0.0f),
        new Vector3(0.0f, 2.0f, 0.0f),
        new Vector3(0.0f, 2.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 2.0f),
        new Vector3(0.0f, 0.0f, -2.0f)
    };
};

[RequireComponent(typeof(MeshFilter))]
public class PlanetGenerator : MonoBehaviour
{
    delegate float ComputeValueDelegate(float kx, float kz);
    delegate Vector3 ComputeVectorDelegate(float kx, float kz);
    delegate Vector3 ComputeVertexPos(float k1, float k2);

    MeshFilter m_MeshFilter;

    [SerializeField] int m_SpherifiedCubeNDivisions;
    [SerializeField] int m_SpherifiedCubeRadius;
    [SerializeField] Texture2D m_HeightMap;

    private void Awake()
    {
        m_MeshFilter = GetComponent<MeshFilter>();


        ComputeValueDelegate heightFunction = (kX, kZ) => m_HeightMap.GetPixel((int)(kX * m_HeightMap.width), (int)(kZ * m_HeightMap.height)).grayscale;
        m_MeshFilter.sharedMesh = PlainSpherifiedCube(m_SpherifiedCubeNDivisions, m_SpherifiedCubeRadius, 0.03f, heightFunction);
    }

    Vector3 Mult(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    Mesh PlainSpherifiedCube(int divisions, float radius, float heightsAmplitude, ComputeValueDelegate heightFunction)
    // https://github.com/caosdoar/spheres/blob/master/src/spheres.cpp
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.name = "SpherifiedCube";

        Vector3[] vertices = new Vector3[6 * (divisions + 1) * (divisions + 1)];
        Vector3[] normals = new Vector3[vertices.Length];
        int[] triangles = new int[6 * divisions * divisions * 2 * 3];
        Vector2[] uv = new Vector2[vertices.Length];

        float step = 1.0f / (float)divisions;
        Vector3 step3 = new Vector3(step, step, step);

        int offset = 0;
        for (int face = 0; face < 6; ++face)
        {
            Vector3 origin = CubeToSphere.origins[face];
            Vector3 right = CubeToSphere.rights[face];
            Vector3 up = CubeToSphere.ups[face];

            for (int j = 0; j < divisions + 1; ++j)
            {
                float kZ = (float)j / divisions;

                Vector3 j3 = new Vector3(j, j, j);
                for (int i = 0; i < divisions + 1; ++i)
                {
                    float kX = (float)i / divisions;

                    Vector3 i3 = new Vector3(i, i, i);
                    Vector3 p = origin + Mult(step3, Mult(i3, right) + Mult(j3, up));
                    Vector3 p2 = Mult(p, p);
                    Vector3 vertexPos = new Vector3(p.x * Mathf.Sqrt(1.0f - 0.5f * (p2.y + p2.z) + p2.y * p2.z / 3.0f),
                                            p.y * Mathf.Sqrt(1.0f - 0.5f * (p2.z + p2.x) + p2.z * p2.x / 3.0f),
                                            p.z * Mathf.Sqrt(1.0f - 0.5f * (p2.x + p2.y) + p2.x * p2.y / 3.0f));

                    vertices[offset + i] = vertexPos.normalized * radius * (1+heightFunction(kX, kZ) * heightsAmplitude);
                    normals[offset + i] = vertices[offset + i].normalized;

                    Spherical sph = CoordConvert.CartesianToSpherical(vertexPos);
                    float rho = sph.rho;
                    float theta = sph.theta;
                    float phi = sph.phi;

                    float kTheta = theta / (2 * Mathf.PI);
                    float kPhi = phi / Mathf.PI;

                    uv[offset + i] = new Vector2(kTheta, 1 - kPhi);
                }
                offset += divisions + 1;
            }
        }

        // bool ok = false;
        int index = 0;
        int k = divisions + 1;
        for (int face = 0; face < 6; ++face)
        {
            for (int j = 0; j < divisions; ++j)
            {
                bool bottom = j < (divisions / 2);
                for (int i = 0; i < divisions; ++i)
                {
                    bool left = i < (divisions / 2);
                    int a = (face * k + j) * k + i;
                    int b = (face * k + j) * k + i + 1;
                    int c = (face * k + j + 1) * k + i;
                    int d = (face * k + j + 1) * k + i + 1;

                    if (bottom ^ left)
                    {
                        triangles[index++] = a;
                        triangles[index++] = c;
                        triangles[index++] = b;
                        triangles[index++] = b;
                        triangles[index++] = c;
                        triangles[index++] = d;
                    }
                    else
                    {
                        triangles[index++] = a;
                        triangles[index++] = c;
                        triangles[index++] = b;
                        triangles[index++] = b;
                        triangles[index++] = c;
                        triangles[index++] = d;

                    }
                }
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

}
