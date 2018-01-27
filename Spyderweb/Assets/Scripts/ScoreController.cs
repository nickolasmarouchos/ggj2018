using System;

public class ScoreController
{
    public int maxScorePerFly = 1000;
    public int scorePerNode = 100;

    ScoreDisplay scoreDisplay;
    int totalScore = 0;

    ScoreController(ScoreDisplay display)
    {
        scoreDisplay = display;
    }

    public void EatFly(float flyLife, int nodeCount)
    {
        totalScore += (int)Math.Round(flyLife * maxScorePerFly) + nodeCount * scorePerNode;
    }
}