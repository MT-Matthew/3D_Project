using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
 
public class SelectionManager : MonoBehaviour
{

    public static SelectionManager Instance {get; set;}

    public GameObject interaction_Info_UI;
    Text interaction_text;
    Transform lastTarget;

    public Image centerDotImage;
    public Image handImage; 

    public bool handIsVisible;

    public bool canPickup;

    public GameObject chopHolder;
    public GameObject selectedTree;

    public GameObject selectedStorageBox;
    public GameObject selectedCampFire;
    public GameObject selectedSoil;



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

 
    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<Text>();
        canPickup = true;
    }
 
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // float rayLength = 100f;
        // Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green);

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            // Debug.Log("Hit: " + selectionTransform);

            InteractableObject targetObject = selectionTransform.GetComponent<InteractableObject>();
            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            Animal animal = selectionTransform.GetComponent<Animal>();
            StorageBox storageBox = selectionTransform.GetComponent<StorageBox>();
            Campfire campfire = selectionTransform.GetComponent<Campfire>();
            Soil soil = selectionTransform.GetComponent<Soil>();


            if (animal && animal.playerInRange)
            {
                if (animal.isDead)
                {
                    interaction_text.text = "Loot";
                    interaction_Info_UI.SetActive(true);

                    centerDotImage.gameObject.SetActive(false);
                    handImage.gameObject.SetActive(true);
                    handIsVisible = true;

                    if (Input.GetMouseButtonDown(0))
                    {
                        Lootable lootable = animal.GetComponent<Lootable>();
                        Loot(lootable);
                    }
                }
                else
                {
                    interaction_text.text = animal.animalName;
                    interaction_Info_UI.SetActive(true);

                    centerDotImage.gameObject.SetActive(true);
                    handImage.gameObject.SetActive(false);
                    handIsVisible = false;

                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon() && EquipSystem.Instance.IsThereASwingLock() == false)
                    {
                        StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                    }
                }
                
            }



            if (choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }

 
            if (targetObject && targetObject.playerInRange)
            {
                interaction_text.text = targetObject.GetItemName();
                interaction_Info_UI.SetActive(true);
                targetObject.onTarget = true;
                lastTarget = selectionTransform;

                centerDotImage.gameObject.SetActive(false);
                handImage.gameObject.SetActive(true);
                handIsVisible = true;
            }

            if (storageBox && storageBox.playerInRange && PlacementSystem.Instance.inPlacementMode == false)
            {
                interaction_text.text = "Open";
                interaction_Info_UI.SetActive(true);

                selectedStorageBox = storageBox.gameObject;

                if (Input.GetMouseButtonDown(0))
                {
                    StorageManager.Instance.OpenBox(storageBox);
                }
            }

            if (campfire && campfire.playerInRange && PlacementSystem.Instance.inPlacementMode == false)
            {
                interaction_text.text = "Use";
                interaction_Info_UI.SetActive(true);

                selectedCampFire = campfire.gameObject;

                if (Input.GetMouseButtonDown(0) && campfire.isCooking == false)
                {
                    campfire.OpenUI();
                }
            }

            if (soil && soil.playerInRange)
            {
                if (soil.isEmpty && EquipSystem.Instance.isPlayerHoldingSeed())
                {

                    string seedName = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>().thisName;
                    string onlyPlantName = seedName.Split(new string[] {" Seed"}, StringSplitOptions.None)[0];

                    interaction_text.text = "Plant " + onlyPlantName;
                    interaction_Info_UI.SetActive(true);

                    if (Input.GetMouseButtonDown(0))
                    {
                        soil.PlantSeed();
                        Destroy(EquipSystem.Instance.selectedItem);
                        Destroy(EquipSystem.Instance.selectedItemModel);
                        EquipSystem.Instance.ChangeColor();
                    }

                }
                else if(soil.isEmpty)
                {
                    interaction_text.text = "Soil";
                    interaction_Info_UI.SetActive(true);
                }
                else
                {
                    if (EquipSystem.Instance.IsPlayerHoldingWateringCan())
                    {
                        if (soil.currentPlant.isWatered)
                        {
                            interaction_text.text = soil.plantName;
                            interaction_Info_UI.SetActive(true);
                        }
                        else
                        {
                            interaction_text.text = "Use Watering Can";
                            interaction_Info_UI.SetActive(true);

                            if (Input.GetMouseButtonDown(0))
                            {
                                SoundManager.Instance.PlaySound(SoundManager.Instance.wateringSound);
                                soil.currentPlant.isWatered = true;
                                soil.MakeSoilWatered();
                            }
                        }
                    }
                    else
                    {
                        interaction_text.text = soil.plantName;
                        interaction_Info_UI.SetActive(true);
                    }
                }
                
                selectedSoil = soil.gameObject;
            }





            if(targetObject == false && animal == false && choppableTree == false && storageBox == false  && campfire == false && soil == false)
            { 
                interaction_Info_UI.SetActive(false);
                centerDotImage.gameObject.SetActive(true);
                handImage.gameObject.SetActive(false);
                handIsVisible = false;

                if(lastTarget != null)
                {
                    lastTarget.GetComponent<InteractableObject>().onTarget = false;
                }

                if (selectedTree != null )
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }

                if(selectedStorageBox != null)
                {
                    selectedStorageBox = null;
                }

                if(selectedCampFire != null)
                {
                    selectedCampFire = null;
                }

                if(selectedSoil != null)
                {
                    selectedSoil = null;
                }
                
            }
 
        }


    }

    private void Loot(Lootable lootable)
    {
        if (lootable.wasLootCalculated == false)
        {
            List<LootRecieved> recievedLoot = new List<LootRecieved>();

            foreach (Lootpossibility loot in lootable.possibleLoot)
            {
                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax + 1);
                if (lootAmount != 0)
                {
                    int NUM = UnityEngine.Random.Range(0, 101);
                    if (NUM >= 0 && NUM <= loot.dropPersent)
                    {
                        LootRecieved lt = new LootRecieved();
                        lt.item = loot.item;
                        lt.amount = lootAmount;

                        recievedLoot.Add(lt);
                    }
                }
            }

            lootable.finalLoot = recievedLoot;
            lootable.wasLootCalculated = true;
        }

        // Spawn the loot
        Vector3 lootSpawnPosition = lootable.gameObject.transform.position;

        foreach (LootRecieved lootRecieved in lootable.finalLoot)
        {
            for (int i = 0; i < lootRecieved.amount; i++)
            {
                GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(lootRecieved.item.name + "_Model"),
                new Vector3(lootSpawnPosition.x, lootSpawnPosition.y + 0.2f, lootSpawnPosition.z),
                Quaternion.Euler(0, 0, 0));
            }
        }

        // If want blood Pubble stay
        if (lootable.GetComponent<Animal>())
        {
            lootable.GetComponent<Animal>().bloodPuddle.transform.SetParent(lootable.transform.parent);
        }

        // Destroy Looted Body
        Destroy(lootable.gameObject);


    }

    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        animal.TakeDamage(damage);
    }

    public void DisableSelection()
    {
        handImage.enabled = false;
        centerDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);

        canPickup = false;
    }

    public void EnableSelection()
    {
        handImage.enabled = true;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);

        canPickup = true;
    }
}