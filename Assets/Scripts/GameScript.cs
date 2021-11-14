using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{

    public float timeLeft = 500f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnGUI();
        timeLeft -= Time.deltaTime;


        if(timeLeft < 0)
        {
            GameOver(1);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), "Time left : " + timeLeft + "s");

    }

    void GameOver(int type)
    {
        if(type == 1)
        {
            ;
        }

        if(type == 2)
        {
            ;
        }
    }
}
