using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    [Range(0f, 1f)]
    public float chanceToRainSpring = 0.3f; // 30%

    [Range(0f, 1f)]
    public float chanceToRainSummer = 0.7f; // 70%

    [Range(0f, 1f)]
    public float chanceToRainFall = 0.1f;

    [Range(0f, 1f)]
    public float chanceToRainWinter = 0.3f;

    public GameObject rainEffect;
    public Material rainSkybox;

    public bool isSpecialWeather;

    public AudioSource rainChannel;
    public AudioClip rainsound;

    public enum WeatherCondition
    {
        Sunny,
        Rainy
    }

    public WeatherCondition currentWeather = WeatherCondition.Sunny;

    void Start()
    {
        TimeManager.Instance.OnDayPass.AddListener(GenerateRandomWeather);
    }

    void GenerateRandomWeather()
    {
        TimeManager.Season currentSeason = TimeManager.Instance.currentSeason;
        float chanceToRain = 0f;

        switch (currentSeason)
        {
            case TimeManager.Season.Spring:
                chanceToRain = chanceToRainSpring;
                break;
            case TimeManager.Season.Summer:
                chanceToRain = chanceToRainSummer;
                break;
            case TimeManager.Season.Fall:
                chanceToRain = chanceToRainFall;
                break;
            case TimeManager.Season.Winter:
                chanceToRain = chanceToRainWinter;
                break;
        }

        if (UnityEngine.Random.value <= chanceToRain)
        {
            currentWeather = WeatherCondition.Rainy;
            isSpecialWeather = true;

            Invoke("StartRain", 1f);
        }
        else
        {
            currentWeather = WeatherCondition.Sunny;
            isSpecialWeather = false;

            StopRain();
        }

    }

    private void StopRain()
    {
        if (rainChannel.isPlaying)
        {
            rainChannel.Stop();
        }

        rainEffect.SetActive(false);
    }

    private void StartRain()
    {
        if (rainChannel.isPlaying == false)
        {
            rainChannel.clip = rainsound;
            rainChannel.loop = true;
            rainChannel.Play();
        }

        RenderSettings.skybox = rainSkybox;
        rainEffect.SetActive(true);
    }
}
