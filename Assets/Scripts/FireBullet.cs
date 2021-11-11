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
            Vector3 pos = transform.position; //On deplace la position de spawn de bullet pour eviter d'entree en colisiob avec le player
            pos.x -= transform.forward.x*2;
            pos.z -= transform.forward.z*2;
            pos.y += 0.5f;
            GameObject prefabcopy = Instantiate(Bullet, pos, Quaternion.identity) as GameObject;
            prefabcopy.GetComponent<Rigidbody>().AddForce(transform.up * 1500);
            Destroy(prefabcopy, 10.0f);
        }
    }
}
