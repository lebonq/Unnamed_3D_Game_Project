using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameExitScript : MonoBehaviour
{

    float CumulTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    // Update is called once per frame
    void Update()
    {
        CumulTime += Time.deltaTime;
        if (Input.anyKey && CumulTime > 3)
        {
            SceneManager.LoadScene(0);
        }
    }
}
