using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourMonster : MonoBehaviour
{
    Rigidbody m_Rb;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");
        float playerX = player.transform.position.x;
        float playerZ = player.transform.position.z;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance > Vector3.Distance(player.transform.position, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z))) m_Rb.AddForce(new Vector3(-0.25f, 0, 0), ForceMode.VelocityChange);
        else if (distance > Vector3.Distance(player.transform.position, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z))) m_Rb.AddForce(new Vector3(0.25f, 0, 0), ForceMode.VelocityChange);
        if (distance > Vector3.Distance(player.transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1))) m_Rb.AddForce(new Vector3(0, 0, -0.25f), ForceMode.VelocityChange);
        else if (distance > Vector3.Distance(player.transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1))) m_Rb.AddForce(new Vector3(0, 0, 0.25f), ForceMode.VelocityChange);

        m_Rb.velocity = Vector3.ClampMagnitude(m_Rb.velocity, 4f);

        m_Rb.AddForce(new Vector3(0.3f,0,0), ForceMode.Acceleration);
    }
}
