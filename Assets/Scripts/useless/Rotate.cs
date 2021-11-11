using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField] float m_RotSpeed = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 euelerAngles = transform.eulerAngles;
        euelerAngles.y += Time.deltaTime*m_RotSpeed;

        transform.eulerAngles = euelerAngles;
    }
}
