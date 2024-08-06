using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public bool isEmpty = true;
    public bool playerInRange;
    public string plantName;


    public Plant currentPlant;

    public Material defaultMaterial;
    public Material wateredMaterial;

    

    void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < 10f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }


    public void PlantSeed()
    {
        InventoryItem selectedSeed = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>();
        string onlyPlantName = selectedSeed.thisName.Split(new string[] {" Seed"}, StringSplitOptions.None)[0];

        isEmpty = false;
        plantName = onlyPlantName;

        GameObject instantiatedPlant = Instantiate(Resources.Load($"{onlyPlantName}Plant") as GameObject);

        instantiatedPlant.transform.SetParent(gameObject.transform, false);
        // instantiatedPlant.transform.parent = gameObject.transform;

        Vector3 plantPosition = Vector3.zero;
        plantPosition.y = 0f;
        instantiatedPlant.transform.localPosition = plantPosition;

        currentPlant = instantiatedPlant.GetComponent<Plant>();
        currentPlant.dayOfPlanting = TimeManager.Instance.dayInGame;
    }

    public void MakeSoilWatered()
    {
        GetComponent<Renderer>().material = wateredMaterial;
    }

    public void MakeSoilNotWatered()
    {
        GetComponent<Renderer>().material = defaultMaterial;
    }
}
