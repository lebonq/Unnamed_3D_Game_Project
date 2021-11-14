using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathTools;

public class PlayerScript : MonoBehaviour
{
    public float m_TranslationSpeed;

    Rigidbody m_Rb;
    Transform m_RbHead;

    float vInput;
    float hInput;

    public Vector2 turn;
    public float sensitivityY = 2f;
    public float sensitivityX = 4f;
    Quaternion originalRotation;

    bool dead = false;


    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localRotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        Vector3[] vert = GameObject.Find("Terrain").GetComponent<MeshFilter>().sharedMesh.vertices;
        Vector3 newPos = transform.position;

        float minidist = Mathf.Infinity; // la distance minimum
        int idx_min = 0; // l'index min correspondant

        for (int v = 0; v < vert.Length; v++)
        { // calcul des distances
            float newdist = calc_distance(new Vector3(newPos.x, 0, newPos.z), vert[v]);

            if (newdist < minidist)
            {
                minidist = newdist;
                idx_min = v;
            }

        } // ici on a la distance a la vertex min et son index
        newPos.y = vert[idx_min].y + 10;
        transform.position = newPos;
    }

    float calc_distance(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.z - b.z, 2));
    }

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_RbHead = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");
        turn.x += Input.GetAxis("Mouse X") * sensitivityX;
        turn.y += Input.GetAxis("Mouse Y") * sensitivityY;
    }

    private void FixedUpdate()
    {

        float roty = Mathf.Clamp(turn.y, -90f, 90f);
        if (turn.y < roty || turn.y > roty)
        {
            turn.y = roty;
        }
        //Get the rotation you will be at next as a Quaternion
        Quaternion yQuaternion = Quaternion.AngleAxis(roty, Vector3.left);
        Quaternion xQuaternion = Quaternion.AngleAxis(turn.x, Vector3.up);

        //Move body on x axis

        transform.localRotation = originalRotation * xQuaternion;
        //move head on Y axis
        this.gameObject.transform.GetChild(1).transform.localRotation = originalRotation * yQuaternion;

        // Use add force method to change de velocity

            Vector3 velocityDelta = (GetComponentInChildren<Transform>().forward * m_TranslationSpeed * vInput ) + (GetComponentInChildren<Transform>().right * m_TranslationSpeed * hInput);
            //On ajoute la gravité
            m_Rb.AddForce(velocityDelta - m_Rb.velocity, ForceMode.VelocityChange);
        


    }

    public bool isDead()
    {
        return dead;
    }

    public void setDead(){
        dead = true;
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
