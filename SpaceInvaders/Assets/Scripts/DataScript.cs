using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataScript
{
    private static int score, level;

    public static int getLevel()
    {
        return level;
    }

    public static void setLevel(int value)
    {
        level = value;
    }

    public static int getScore()
    {
        return score;
    }
    public static void setScore(int value)
    {
        score = value;
    }
}
