using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{

    public GameObject Bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject prefabcopy = Instantiate(Bullet, transform.position, Quaternion.identity) as GameObject;
            prefabcopy.GetComponent<Rigidbody>().AddForce(transform.up * 1500);
            Destroy(prefabcopy, 1.0f);
        }

    }
}
