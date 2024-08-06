using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaterSource {
    Lake,
    River,
    Ocean
}

public class FishingSystem : MonoBehaviour
{
    public static FishingSystem Instance { get; set; }

    private void Awake()
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

    public bool isThereABite;
    bool hasPulled;
    public static event Action OnFishingEnd;
    public GameObject minigame;

    FishData fishBiting;

    public FishMovement fishMovement;

    public void StartFishing(WaterSource waterSource)
    {
        StartCoroutine(FishingCoroutine(waterSource));
    }

    IEnumerator FishingCoroutine(WaterSource waterSource)
    {
        yield return new WaitForSeconds(3f);
        FishData fish = CalculateBite(waterSource);

        if (fish.fishName == "NoBite")
        {
            Debug.LogWarning("No Fish caught");
            EndFishing();
        }
        else
        {
            Debug.LogWarning(fish.fishName + " is biting");
            StartCoroutine(StartFishStruggle(fish));
        }
    }

    IEnumerator StartFishStruggle(FishData fish)
    {
        isThereABite = true;

        while (hasPulled == false)
        {
            yield return null;
        }

        // Debug.LogWarning("Start Minigame");

        fishBiting = fish;
        StartMinigame();
    }

    private void StartMinigame()
    {
        minigame.SetActive(true);
        fishMovement.SetDifficulty(fishBiting);
    }

    public void SetHasPull()
    {
        hasPulled = true;
    }

    private void EndFishing()
    {
        isThereABite = false;
        hasPulled = false;
        fishBiting = null;

        OnFishingEnd?.Invoke();

        var slot = EquipSystem.Instance.selectedNumber;

        EquipSystem.Instance.SelectQuickSlot(slot);
        EquipSystem.Instance.SelectQuickSlot(slot);
    }

    FishData CalculateBite(WaterSource waterSource)
    {
        List<FishData> availableFish = GetAvailableFish(waterSource);

        float totalProbability = 0f;
        foreach (FishData fish in availableFish)
        {
            totalProbability += fish.probability;
        }

        int randomValue = UnityEngine.Random.Range(0, Mathf.FloorToInt(totalProbability) + 1);
        // Debug.Log("Random is: " + randomValue);
        float cumulativeProbability = 0f;
        foreach (FishData fish in availableFish)
        {
            cumulativeProbability += fish.probability;
            if (randomValue <= cumulativeProbability)
            {
                return fish;
            }
        }

        return null;

    }

    private List<FishData> GetAvailableFish(WaterSource waterSource)
    {
        switch (waterSource)
        {
            case WaterSource.Lake:
                return lakeFishList;
            case WaterSource.River:
                return riverFishList;
            case WaterSource.Ocean:
                return oceanFishList;

            default:
                return null;
        }
    }

    internal void EndMinigame(bool success)
    {
        minigame.gameObject.SetActive(false);

        if (success)
        {
            InventorySystem.Instance.AddToInventory(fishBiting.fishName);
            EndFishing();
        }
        else
        {
            EndFishing();
        }
    }

    public List<FishData> lakeFishList;
    public List<FishData> riverFishList;
    public List<FishData> oceanFishList;







}
