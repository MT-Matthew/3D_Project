using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    
    public Light directionalLight;

    public float dayDurationInSeconds = 24f;
    public int currentHour;
    float currentTimeOfDay = 0.35f; // equal to 8 AM

    public List<SkyboxTimeMapping> timeMappings;

    float blendedValue = 0f;
    bool lockNextDayTrigger = false;

    public TextMeshProUGUI timeUI;

    public WeatherSystem weatherSystem;


    void Update()
    {
        // calculate the current time of day based on the game time
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1; // Ensure it stays between 0 and 1

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        timeUI.text = $"{currentHour}:00";

        // update the direction light's rotation
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) -90, 170, 0));

        // update the skybox material based on the time of day
        if (weatherSystem.isSpecialWeather == false)
        {
            UpdateSkybox();
        }

        if (currentHour == 0 && lockNextDayTrigger == false)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }

        if (currentHour != 0)
        {
            lockNextDayTrigger = false;
        }
        
    }

    void UpdateSkybox()
    {
        Material currentSkybox = null;
        foreach (SkyboxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;

                if (currentSkybox.shader != null)
                {
                    if (currentSkybox.shader.name == "Custom/SkyboxTransition")
                    {
                        blendedValue += Time.deltaTime;
                        blendedValue = Mathf.Clamp01(blendedValue);

                        currentSkybox.SetFloat("_TransitionFactor", blendedValue);
                    }
                    else
                    {
                        blendedValue = 0;
                    }
                }

                break;
            }
        }


        if (currentSkybox != null)
        {
            RenderSettings.skybox = currentSkybox;
        }

    }
}

[System.Serializable]
public class SkyboxTimeMapping
{
    public string phaseName;
    public int hour;
    public Material skyboxMaterial;
}
