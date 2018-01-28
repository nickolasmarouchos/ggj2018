using System;

public class ScoreController
{
    public int maxScorePerFly = 10;
    public int scorePerNode = 1;

    int flies = 0;
    int nodes = 0;

    ScoreDisplay scoreDisplay;
    int totalScore = 0;

    public ScoreController(ScoreDisplay display)
    {
        scoreDisplay = display;
    }

    public void EatFly(float flyLife, int nodeCount)
    {
        ++flies;
        totalScore += /* (int)Math.Round(flyLife * maxScorePerFly) + */ nodeCount * scorePerNode;
        scoreDisplay.SetScore(totalScore);
    }

    public void CreateNode(int maxNodes)
    {
        nodes = maxNodes;
    }

    internal Score GetScore()
    {
        return new Score(flies, nodes);
    }
}

public struct Score
{
    public int nodes;
    public int flies;

    public Score (int flies, int nodes)
    {
        this.nodes = nodes;
        this.flies = flies;
    }
}