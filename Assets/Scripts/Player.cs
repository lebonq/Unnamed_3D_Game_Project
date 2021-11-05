using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMathTools;

public class Player : MonoBehaviour
{

    public float m_TranslationSpeed;
    //public float m_RotationSpeed;

    Rigidbody m_Rb;

    public Vector2 turn;
    public float sensitivity = 10f;
    public float speed = 1;


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
    }

    // Update is called once per frame
    void Update()
    {
        turn.x += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        turn.y += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        //transform.localRotation = Quaternion.Euler(0, turn.x, 0);
        // transform.position += transform.forward * vInput * Time.deltaTime * m_TranslationSpeed;
        //transform.localRotation = Quaternion.AngleAxis(turn.x * Time.deltaTime*sensitivity,Vector3.up) * transform.rotation;
    }

    private void FixedUpdate(){
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        // Vector3 moveVect = transform.forward * vInput * Time.fixedDeltaTime * m_TranslationSpeed;
        // m_Rb.MovePosition(m_Rb.position+moveVect);
        m_Rb.MoveRotation(Quaternion.AngleAxis(turn.x*Time.fixedDeltaTime*sensitivity,Vector3.up) * transform.rotation);

        // m_Rb.angularVelocity = Vector3.zero;

        // Use add force method to change de velocity
        if(vInput != 0 || hInput != 0){
            Vector3 velocityDelta = transform.forward * m_TranslationSpeed * vInput - m_Rb.velocity;
            m_Rb.AddForce(velocityDelta,ForceMode.VelocityChange);
            m_Rb.AddForce(Vector3.down, ForceMode.Force);

            //Vector3 angularVelocityDelta = Vector3.up*m_RotationSpeed*Mathf.Deg2Rad*hInput-m_Rb.angularVelocity;
            //m_Rb.AddTorque(angularVelocityDelta, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionEnter(Collision collision){

    }
}
