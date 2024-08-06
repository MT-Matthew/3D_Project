using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance {get; set;}

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public UnityEvent OnDayPass = new UnityEvent();

    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public Season currentSeason = Season.Spring;

    public enum DayOfWeak
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public DayOfWeak currentDayOfWeak = DayOfWeak.Monday;

    public int daysPerSeason = 30;
    public int daysInCurrentSeason = 1;

    public int dayInGame = 1;
    public int yearInGame = 0;

    public TextMeshProUGUI dayUI;

    void Start()
    {
        UpdateUI();
    }

    public void TriggerNextDay()
    {
        dayInGame += 1;
        daysInCurrentSeason += 1;

        currentDayOfWeak = (DayOfWeak)(((int)currentDayOfWeak + 1) % 7);

        if (daysInCurrentSeason > daysPerSeason)
        {
            daysInCurrentSeason = 1;
            currentSeason = GetNextSeason();
        }
        
        UpdateUI();
        OnDayPass.Invoke();
    }

    void UpdateUI()
    {
        dayUI.text = $"{currentDayOfWeak} {daysInCurrentSeason}, {currentSeason}";
    }

    Season GetNextSeason()
    {
        int currentSeasonIndex = (int)currentSeason;
        int nextSeasonIndex = (currentSeasonIndex + 1) % 4;
        if (nextSeasonIndex == 0)
        {
            yearInGame += 1;
        }
        return (Season)nextSeasonIndex;
    }
}
