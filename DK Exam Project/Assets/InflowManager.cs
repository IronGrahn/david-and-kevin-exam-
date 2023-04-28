using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InflowManager : MonoBehaviour
{
    [Header("General information")]
    public int year;
    public int days;
    public int month; 
    string[] months = new string[] 
    { "January", "February",
        "March", "April", "May",
        "June","July", "August",
        "September", "October", "November",
        "December" };

    [Header("Customisable settings")]
    public int numberOfPriorities;

    [Header("Simulated information")]
    public string currentYear;
    public string currentMonth;
    public int numberOfAgeGroups;
    public int totalPatients;
    public int[] ages;

    private MonthData currentMonthData; 
    public AgeGroup[] groups;

    [Header("Priority Percent")]
    public float[] priorityPercent;

    [Header("Age Percent")]
    public float[] agePercent;



    // Start is called before the first frame update
    void Start()
    {
        //initialize arrays
        groups = new AgeGroup[ages.Length]; 
        agePercent = new float[ages.Length];
        numberOfAgeGroups = ages.Length - 1;
        numberOfPriorities = priorityPercent.Length - 1;
        Debug.Log("Age groups are: " + numberOfAgeGroups);
        Debug.Log("Priorities are: " + numberOfPriorities);
        //initialize age groups
        for(int i = 0; i< numberOfAgeGroups; i++)        
            groups[i] = new AgeGroup(ages[i], (ages[i+1])-1);   

        //Init dates
        currentMonth = months[month];
        currentMonthData = Database.getMonthData(month);
        currentYear = year.ToString();

        //initialize percentages 

        for(int i= 0; i < numberOfPriorities;i++)
            priorityPercent[i] = currentMonthData.priority[i];

        for (int i = 0; i < numberOfAgeGroups; i++)
            agePercent[i] = currentMonthData.age[i];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class AgeGroup
{
    private int minimumAge;
    private int maximumAge;

    public AgeGroup(int min, int max)
    {
        minimumAge = min;
        maximumAge = max;
    }
}
