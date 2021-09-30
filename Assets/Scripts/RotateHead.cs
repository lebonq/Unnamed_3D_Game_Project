using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHead : MonoBehaviour
{
    public Vector2 turn;
    public float sensitivity = 10f;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        turn.x += Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        turn.y += Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

        if(-turn.y<90 && -turn.y > -75)
        {
            transform.localRotation = Quaternion.AngleAxis(-turn.y, Vector3.right);
        }
    }
}
