using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    TMP_Text scoreText;
    int prevScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentScore = Global.playerScore;
        if (prevScore != currentScore)
        {
            scoreText.SetText("SCORE = " + currentScore);
            prevScore = currentScore;
        }
    }
}
