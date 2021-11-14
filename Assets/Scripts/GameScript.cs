using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameScript : MonoBehaviour
{

    public int maxTime = 500;
    int timeLeft;
    // Start is called before the first frame update

    void Start()
    {
        timeLeft = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= (int)(Mathf.Round( Time.deltaTime));

        if(timeLeft < 0)
        {
            GameOver(0);
        }

        PlayerScript player = GameObject.Find("Player").GetComponent<PlayerScript>();

        if (player.isDead())
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

        if (timeLeft >= 0) GUI.Label(new Rect(0, 0, 100, 100), "Time left : " + timeLeft + "s", style);

    }

    void GameOver(int type)
    {
        if(type == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }

        if (type == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

        }
    }
}
