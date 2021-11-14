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

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localRotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        Vector3[] vert = GameObject.Find("Terrain").GetComponent<MeshFilter>().sharedMesh.vertices;
        Vector3 newPos = transform.position;
        for (int i = 0; i < vert.Length; i++)
        {
            if ((vert[i].x - newPos.x) * (vert[i].x - newPos.x) + (vert[i].z - newPos.z) * (vert[i].z - newPos.z) < 20)
            {
                newPos.y = vert[i].y + 5;
                break;
            }
        }
        transform.position = newPos;
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
        if (vInput != 0 || hInput != 0)
        {
            Vector3 velocityDelta = GetComponentInChildren<Transform>().forward * m_TranslationSpeed * vInput - m_Rb.velocity;
            m_Rb.AddForce(velocityDelta, ForceMode.VelocityChange);
            m_Rb.AddForce(transform.up * 9.81f, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
