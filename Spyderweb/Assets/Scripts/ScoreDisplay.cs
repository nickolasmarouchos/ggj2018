using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreDisplay : MonoBehaviour
{
    string baseString = "Score: ";
    string currentString = "Score: 0";
    public Text textBox;

    private void Update()
    {

    }

    public void SetScore(Score score)
    {
        if (textBox != null)
            textBox.text = "Highest node count: " + score.nodes + "\n Flies eaten: " + score.flies; 
    }
}