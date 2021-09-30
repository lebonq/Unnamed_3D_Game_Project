using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHead : MonoBehaviour
{
    public Vector2 turn;
    public float sensitivity = 10f;
    Vector3 deltaMove;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        turn.x += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        turn.y += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
