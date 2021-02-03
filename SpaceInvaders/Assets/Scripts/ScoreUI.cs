using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    Text scoreText;
    int prevScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentScore = Global.playerScore;
        if (prevScore != currentScore)
        {
            scoreText.text = "SCORE = " + currentScore;
            prevScore = currentScore;
        }
    }
}
