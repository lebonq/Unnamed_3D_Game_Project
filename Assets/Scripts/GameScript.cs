using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameScript : MonoBehaviour
{

    public float maxTime = 500;
    public float timeLeft;
    // Start is called before the first frame update

    void Start()
    {
        timeLeft = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;

        if(timeLeft < 0)
        {
            GameOver(1);
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.fontSize = 40;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(0, 0, 100, 100), "Time left : " + timeLeft + "s", style);

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
