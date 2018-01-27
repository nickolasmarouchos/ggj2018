using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreDisplay : MonoBehaviour
{
    string baseString = "Score: ";
    string currentString = "Score: 0";

    private void Update()
    {
        // TODO: draw score
    }

    public void SetScore (int score)
    {
        currentString = baseString + score;
        Debug.Log(currentString);
    }
}