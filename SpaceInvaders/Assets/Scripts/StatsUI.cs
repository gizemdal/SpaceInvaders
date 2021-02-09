using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{
    TMP_Text statsText;

    // Start is called before the first frame update
    void Start()
    {
        statsText = gameObject.GetComponent<TMP_Text>();
        statsText.text = "SCORE = " + DataScript.getScore() + "\nLEVEL = " + DataScript.getLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
