using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] float m_RotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(new Vector3(0, transform.position.y, 0), Vector3.up, m_RotationSpeed * Time.deltaTime);
        transform.LookAt(new Vector3(0, transform.position.y, 0));
    }
}
