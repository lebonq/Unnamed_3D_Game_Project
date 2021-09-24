using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    float m_WalkSpeed = 5f;
    float m_RunSpeed = 0.05f;
    float m_JumpSpeed = 0f;

    Rigidbody m_Rb;

    // Start is called before the first frame update
    void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeys();
        
    }

    void CheckKeys()
    {
        float whichSpeed = m_WalkSpeed;
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            whichSpeed = m_RunSpeed;
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_Rb.velocity = Vector3.left * whichSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_Rb.velocity = Vector3.right * whichSpeed;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_Rb.velocity = Vector3.forward * whichSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_Rb.velocity = Vector3.back * whichSpeed;
        }
    }
}
