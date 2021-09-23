using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    float m_WalkSpeed = 0.01f;
    float m_RunSpeed = 0.05f;
    float m_JumpSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
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
            transform.position += Vector3.left * whichSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * whichSpeed;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * whichSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * whichSpeed;
        }
    }
}
