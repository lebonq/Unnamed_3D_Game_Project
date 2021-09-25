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

        Vector3 moveVect = transform.forward * vInput * Time.fixedDeltaTime * m_TranslationSpeed;
        m_Rb.MovePosition(m_Rb.position+moveVect);
        m_Rb.MoveRotation(Quaternion.AngleAxis(hInput*Time.fixedDeltaTime*m_RotationSpeed,Vector3.up) * transform.rotation);

        m_Rb.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision){

    }
}
