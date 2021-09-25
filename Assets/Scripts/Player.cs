using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float m_TranslationSpeed;
    public float m_RotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        transform.position += transform.forward * vInput * Time.deltaTime * m_TranslationSpeed;
        transform.rotation = Quaternion.AngleAxis(hInput*Time.deltaTime*m_RotationSpeed,Vector3.up) * transform.rotation;
    }
}
