using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class DisplayMeshInfo : MonoBehaviour
{
    MeshFilter m_Mf;

    [SerializeField] bool m_DisplayNormals;
    [SerializeField] bool m_DisplayVertices;
    [SerializeField] float m_VertexSphereRadius;

    private void Awake()
    {
        m_Mf = GetComponent<MeshFilter>();
    }

    private void OnDrawGizmos()
    {
        if (!m_Mf || !m_Mf.sharedMesh) return;

        Vector3[] vertices = m_Mf.sharedMesh.vertices;
        Vector3[] normals = m_Mf.sharedMesh.normals;


        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 pos = vertices[i];
            Vector3 normal = normals[i];

            Gizmos.color = Color.white;
            if (m_DisplayNormals) Gizmos.DrawLine(pos, pos + normal);
            Gizmos.color = Color.red;
            if (m_DisplayVertices) Gizmos.DrawSphere(pos, m_VertexSphereRadius);
        }
    }
}
