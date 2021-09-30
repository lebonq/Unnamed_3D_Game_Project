using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float m_TranslationSpeed;
    public float m_RotationSpeed;

    Rigidbody m_Rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake(){
        m_Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        // transform.position += transform.forward * vInput * Time.deltaTime * m_TranslationSpeed;
        // transform.rotation = Quaternion.AngleAxis(hInput*Time.deltaTime*m_RotationSpeed,Vector3.up) * transform.rotation;
    }

    private void FixedUpdate(){
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        // Vector3 moveVect = transform.forward * vInput * Time.fixedDeltaTime * m_TranslationSpeed;
        // m_Rb.MovePosition(m_Rb.position+moveVect);
        // m_Rb.MoveRotation(Quaternion.AngleAxis(hInput*Time.fixedDeltaTime*m_RotationSpeed,Vector3.up) * transform.rotation);

        // m_Rb.angularVelocity = Vector3.zero;

        // Use add force method to change de velocity
        if(vInput != 0 || hInput != 0){
            Vector3 velocityDelta = transform.forward * m_TranslationSpeed * vInput - m_Rb.velocity;
            m_Rb.AddForce(velocityDelta,ForceMode.VelocityChange);
            m_Rb.AddForce(Vector3.down, ForceMode.Force);

            Vector3 angularVelocityDelta = Vector3.up*m_RotationSpeed*Mathf.Deg2Rad*hInput-m_Rb.angularVelocity;
            m_Rb.AddTorque(angularVelocityDelta, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionEnter(Collision collision){

    }
}
