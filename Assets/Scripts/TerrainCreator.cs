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
    public GameObject Monster;


    GameObject TreeCollector; // là où placer tt les instances creees
    GameObject MonsterCollector;

    Vector3[] vert;

    private void Awake()
    {
        m_Mf = GetComponent<MeshFilter>();

        float offSetX = Random.Range(-200, 200);
        float offSetZ = Random.Range(-200, 200);

        m_Mf.sharedMesh = GenerateTerrainFromHeightFunction(m_XSize, m_ZSize, new Vector3(m_XSize, 64, m_ZSize),
            (kX, kZ) => MyNoise.noiseMap(kX*(m_XSize/500)+offSetX,kZ*(m_ZSize/500)+offSetZ));

        gameObject.AddComponent<MeshCollider>();

        vert = m_Mf.sharedMesh.vertices;

        TreeCollector = new GameObject("TreeCollector");
        place_trees();

        MonsterCollector = new GameObject("MonsterCollector");
        place_monsters();

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

    float calc_distance(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.x - b.x,2) + Mathf.Pow(a.z - b.z,2));
    }

    void place_trees()
    {
        float nb_trees = Random.Range(20, 100); // nombre d'arbres

        for (int tree = 0; tree < nb_trees; tree++)
        {
            
            float posx_tree = Random.Range(m_Mf.sharedMesh.bounds.min.x + 50, m_Mf.sharedMesh.bounds.max.x - 50);
            float posz_tree = Random.Range(m_Mf.sharedMesh.bounds.min.z + 50, m_Mf.sharedMesh.bounds.max.z - 50); // position random

            Vector3 newPos = new Vector3(0,0,0);
            newPos.x = posx_tree;
            newPos.z = posz_tree;

            int idx_min = idx_dist_min(newPos); // l'index min correspondant

            newPos.y = vert[idx_min].y - 10;

            create_tree(newPos);
        }
    }

    void create_tree(Vector3 newPos) // cree un seul arbre
    {
       
        int treeh = Random.Range(40, 80); // taille random de l'arbre
   
        List<Vector3> branchlist = new List<Vector3>(); // chaque debut de branche de l'arbre

        Vector3 precedent_trunk = newPos;  // on commence au debut

        // the trunk
        for (int points = 1; points<treeh; points++)
        {
            precedent_trunk = place_trunk_block(precedent_trunk); // on place le nouveau truk et on recupere sa position

            if(Random.Range(0,4) == 0 && points > 30 && points < 65) // 20% de chance d'avoir une branche a chaque trunk
            {
                branchlist.Add(precedent_trunk); // add a branch to the tree.
            }

        } // create the trunk

        place_leaves_block(precedent_trunk);
        // l'arbre de base est fini

        /*
        //maintenant, les branches
        for(int b = 0; b < branchlist.Count; b++) // pour chaque debut de branche, on cree la suite
        {
            int direction_sprout_x = Random.Range(0, 1) == 0 ? -1 : 1; // on prend une valeur random. si elle est == 0, on dit croissance negative, sinon positive
            int direction_sprout_z = Random.Range(0, 1) == 0 ? -1 : 1; // la  branche croit toujours dans la meme direction

            int branch_len = Random.Range(6, 15); //longueur de la branche

            Vector3 block_precedent = branchlist[b]; // le block précédent sur la branche. On commence a celui dans la liste
            for(int block = 0; block < branch_len; block++)
            {
                block_precedent = place_branch_block(block_precedent, direction_sprout_x, direction_sprout_z); // on avance d'un block
            }
            place_leaves_block(block_precedent); // la branche actuelle est finie

        }*/
    }


    Vector3 place_trunk_block(Vector3 position_precedent) // to place blocks on the tree trunk
    {
        Vector3 position_actual;
        position_actual.x = position_precedent.x + Random.Range(-4, 4); ;
        position_actual.y = position_precedent.y + Random.Range(1, 5);
        position_actual.z = position_precedent.z + Random.Range(-4, 4);// position random par rapport au précédent, mais proche et toujours plus haut

        GameObject prefabtrunk = Instantiate(trunkPart, position_actual, Quaternion.identity) as GameObject;
        prefabtrunk.transform.parent = TreeCollector.transform; // cree le nouveau trunk

        return position_actual;
    }

    void place_leaves_block(Vector3 position) // to place leaves at the end
    {
        GameObject prefableaves = Instantiate(leaves, position, Quaternion.identity) as GameObject; // leaves on top
        prefableaves.transform.parent = TreeCollector.transform; // place leaves on top
    }

    Vector3 place_branch_block(Vector3 position_precedent, int x_factor, int z_factor) // to place blocks on the tree branch
    {
        Vector3 position_actual;
        position_actual.x = position_precedent.x + Random.Range(0, 4) * x_factor;
        position_actual.y = position_precedent.y + Random.Range(1, 3);
        position_actual.z = position_precedent.z + Random.Range(0, 4) * z_factor;// position random par rapport au précédent, mais proche et toujours plus haut, et toujours meme direction

        GameObject prefabtrunk = Instantiate(trunkPart, position_actual, Quaternion.identity) as GameObject;
        prefabtrunk.transform.parent = TreeCollector.transform; // cree le nouveau trunk

        return position_actual;
    }

    void place_monsters()
    {
        float nb_monsters = Random.Range(50, 150); // nombre de monstres

        for (int mobs = 0; mobs < nb_monsters; mobs++)
        {
            Vector3 newPosMob = new Vector3(0, 0, 0);

            float posx_mob = Random.Range(m_Mf.sharedMesh.bounds.min.x + 50, m_Mf.sharedMesh.bounds.max.x - 50);
            float posz_mob = Random.Range(m_Mf.sharedMesh.bounds.min.z + 50, m_Mf.sharedMesh.bounds.max.z - 50); // position random

            newPosMob.x = posx_mob;
            newPosMob.z = posz_mob;

            int idx_min = idx_dist_min(newPosMob); // l'index min correspondant

            newPosMob.y = vert[idx_min].y + 10;


            GameObject prefabmonster = Instantiate(Monster, newPosMob, Quaternion.identity) as GameObject;
            prefabmonster.transform.parent = MonsterCollector.transform; // cree le monstre et stock le
        }
    }

    int idx_dist_min(Vector3 pos_object)
    {
        Vector3[] vert = m_Mf.sharedMesh.vertices;
        float minidist = Mathf.Infinity; // la distance minimum
        int idx_min = 0; // l'index min correspondant

        for (int v = 0; v < vert.Length; v++)
        { // calcul des distances
            float newdist = calc_distance(new Vector3(pos_object.x, 0, pos_object.z), vert[v]);

            if (newdist < minidist)
            {
                minidist = newdist;
                idx_min = v;
            }

        } // ici on a la distance a la vertex min et son index
        return idx_min;
    }
}
