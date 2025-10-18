using System;
using UnityEngine;

public static class ScoreData
{
    public static int currentLevel = 0;

    public static int[] levelScores = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

    public static bool[] completedLevels = new bool[8] { true, false, false, false, false, false, false, false };

    public static int TotalStars()
    {
        int sumScore = 0;

        for (int i = 0; i < levelScores.Length; i++)
        {
            Debug.Log("level " + i + " is " + levelScores[i]);
            sumScore += levelScores[i];
        }

        Debug.Log(sumScore);
        return sumScore;
    }
    
    public static void CalculateLevel(int score)
    {
        switch (currentLevel)
        {
            case 1:
                IsScoreHighscore(score, 1);
                break;
            case 2:
                IsScoreHighscore(score, 2);
                break;
            case 3:
                IsScoreHighscore(score, 3);
                break;
            case 4:
                IsScoreHighscore(score, 4);
                break;
            case 5:
                IsScoreHighscore(score, 5);
                break;
            case 6:
                IsScoreHighscore(score, 6);
                break;
            case 7:
                IsScoreHighscore(score, 7);
                break;
            case 8:
                IsScoreHighscore(score, 8);
                break;
        }
    }

    private static void IsScoreHighscore(int score, int level)
    {
        if (levelScores[level-1] < score)
        {
            levelScores[level - 1] = score;
            Debug.Log(score);
        }
    }
}
