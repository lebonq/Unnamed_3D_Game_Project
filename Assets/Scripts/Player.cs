using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public m_TranslationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float vInput = Input.getAxis("Vertical");
        float hInput = Input.getAxis("Horizontal");

        transform.position += transform.foward * vInput * Time.deltaTime * m_TranslationSpeed;
    }
}
