using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class Tube3DGenerator : MonoBehaviour
{
    delegate float ComputeValueDelegate(float kx, float kz);

    MeshFilter m_MeshFilter;

    [SerializeField] Transform[] m_WayPoints;
    LTSpline m_Spline;

    [SerializeField] int m_NSegmentsX;
    [SerializeField] int m_NSegmentsZ;
    [SerializeField] float m_TubeNominalRadius;
    [SerializeField] AnimationCurve m_TubeRadiusCurve;

    [SerializeField] bool m_GenerateCollider = false;

    private void Awake()
    {
        m_Spline = new LTSpline(m_WayPoints.Select(item => item.position).ToArray());

        m_MeshFilter = GetComponent<MeshFilter>();
        m_MeshFilter.sharedMesh = Generate3DPipeMesh((kx, kz) => 1* m_TubeRadiusCurve.Evaluate(kx), !m_GenerateCollider, m_GenerateCollider);
    }

    Mesh Generate3DPipeMesh(ComputeValueDelegate radiusFunction, bool useNormals = true, bool flipTriangles = false)
    {
        Mesh mesh = new Mesh();
        mesh.name = "3DPipe";

        int effectiveNSegmentsZ = m_GenerateCollider ? m_NSegmentsZ / 2 : m_NSegmentsZ;
        Vector3[] vertices = new Vector3[(m_NSegmentsX + 1) * (effectiveNSegmentsZ + 1)];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[m_NSegmentsX * effectiveNSegmentsZ * 2 * 3];

        int index = 0;
        Vector3 prevTangent = Vector3.zero;
        Vector3 baseVector = Vector3.zero;

        for (int i = 0; i < m_NSegmentsX + 1; i++)
        {
            float kx = (float)i / (m_NSegmentsX+1);
            float nextKx = (float)(i + 1) / (m_NSegmentsX + 1);

            Vector3 circleCenter = m_Spline.point(kx);
            Vector3 nextCircleCenter = m_Spline.point(nextKx);

            Vector3 tangent = i < m_NSegmentsX ? (nextCircleCenter - circleCenter).normalized : prevTangent;
            if(i==0) baseVector = Quaternion.LookRotation(tangent, Vector3.up) * Vector3.up;
            else
                baseVector = (Quaternion.FromToRotation(prevTangent, tangent) * baseVector).normalized;

            prevTangent = tangent;

            for (int j = 0; j < effectiveNSegmentsZ + 1; j++)
            {
                float kz = (float)j / effectiveNSegmentsZ;
                float radius = m_TubeNominalRadius*radiusFunction(kx, kz);

                float angleZ = Mathf.Lerp(0, 360f, 1 - kz);

                Quaternion rotQ = Quaternion.AngleAxis(angleZ, tangent);
                Vector3 rotatedBaseVector = rotQ * baseVector;
                Vector3 pos = circleCenter + rotatedBaseVector * radius;

                vertices[index] = pos;

                normals[index] = (pos - circleCenter).normalized;

                uv[index++] = new Vector2(kx, kz);
            }
        }

        index = 0;

        for (int i = 0; i < m_NSegmentsX; i++)
        {
            int offset = i * (effectiveNSegmentsZ + 1);
            for (int j = 0; j < effectiveNSegmentsZ; j++)
            {
                triangles[index++] = offset + j;
                triangles[index++] = offset + j + 1;
                triangles[index++] = offset + j + effectiveNSegmentsZ + 1;

                triangles[index++] = offset + j + effectiveNSegmentsZ + 2;
                triangles[index++] = offset + j + effectiveNSegmentsZ + 1;
                triangles[index++] = offset + j + 1;
            }
        }


        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        //mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
        
    }

	private void OnDrawGizmos()
	{
        if (m_Spline != null) m_Spline.drawGizmo(Color.red);

    }
}
