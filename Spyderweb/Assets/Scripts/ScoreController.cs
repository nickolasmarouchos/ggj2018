using System;

public class ScoreController
{
    public int maxScorePerFly = 10;
    public int scorePerNode = 1;

    ScoreDisplay scoreDisplay;
    int totalScore = 0;

    public ScoreController(ScoreDisplay display)
    {
        scoreDisplay = display;
    }

    public void EatFly(float flyLife, int nodeCount)
    {
        totalScore += (int)Math.Round(flyLife * maxScorePerFly) + nodeCount * scorePerNode;
        scoreDisplay.SetScore(totalScore);
    }
}