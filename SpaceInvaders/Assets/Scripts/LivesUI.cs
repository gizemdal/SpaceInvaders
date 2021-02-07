using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LivesUI : MonoBehaviour
{
    TMP_Text livesText;    
    int lives = 0;
    // Start is called before the first frame update
    void Start()
    {
        livesText = gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int remainingLives = Ship.remainingLives;
        if (lives != remainingLives)
        {
            livesText.SetText("LIVES = " + remainingLives);
            lives = remainingLives;
        }
    }
}
