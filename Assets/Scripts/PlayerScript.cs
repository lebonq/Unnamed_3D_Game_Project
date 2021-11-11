using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathTools;

public class PlayerScript : MonoBehaviour
{

    Rigidbody m_Rb;

    public float horizontalmove;
    public float verticalmove;
    Vector3 movedirection;
    public float speed = 6f;
    float speedmultiplier = 10f;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Vector3[] vert = GameObject.Find("Terrain").GetComponent<MeshFilter>().sharedMesh.vertices;
        Vector3 newPos = transform.position;

        for(int i = 0; i < vert.Length; i++)
        {
            if((vert[i].x - newPos.x) * (vert[i].x - newPos.x) + (vert[i].z - newPos.z)* (vert[i].z - newPos.z) < 20)
            {
                newPos.y = vert[i].y + 10;
                break;
            }
        }
        transform.position = newPos;
            

    }

    private void Awake(){
        m_Rb = GetComponent<Rigidbody>();
        m_Rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    void MyInput()
    {
        horizontalmove = Input.GetAxisRaw("Horizontal");
        verticalmove = Input.GetAxisRaw("Vertical");

        movedirection = transform.forward * verticalmove + transform.right * horizontalmove;
    }


    private void FixedUpdate(){

        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 velocityDelta = movedirection.normalized * speed - m_Rb.velocity;
        m_Rb.AddForce(velocityDelta * speedmultiplier, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision){

    }
}
