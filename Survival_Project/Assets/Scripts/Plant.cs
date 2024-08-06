using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] GameObject seedModel;
    [SerializeField] GameObject youngPlantModel;
    [SerializeField] GameObject maturePlantModel;

    [SerializeField]  List<GameObject> plantProduceSpawns;
    [SerializeField]  GameObject producePrefab;

    public int dayOfPlanting;
    [SerializeField] int plantAge = 0;

    [SerializeField] int ageForYoungModel;
    [SerializeField] int ageForMatureModel;
    [SerializeField] int ageForFirstProduceBatch;

    [SerializeField] int daysForNewProduce;
    [SerializeField] int daysRemainingForNewProduceCounter;

    [SerializeField] bool isOneTimeHarvest;
    public bool isWatered;

    void OnEnable()
    {
        TimeManager.Instance.OnDayPass.AddListener(DayPass);
    }

    void OnDisable()
    {
        TimeManager.Instance.OnDayPass.RemoveListener(DayPass);
    }


    void DayPass()
    {
        if (isWatered)
        {
            plantAge++;
            isWatered = false;
            GetComponentInParent<Soil>().MakeSoilNotWatered();
        }
        
        CheckGrowth();

        if (isOneTimeHarvest == false)
        {
            CheckProduce();
        }
        
    }

    void CheckGrowth()
    {
        seedModel.SetActive(plantAge < ageForYoungModel);
        youngPlantModel.SetActive(plantAge >= ageForYoungModel && plantAge < ageForMatureModel);
        maturePlantModel.SetActive(plantAge >= ageForMatureModel);

        if (plantAge >= ageForMatureModel && isOneTimeHarvest)
        {
            MakePlantPickable();
        }
    }

    private void MakePlantPickable()
    {
        GetComponent<InteractableObject>().enabled = true;
        if (GetComponent<SphereCollider>())
        {
            GetComponent<SphereCollider>().enabled = true;
        }
    }

    void CheckProduce()
    {
        if (plantAge == ageForFirstProduceBatch)
        {
            GenerateProduceForEmptySpawns();
        }

        if (plantAge > ageForFirstProduceBatch)
        {
            if (daysRemainingForNewProduceCounter == 0)
            {
                GenerateProduceForEmptySpawns();
                daysRemainingForNewProduceCounter = daysForNewProduce;
            }
            else
            {
                daysRemainingForNewProduceCounter--;
            }
        }
    }

    void GenerateProduceForEmptySpawns()
    {
        foreach (GameObject spawn in plantProduceSpawns)
        {
            if (spawn.transform.childCount == 0)
            {
                GameObject produce = Instantiate(producePrefab);

                produce.transform.SetParent(spawn.transform, false);
                // produce.transform.parent = spawn.transform;

                Vector3 producePossition = Vector3.zero;
                producePossition.y = 0f;
                produce.transform.localPosition = producePossition;
            }
        }
    }

    void OnDestroy()
    {
        if (GetComponentInParent<Soil>())
        {
            GetComponentInParent<Soil>().isEmpty = true;
            GetComponentInParent<Soil>().plantName = "";
            GetComponentInParent<Soil>().currentPlant = null;
        }
        
    }
    

}
