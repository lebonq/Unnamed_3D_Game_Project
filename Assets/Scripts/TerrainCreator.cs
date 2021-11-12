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

    public MeshFilter m_Mf;
    [SerializeField] int m_XSize;
    [SerializeField] int m_ZSize;

    public GameObject trunkPart;
    public GameObject leaves;

    private void Awake()
    {
        m_Mf = GetComponent<MeshFilter>();

        float offSetX = Random.Range(-200, 200);
        float offSetZ = Random.Range(-200, 200);

        m_Mf.sharedMesh = GenerateTerrainFromHeightFunction(m_XSize, m_ZSize, new Vector3(m_XSize, 64, m_ZSize),
            (kX, kZ) => MyNoise.noiseMap(kX*(m_XSize/500)+offSetX,kZ*(m_ZSize/500)+offSetZ));

        gameObject.AddComponent<MeshCollider>();

        place_trees();
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
        mesh.name = "Terrain";

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

    void place_trees()
    {
        float nb_trees = Random.Range(80, 150);

        Vector3[] vert = m_Mf.sharedMesh.vertices;

        for (int i = 0; i<nb_trees; i++)
        {
            float posx_tree = Random.Range(0, 2000);
            float posz_tree = Random.Range(0, 2000);

            Vector3 newPos = new Vector3(0,0,0);
            newPos.x = posx_tree;
            newPos.z = posz_tree;

            for (int tree = 0; tree < vert.Length; tree++)
            {
                if ((vert[tree].x - newPos.x) * (vert[tree].x - newPos.x) + (vert[tree].z - newPos.z) * (vert[tree].z - newPos.z) < 5)
                {
                   
                    newPos.y = vert[i].y - 40;
    
                    break;
                }
            }

            create_tree(newPos);
        }
    }

    void create_tree(Vector3 newPos)
    {
        GameObject TreeGatherer = GameObject.Find("TreeGatherer");
        int treeh = Random.Range(20, 80);
        Vector3[] pointList;
        Vector3 p0 = newPos;

        pointList = new Vector3[treeh];
        pointList[0] = p0;

        // the trunk
        for (int points = 1; points<treeh; points++)
        {
            Vector3 this_tree_pos = new Vector3(0,0,0);
            this_tree_pos.x = pointList[points - 1].x + Random.Range(-2, 2);
            this_tree_pos.y = pointList[points - 1].y + Random.Range(1, 5);
            this_tree_pos.z = pointList[points - 1].z + Random.Range(-2, 2);

            pointList[points] = this_tree_pos;

            Vector3 CreateAtPosition = pointList[points - 1] + (pointList[points] - pointList[points - 1]);
            GameObject prefabtrunk = Instantiate(trunkPart, CreateAtPosition, Quaternion.identity) as GameObject;
            prefabtrunk.transform.parent = TreeGatherer.transform;
        }

        GameObject prefableaves = Instantiate(leaves, pointList[treeh - 1], Quaternion.identity) as GameObject; // leaves on top
        prefableaves.transform.parent = TreeGatherer.transform;

    }
}
