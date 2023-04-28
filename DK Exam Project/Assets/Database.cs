using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Database 
{
    public static MonthData[] data;

    static Database()
    {
        data = new MonthData[11];
        data[0] = new MonthData(23, 22, 21, 34, 17, 16, 20, 26, 21);
        // Initialization code for the static members goes here
    }

    public static MonthData getMonthData(int i)
    {
        return data[i];
    }


}

public class MonthData
{
    // Priority percent numbers
    public float[] priority;

    // Age percent numbers
    public float[] age;

    public MonthData(float priorityOnePercent, float priorityTwoPercent, float priorityThreePercent, float priorityFourPercent,
        float ageOnePercent, float ageTwoPercent, float ageThreePercent, float ageFourPercent, float ageFivePercent)
    {

        priority = new float[5];
        age = new float[5];

        priority[0] = priorityOnePercent;
        priority[1] = priorityTwoPercent;
        priority[2] = priorityThreePercent;
        priority[3] = priorityFourPercent;

        age[0] = ageOnePercent;
        age[1] = ageTwoPercent;
        age[2] = ageThreePercent;
        age[3] = ageFourPercent;
        age[4] = ageFivePercent;
    }
}


