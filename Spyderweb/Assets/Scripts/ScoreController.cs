using System;

public class ScoreController
{
    public int maxScorePerFly = 10;
    public int scorePerNode = 1;

    Score score;

    ScoreDisplay scoreDisplay;
    //int totalScore = 0;

    public ScoreController(ScoreDisplay display)
    {
        scoreDisplay = display;
        score = new Score(0, 6);
        scoreDisplay.SetScore(score);
    }

    public void EatFly(float flyLife, int nodeCount)
    {
        ++score.flies;
        //totalScore += /* (int)Math.Round(flyLife * maxScorePerFly) + */ nodeCount * scorePerNode;
        scoreDisplay.SetScore(score);
    }

    public void CreateNode(int maxNodes)
    {
        score.nodes = maxNodes;
        scoreDisplay.SetScore(score);
    }

    internal Score GetScore()
    {
        return score;
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